using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace M3U8_GETTER.classes
{

    public class ThreadInfo
    {
        public int M3u8ItemId;
        public int Id;
        public Task TaskThread;
        public ListenerMessage Msg;
        public FileDownloader Downloader;
    }

    /// <summary>
    /// 文件批量下载管理类
    /// 支持设置同时下载文件的数量
    /// </summary>
    public class DownloadFile
    {

        /// <summary>
        /// 单个片段开几个线程下载
        /// </summary>
        public int ThreadNum = 1;

        /// <summary>
        /// 每个文件的线程信息数组
        /// </summary>
        public Dictionary<int, ThreadInfo> ThreadInfos = new Dictionary<int, ThreadInfo>();

        /// <summary>
        /// 当前ts片段序号
        /// </summary>
        public int M3u8ItemId = 0;

        /// <summary>
        /// 片段文件总数
        /// </summary>
        public int FileCount = 0;

        /// <summary>
        /// 已下载片段数
        /// </summary>
        public int DoneCount = 0;

        /// <summary>
        /// m3u8文件序号
        /// </summary>
        public int M3u8InstanceId { get; set; }

        public DownloadFile()
        {
            DoSendMsg += Change;
        }

        private void Change(ListenerMessage _msg)
        {
            if (_msg.Status == ListenerStatus.Error || _msg.Status == ListenerStatus.End)
            {
                StartDown(1, _msg.M3u8ItemId);
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="_url">URL</param>
        /// <param name="_dir">本地路径</param>
        /// <param name="_id">ts 片段的索引值</param>
        /// <param name="_filename">短文件名</param>
        public void AddFile(string _url, string _dir, int _id = 0, string _filename = "")
        {
            ThreadInfo ti = new ThreadInfo
            {
                Id = _id,
                TaskThread = new Task(() =>
                {
                    //Console.WriteLine(_url);
                    Download(_url, _dir, _id, _filename);
                })
            };
            if (ThreadInfos.ContainsKey(_id))
            {
                ThreadInfos[_id] = ti;
            }
            else
            {
                ThreadInfos.Add(_id, ti);
            }
        }

        public void RetryFile(string _url, string _dir, int _id = 0, string _filename = "")
        {
            if(ThreadInfos[_id].TaskThread != null)
            {
                ThreadInfos[_id].TaskThread.Dispose();
            }
            ThreadInfo ti = new ThreadInfo
            {
                Id = _id,
                TaskThread = new Task(() =>
                {
                    Console.WriteLine("RetryFile：" + _filename);
                    Download(_url, _dir, _id, _filename);
                })
            };
            if (ThreadInfos.ContainsKey(_id))
            {
                ThreadInfos[_id] = ti;
            }
            else
            {
                ThreadInfos.Add(_id, ti);
            }
        }

        /// <summary>
        /// 开始下载所有文件，支持设置同时下载文件个数
        /// </summary>
        /// <param name="_startNum">同时下载文件个数</param>
        /// <param name="_m3u8ItemId">m3u8ItemId</param>
        public void StartDown(int _startNum = 3, int _m3u8ItemId = 0)
        {
            if (!Core.Started) return;
            for (int i2 = 0; i2 < _startNum; i2++)
            {
                lock (ThreadInfos)
                {
                    foreach (var item in ThreadInfos)
                    {
                        if (item.Value.TaskThread.Status == TaskStatus.Created)
                        {
                            item.Value.M3u8ItemId = _m3u8ItemId;
                            item.Value.TaskThread.Start();
                            break;
                        }
                    }
                }
            }
        }

        public delegate void DlgSendMsg(ListenerMessage _msg);
        public event DlgSendMsg DoSendMsg;

        /// <summary>
        /// 单个文件下载
        /// </summary>
        /// <param name="_url">URL</param>
        /// <param name="_dir">本地路径</param>
        /// <param name="id">ts 片段的索引值</param>
        /// <param name="_filename">短文件名</param>
        private void Download(string _url, string _dir, int id = 0, string _filename = "")
        {
            try
            {
                ThreadInfos[id].Msg = new ListenerMessage
                {
                    M3u8ItemId = ThreadInfos[id].M3u8ItemId,
                    Id = id,
                    Url = _url,
                    Filename = _filename,
                    Fullpath = Path.Combine(_dir, _filename),
                    Status = ListenerStatus.Start,
                    TimeoutNums = 0
                };
                ThreadInfos[id].Downloader = new FileDownloader(_url, _dir, _filename, ThreadNum);
                ThreadInfos[id].Msg.ContentLength = ThreadInfos[id].Downloader.FileSize;
                ThreadInfos[id].Downloader.Data.Clear();
                DoSendMsg(ThreadInfos[id].Msg);
                DownloadProgressListener linstenter = new DownloadProgressListener(ThreadInfos[id].Msg)
                {
                    DoSendMsg = new DownloadProgressListener.DlgSendMsg(DoSendMsg)
                };
                if (ThreadInfos[id].Downloader.AllDone == true)
                {
                    linstenter.NotifyDownloadedSize(ThreadInfos[id].Msg.ContentLength, 1000, 1);//通知已完成
                }
                else
                {
                    //改用任务异步启动
                    Task.Factory.StartNew(() => ThreadInfos[id].Downloader.StartDownload(linstenter));
                }
            }
            catch (Exception ex)
            {
                if (ThreadInfos[id] == null) return;
                if (ThreadInfos[id].Msg == null)
                {
                    ThreadInfos[id].Msg = new ListenerMessage();
                }
                ThreadInfos[id].Msg.Id = id;
                ThreadInfos[id].Msg.Url = _url;
                ThreadInfos[id].Msg.Filename = _filename;
                ThreadInfos[id].Msg.Fullpath = Path.Combine(_dir, _filename);
                ThreadInfos[id].Msg.Status = ListenerStatus.Error;
                ThreadInfos[id].Msg.ContentLength = 0;
                ThreadInfos[id].Msg.ErrMessage = ex.Message;
                DoSendMsg(ThreadInfos[id].Msg);

                Console.WriteLine("单个文件下载线程终止 : " + ex.Message);
            }
        }

    }

}
