using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{

    public class ThreadArgs
    {
        public FileDownloader Downloader;
        public string Url;
        public string SaveFile;
        public long FileSize;
        public long BlockSize;
        public long DownSize;
        public int ThreadId;
    }

    /// <summary>
    /// 单文件下载类，支持多线程分片段下载
    /// </summary>
    public class FileDownloader
    {
        /// <summary>
        /// 线程数
        /// </summary>
        private DownloadThread[] Threads;

        public static List<Task> TaskList = new List<Task>();

        /// <summary>
        /// 本地保存文件
        /// </summary>
        private string SaveFile;
        /// <summary>
        /// 缓存各线程下载的长度
        /// </summary>
        public Dictionary<int, long> Data = new Dictionary<int, long>();
        /// <summary>
        /// 每条线程下载的长度
        /// </summary>
        private long ThreadBlockSize;
        /// <summary>
        /// 下载路径
        /// </summary>
        private String DownloadUrl;

        public long DownloadLength { get; set; }

        /// <summary>
        /// 原始文件长度
        /// </summary>
        /// <returns>获取文件大小</returns>
        public long FileSize { get; set; }

        public bool AllDone { get; set; }

        public uint StartTime { get; set; }

        public uint UsedTime { get; set; }

        public int TimeoutTimes { get; set; }

        public bool IsGziped { get; set; }

        /// <summary>
        ///  获取线程数
        /// </summary>
        /// <returns> 获取线程数</returns>
        public int GetThreadsCount()
        {
            return Threads.Length;
        }

        /// <summary>
        /// 累计已下载大小
        /// </summary>
        /// <param name="_downLength">累计已下载大小</param>
        public void Append(long _downLength)
        {
            lock (this)  //锁定同步..............线程开多了竟然没有同步起来.文件下载已经完毕了,下载总数量却不等于文件实际大小,找了半天原来这里错误的
            {
                DownloadLength += _downLength;
            }
        }

        /// <summary>
        /// 更新指定线程最后下载的位置
        /// </summary>
        /// <param name="_threadId">ThreadId 线程id</param>
        /// <param name="_pos">最后下载的位置</param>
        public void Update(int _threadId, long _pos)
        {
            if (Data.ContainsKey(_threadId))
            {
                this.Data[_threadId] = _pos;
            }
            else
            {
                this.Data.Add(_threadId, _pos);
            }
        }

        /// <summary>
        /// 构建下载准备,获取文件大小
        /// </summary>
        /// <param name="_downloadUrl">下载路径</param>
        /// <param name="_fileSaveDir"> 文件保存目录</param>
        /// <param name="_threadNum">下载线程数</param>
        public FileDownloader(string _downloadUrl, string _fileSaveDir, string _filename= "", int _threadNum = 3)
        {
            if (String.IsNullOrEmpty(_downloadUrl)) return;
            //这里先填充数据，以防止出现超时的异常时，引发“未将对象引用设置到对象的实例”异常
            this.Threads = new DownloadThread[_threadNum];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_downloadUrl);
            try
            {
                if (string.IsNullOrEmpty(_filename))
                {
                     _filename = Uri.UnescapeDataString(Path.GetFileName(_downloadUrl));//获取文件名称 uri 解码中文字符
                    if (_filename.Length == 0) throw new Exception("获取文件名失败");
                }
                //构建http 请求
                this.DownloadUrl = _downloadUrl;
                if (!Directory.Exists(_fileSaveDir)) Directory.CreateDirectory(_fileSaveDir);
                this.SaveFile = Path.Combine(_fileSaveDir, _filename); //构建保存文件

                request.Referer = _downloadUrl.ToString();
                request.Method = "HEAD";
                request.UserAgent = HttpHeader.USER_AGENT;
                request.ContentType = "application/octet-stream";
                request.Accept = "text/html,application/xhtml+xml,image/webp,image/apng,image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Timeout = 5000;
                request.AllowAutoRedirect = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //ContentLength = -1 则表示，资源经过gzip压缩，需要一直获取知道 stream 为0
                        //ContentLength = 0 则表示远程文件不存在
                        this.FileSize = response.ContentLength;//根据响应获取文件大小

                        //如果本地文件存在，并且和 ContentLength 相同，则跳过
                        if (File.Exists(this.SaveFile) && Core.GetFilesize(this.SaveFile) == this.FileSize)
                        {
                            this.AllDone = true;
                            return;
                        }

                        if (this.FileSize == -1)
                        {
                            //如果文件大小未知，则只启用单线程下载
                            this.Threads = new DownloadThread[1];
                            IsGziped = true;
                            //假设文件大小为 50M
                            this.FileSize = 50 * 1024 * 1024;
                        }
                        else
                        {
                            IsGziped = false;
                            this.Threads = new DownloadThread[_threadNum];
                        }

                        //计算每条线程下载的数据长度
                        this.ThreadBlockSize = (this.FileSize % this.Threads.Length) == 0 ? this.FileSize / this.Threads.Length : this.FileSize / this.Threads.Length + 1;
                    }
                    else
                    {
                        throw new Exception("服务器返回状态失败,StatusCode:" + response.StatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                this.FileSize = 0;
                IsGziped = false;
                Console.WriteLine("无法连接下载地址： " + e.Message);
            }
            finally
            {
                if (request != null) request.Abort();
            }
        }

        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <param name="_listener">监听下载数量的变化,如果不需要了解实时下载的数量,可以设置为null</param>
        /// <returns>已下载文件大小</returns>
        public long StartDownload(DownloadProgressListener _listener)
        {
            if (String.IsNullOrEmpty(this.DownloadUrl)) return -1;
            if (this.AllDone)
            {
                if (_listener != null)
                {
                    _listener.NotifyDownloadedSize(this.DownloadLength, this.UsedTime, TimeoutTimes);//通知目前已经下载完成的数据长度
                }
                return this.DownloadLength;
            }
            if (_listener != null) _listener.Msg.IsGziped = IsGziped;
            this.StartTime = Core.GetTickTime();
            this.UsedTime = 0;
            try
            {
                //todo 异常，未引用实例
                int thdLen = this.Threads.Length;
                //建立要保存的空文件
                using (FileStream fstream = new FileStream(this.SaveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fstream.Close();
                }
                if (this.Data.Count != thdLen)
                {
                    this.Data.Clear();
                    for (int i = 0; i < thdLen; i++)
                    {
                        this.Data.Add(i + 1, 0);//初始化每条线程已经下载的数据长度为0
                    }
                }

                for (int i = 0; i < thdLen; i++)
                {//开启线程进行下载
                    long _downLength = this.Data[i + 1];
                    if (_downLength < this.ThreadBlockSize && this.DownloadLength < this.FileSize)
                    {//判断线程是否已经完成下载,否则继续下载
                        // Console.WriteLine("Threads" + i.ToString() + ",下载块" + this.ThreadBlockSize.ToString() + "    " + this.Data[i + 1].ToString() + "              " + DownloadLength.ToString());
                        this.Threads[i] = new DownloadThread(this, this.DownloadUrl, this.SaveFile, this.FileSize, this.ThreadBlockSize, this.Data[i + 1], i + 1);
                        this.Threads[i].ThreadRun();
                    }
                }
                bool notFinish = true;//下载未完成
                bool isTimeout = false;
                TimeoutTimes = 0;
                while (notFinish) // 循环判断所有线程是否完成下载
                {
                    if (!Core.Started) break;
                    Thread.Sleep(1000);
                    this.UsedTime = Core.GetTickTime() - this.StartTime;
                    notFinish = false;//假定全部线程下载完成
                    for (int i = 0; i < thdLen; i++)
                    {
                        if (!Core.Started) break;
                        //Console.WriteLine(Path.GetFileNameWithoutExtension(this.SaveFile) + " ： " + this.Threads[i].Finish.ToString());
                        if (this.Threads[i] != null && !this.Threads[i].Finish)
                        {//如果发现线程未完成下载
                            notFinish = true;//设置标志为下载没有完成
                            if (this.Threads[i].DownLength == -1)
                            {//如果下载失败,再重新下载
                                this.Threads[i] = new DownloadThread(this, this.DownloadUrl, this.SaveFile, this.FileSize, this.ThreadBlockSize, this.Data[i + 1], i + 1);
                                this.Threads[i].ThreadRun();
                                isTimeout = true;
                            }
                        }
                        else if (this.Threads[i] == null)
                        {
                            notFinish = false;//假定全部线程下载完成
                            this.Threads[i] = new DownloadThread(this, this.DownloadUrl, this.SaveFile, this.FileSize, this.ThreadBlockSize, this.Data[i + 1], i + 1);
                            this.Threads[i].ThreadRun();
                        }
                    }
                    if(isTimeout)
                    {
                        isTimeout = false;
                        TimeoutTimes += 1;
                        //Console.WriteLine(Path.GetFileName(SaveFile) + " 重试第 " + TimeoutTimes.ToString() + " 次");
                    }
                    if (_listener != null)
                    {
                        if (!notFinish && IsGziped) _listener.Msg.ContentLength = this.DownloadLength;
                        _listener.NotifyDownloadedSize(this.DownloadLength, this.UsedTime, TimeoutTimes);//通知目前已经下载完成的数据长度
                        //Console.WriteLine(this.DownloadLength);
                    }
                }
                if(this.IsGziped)
                {
                    if (this.DownloadLength > 0) this.AllDone = true;
                    else this.AllDone = false;
                }
                else
                {
                    if (this.DownloadLength > this.FileSize) this.AllDone = true;
                    else this.AllDone = false;
                }
            }
            catch (Exception e)
            {
                this.DownloadLength = -1;
                this.AllDone = false;
                Console.WriteLine("下载文件失败[" + TimeoutTimes.ToString() + "]： " + e.Message);
            }
            return this.DownloadLength;
        }
    }
}
