using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Taskbar;
using M3U8_GETTER.classes;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;

namespace M3U8_GETTER
{

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(IntPtr handlerRoutine, bool add);
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// 启动参数数组
        /// </summary>
        private string[] Args;

        private string ConfigFile = "";

        private string AppPath = "";

        private string TmpPath = "";

        private string MySiteLink = "http://wujinhai.cn/productions/66.html";

        //任务栏进度条的实现。
        public TaskbarManager WindowsTaskbar = TaskbarManager.Instance;

        public ArrayList LogList = null;

        public List<M3u8ItemInfo> M3u8ItemInfos = new List<M3u8ItemInfo>();
        public List<DownloadItemInfo> DownloadItemInfos = new List<DownloadItemInfo>();


        public Form1(string[] args)
        {
            Args = args;
            InitializeComponent();
            AppPath = Path.GetDirectoryName(Application.ExecutablePath);
            this.ConfigFile = AppPath + @"\config.ini";
            LogList = new ArrayList();
        }

        private void ExitAppFunc()
        {
            Core.Started = false;
            SaveSettings();
            try
            {
                for (int i = 0; i < M3u8ItemInfos.Count; i++)
                {
                    int _pId = M3u8ItemInfos[i].FfmpegProcessId;
                    if (Process.GetProcessById(_pId) != null)
                    {
                        Stop(_pId);
                        try
                        {
                            if (Process.GetProcessById(_pId) != null)  //如果进程还存在就强制结束它
                            {
                                Process.GetProcessById(_pId).Kill();
                                Dispose();
                                Application.Exit();
                            }
                        }
                        catch
                        {
                            Dispose();
                            Application.Exit();
                        }
                    }
                }
            }
            catch
            {
                Dispose();
                Application.Exit();
            }
        }

        public void CheckAndMake(object _obj)
        {
            int _itemId = Convert.ToInt32(_obj);
            BeginInvoke(new ShowProgressDelegate(ShowProgress), 100);
            BeginInvoke(new ShowLogDelegate(ShowLog), "开始处理视频！");

            M3u8ItemInfo _mii = GetM3u8ItemById(_itemId);
            if (_mii == null) return;

            //如果存在本地key，则复制本地key到临时目录
            if (File.Exists(_mii.Loader.SourceKeyFilePath))
            {
                M3U8Loader.CopyKeyFile(_mii.Loader.SourceKeyFilePath, _mii.Loader.Md5KeyFilePath);
            }

            string m3u8Filename = _mii.Loader.M3u8Md5FilePath;
            string downloadPath = _mii.Md5Path;
            string commandText = "";
            if (rbtnVideoTypeMP4.Checked == true)
            {
                BeginInvoke(new ShowLogDelegate(ShowLog), "调用 FFmpeg 生成 " + Core.VIDEO_NAME + ".mp4");
                //commandText = "-allowed_extensions ALL -protocol_whitelist \"file,http,crypto,tcp\" -i " + "\"" + m3u8Filename + "\"" + " -vcodec copy -acodec copy -absf aac_adtstoasc " + "\"" + downloadPath + "\\" + Core.VIDEO_NAME + ".mp4" + "\"";
                commandText = Core.GetCommandText(m3u8Filename, downloadPath + "\\" + Core.VIDEO_NAME + ".mp4");
                BeginInvoke(new ShowLogDelegate(ShowLog), "命令： ffmpeg " + commandText);
                //启动进程执行相应命令,此例中以执行ffmpeg.exe为例
                RealAction(AppPath + @"\tools\ffmpeg.exe", commandText, _itemId);
            }
        }

        public delegate void ShowLogDelegate(string text);
        public void ShowLog(string text)
        {
            try
            {
                string log = DateTime.Now.ToLongTimeString() + " ： " + text;
                LogList.Add(log);
                if (LogList.Count > 50)
                {
                    LogList.RemoveAt(0);
                }
                rtxtLogs.Lines = (string[])LogList.ToArray(typeof(string));
                rtxtLogs.Select(rtxtLogs.TextLength, 0);
                //滚动到控件光标处
                rtxtLogs.ScrollToCaret();
            } catch(Exception e) { }
        }

        public delegate void ShowProgressDelegate(int percent);
        public void ShowProgress(int percent)
        {
            percent = percent > 100 ? 100 : percent;
            this.DownloadProgress.Value = percent;
            WindowsTaskbar.SetProgressValue(percent, 100, this.Handle);
        }

        public delegate void EnableBtnDelegate(bool v);
        public void EnableBtn(bool v)
        {
            this.btnStartDownload.Enabled = v;
            this.btnStop.Enabled = !v;
        }

        public delegate void CurrentDownListTextDelegate(string _text);
        public void CurrentDownListText(string _text)
        {
            tpCurrentDownList.Text = _text;
        }

        public delegate void M3u8FileListTextDelegate(string _text);
        public void M3u8FileListText(string _text)
        {
            tpM3u8FileList.Text = _text;
        }

        /// <summary>
        /// 获取m3u8待下载项索引
        /// </summary>
        /// <param name="_id">序号</param>
        /// <returns>int</returns>
        public int GetM3u8ItemIndexById(int _id)
        {
            return M3u8ItemInfos.FindIndex(item => item.Id == _id);
        }

        /// <summary>
        /// 获取m3u8待下载项
        /// </summary>
        /// <param name="_id">序号</param>
        /// <returns>M3u8ItemInfo|null</returns>
        public M3u8ItemInfo GetM3u8ItemById(int _id)
        {
            int _index = M3u8ItemInfos.FindIndex(item => item.Id == _id);
            if (_index >= 0) return M3u8ItemInfos[_index];
            return null;
        }

        public int GetM3u8ItemLastId()
        {
            return M3u8ItemInfos.Count == 0 ? 0 : M3u8ItemInfos.Last().Id;
        }

        public delegate void SetM3u8DownloadItemShortnameDelegate(int _id, string _newName);
        /// <summary>
        /// 设置m3u8待下载列表当前下载项的文件名
        /// </summary>
        /// <param name="_id">序号</param>
        /// <param name="_newName">新文件名</param>
        public void SetM3u8DownloadItemShortname(int _id, string _newName)
        {
            int _index = GetM3u8ItemIndexById(_id);
            if (_index >= 0)
            {
                M3u8ItemInfos[_index].ShortName = _newName;
            }
            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
        }

        public delegate void SetM3u8DownloadItemStatusDelegate(int _id, string _status);
        /// <summary>
        /// 设置m3u8待下载列表当前下载项的状态
        /// </summary>
        /// <param name="_id">序号</param>
        /// <param name="_status">状态文本</param>
        public void SetM3u8DownloadItemStatus(int _id, string _status)
        {
            int _index = GetM3u8ItemIndexById(_id);
            if (_index >= 0)
            {
                M3u8ItemInfos[_index].Status = _status;                
            }
            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
        }

        /// <summary>
        /// 设置m3u8待下载列表当前下载项的视频状态
        /// </summary>
        /// <param name="_id">序号</param>
        /// <param name="_status">状态文本</param>
        public void SetM3u8DownloadItemVideoStatus(int _id, string _status)
        {
            int _index = GetM3u8ItemIndexById(_id);
            if (_index >= 0)
            {
                M3u8ItemInfos[_index].VideoStatus = _status;
            }
            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
        }

        public void RemoveM3u8DownloadItemById(int _id)
        {
            lock (M3u8ItemInfos)
            {
                int _index = GetM3u8ItemIndexById(_id);
                if (_index >= 0)
                {
                    M3u8ItemInfos.RemoveAt(_index);
                }
            }
            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
        }

        public delegate void RemoveDownloadItemByIdDelegate(int _id);
        public void RemoveDownloadItemById(int _id)
        {
            lock (DownloadItemInfos)
            {
                int _index = DownloadItemInfos.FindIndex(item => item.Id == _id);
                if (_index >= 0)
                {
                    DownloadItemInfos.RemoveAt(_index);
                }
            }
        }

        /// <summary>
        /// 根据序号获取下载队列项
        /// </summary>
        /// <param name="_id">序号</param>
        /// <returns>DownloadItemInfo|null</returns>
        public DownloadItemInfo GetDownloadListItemById(int _id)
        {
            lock (DownloadItemInfos)
            {
                int _index = DownloadItemInfos.FindIndex(item => item.Id == _id);
                if (_index >= 0)
                {
                    return DownloadItemInfos[_index];
                }
            }
            return null;
        }

        /// <summary>
        /// 设置下载队列的指定序号的子项文本
        /// </summary>
        /// <param name="_id">序号</param>
        /// <param name="_name">属性名</param>
        /// <param name="_text">文本数据</param>
        public void SetStatusListItemText(int _id, string _name, string _text)
        {
            DownloadItemInfo item = GetDownloadListItemById(_id);
            if (item != null)
            {
                switch (_name)
                {
                    case "s":
                        break;

                    case "Status":
                        item.Status = _text;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 检测m3u8待下载列表中是否有重复的URL
        /// </summary>
        /// <param name="_url">URL</param>
        /// <returns>Boolean</returns>
        public bool CheckM3U8ListUrlRepeat(string _url)
        {
            _url = _url.Trim();
            if(M3u8ItemInfos.FindIndex(item => item.Url == _url) >= 0) return true;
            return false;
        }

        public void M3U8ListAddItem(string _shortName, string _downPath, string _url)
        {
            //判断是否是本地文件
            if (!Regex.IsMatch(_url, @"^http.*"))
            {
                if (!File.Exists(_url)) return;
            }
            if (!CheckM3U8ListUrlRepeat(_url))
            {
                lock (M3u8ItemInfos)
                {
                    M3u8ItemInfo mi = new M3u8ItemInfo
                    {
                        Id = GetM3u8ItemLastId() + 1,
                        ShortName = _shortName,
                        VideoPath = _downPath,
                        Md5Path = Path.Combine(this.TmpPath, Md5Helper.Md5(_url.Trim())),
                        Url = _url.Trim(),
                        Status = "等待下载",
                        VideoStatus = "",
                        State = ItemInfoState.Unstarted
                    };
                    //初始化m3u8 Loader 类
                    mi.Loader.Init(mi.Url, mi.VideoPath, mi.Md5Path, ".mp4", (mi.Id - 1) * 10000 + 1);                                        
                    M3u8ItemInfos.Add(mi);
                }
            }
        }

        private void btnStartDownload_Click(object sender, EventArgs e)
        {
            lock (DownloadItemInfos)
            {
                //清理当前下载列表
                DownloadItemInfos.Clear();
                //BeginInvoke(new DownloadListRefreshDelegate(DownloadListRefresh));
            }
            Core.command = txtFfmpegCommand.Text;
            this.TmpPath = txtTempPath.Text;
            if (this.TmpPath == "")
            {
                MessageBox.Show("请设置临时文件夹！");
                return;
            }
            if (M3u8ItemInfos.Count == 0)
            {
                MessageBox.Show("m3u8待下载列表为空，请添加文件！");
                return;
            }

            Core.Started = true;
            timrRefresh.Enabled = true;
            Core.M3u8ListDownMax = Convert.ToInt32(txtM3u8ListDownMax.Text != "" ? txtM3u8ListDownMax.Text  : "1");
            Core.M3u8FileDownMax = Convert.ToInt32(txtM3u8FileDownMax.Text != "" ? txtM3u8FileDownMax.Text : "3");
            Core.M3u8FileDownMax = Core.M3u8FileDownMax < 0 ? 3 : Core.M3u8FileDownMax;
            Core.FileThreadMax = Convert.ToInt32(txtFileThreadMax.Text != "" ? txtFileThreadMax.Text : "1");
            Core.FileThreadMax = Core.FileThreadMax < 0 ? 1 : Core.FileThreadMax;

            BeginInvoke(new EnableBtnDelegate(EnableBtn), false);
            //清理所有 DownloadFile 实例
            //Core.M3u8Instances.Clear();
            DoStart(Core.M3u8ListDownMax);
        }

        /// <summary>
        /// 开始下载启动函数
        /// </summary>
        public void DoStart(int StartNum = 1)
        {
            if (M3u8ItemInfos.Count == 0) return;
            BeginInvoke(new ShowProgressDelegate(ShowProgress), 0);
            int notDoneNum = M3u8ItemInfos.Count;
            for (int i = 0; i < M3u8ItemInfos.Count; i++)
            {
                if (M3u8ItemInfos[i].State == ItemInfoState.Finished)
                {
                    notDoneNum -= 1;
                }
            }
            for (int n = 0; n < StartNum; n++)
            {
                int _count = M3u8ItemInfos.Count;
                for (int i = 0; i < _count; i++)
                {
                    if (M3u8ItemInfos[i].State == ItemInfoState.Unstarted)
                    {
                        if (M3u8ItemInfos[i].Status != "下载完成" && M3u8ItemInfos[i].Status != "下载失败")
                        {
                            //把批量下载类实例进行保存
                            M3u8ItemInfos[i].DownloadInstance = new DownloadFile
                            {
                                M3u8InstanceId = M3u8ItemInfos[i].Id, //存入要下载的序号
                                FileCount = 0,
                                DoneCount = 0,
                                ThreadNum = Core.FileThreadMax   //线程数，不设置默认为3
                            };
                            M3u8ItemInfos[i].State = ItemInfoState.Started;
                            M3u8ItemInfos[i].Status = "开始下载";
                            M3u8ItemInfos[i].DownloadInstance.DoSendMsg += SendMsgHander;//下载过程处理事件

                            //用线程启动批量下载的开始函数
                            Task.Factory.StartNew(() => DoStartThreadFunc(M3u8ItemInfos[i].Id));
                            //ThreadPool.QueueUserWorkItem(DoStartThreadFunc, M3u8ItemInfos[i].Id);
                        }
                        break;
                    }
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            }
            if (notDoneNum <= 0)
            {
                Core.Started = false;
                BeginInvoke(new EnableBtnDelegate(EnableBtn), true);
                BeginInvoke(new M3u8FileListTextDelegate(M3u8FileListText), "M3U8 待下载列表");
                BeginInvoke(new ShowLogDelegate(ShowLog), "M3U8 全部下载完成！");
            }
            else
            {
                BeginInvoke(new M3u8FileListTextDelegate(M3u8FileListText), "M3U8 待下载列表(剩余: " + notDoneNum.ToString() + ")");
            }
        }

        /// <summary>
        /// 开始下载线程
        /// </summary>
        public void DoStartThreadFunc(object _obj)
        {
            int _itemId = Convert.ToInt32(_obj);
            int _itemIdIndex = GetM3u8ItemIndexById(_itemId);
            if (_itemIdIndex < 0)
            {
                DoStart();
                return;
            }
            BeginInvoke(new ShowLogDelegate(ShowLog), "开始下载： " + M3u8ItemInfos[_itemIdIndex].Url);

            if (!Directory.Exists(M3u8ItemInfos[_itemIdIndex].VideoPath))//若文件夹不存在则新建文件夹
            {
                Directory.CreateDirectory(M3u8ItemInfos[_itemIdIndex].VideoPath); //新建文件夹
            }

            if (!Directory.Exists(M3u8ItemInfos[_itemIdIndex].Md5Path))//若文件夹不存在则新建临时文件夹
            {
                Directory.CreateDirectory(M3u8ItemInfos[_itemIdIndex].Md5Path); //新建临时文件夹
            }

            //如果临时目录已存在合成好的视频文件，则复制并清理临时目录
            if (File.Exists(M3u8ItemInfos[_itemIdIndex].Loader.Md5VideoFilePath) && Core.GetFilesize(M3u8ItemInfos[_itemIdIndex].Loader.Md5VideoFilePath) > 1000)
            {
                string _videoFile = Core.GetNumberFormatName(M3u8ItemInfos[_itemIdIndex].Loader.VideoFilePath);
                File.Copy(M3u8ItemInfos[_itemIdIndex].Loader.Md5VideoFilePath, _videoFile, true);
                M3u8ItemInfos[_itemIdIndex].Status = "下载完成";
                M3u8ItemInfos[_itemIdIndex].VideoStatus = "处理完成";
                ClearM3u8TempFiles(_videoFile, M3u8ItemInfos[_itemIdIndex].Md5Path, M3u8ItemInfos[_itemIdIndex].Loader.M3u8Url, M3u8ItemInfos[_itemIdIndex].Loader.SourceKeyFilePath);
                M3u8ItemInfos[_itemIdIndex].DownloadInstance = null;
                
                DoStart();
                return;
            }
            else
            {
                //特殊情况下，复制本地key到临时目录
                if (File.Exists(M3u8ItemInfos[_itemIdIndex].Loader.SourceKeyFilePath))
                {
                    M3U8Loader.CopyKeyFile(M3u8ItemInfos[_itemIdIndex].Loader.SourceKeyFilePath, M3u8ItemInfos[_itemIdIndex].Loader.Md5KeyFilePath);
                }

                //处理 m3u8 文件，分析 片段
                bool rt = M3u8ItemInfos[_itemIdIndex].Loader.Run();
                if (M3u8ItemInfos[_itemIdIndex].Loader.IsLocalFile == false)
                {
                    BeginInvoke(new SetM3u8DownloadItemShortnameDelegate(SetM3u8DownloadItemShortname), _itemId, M3u8ItemInfos[_itemIdIndex].Loader.ShortName);
                }
                //rt = false m3u8文件处理失败，则不执行
                if (rt == true && M3u8ItemInfos[_itemIdIndex].Loader.ErrorInfo.Length == 0)
                {
                    string _shortName = "";
                    int _itemIndex = M3u8ItemInfos[_itemIdIndex].Loader.StartIndex;
                    int _count = M3u8ItemInfos[_itemIdIndex].Loader.FilesList.Count;
                    for (int i = 0; i < _count; i++)
                    {
                        M3u8ItemInfos[_itemIdIndex].DownloadInstance.FileCount += 1;
                        _shortName = Core.TrimFilename(Path.GetFileName(M3u8ItemInfos[_itemIdIndex].Loader.FilesList[i]));
                        lock (DownloadItemInfos)
                        {
                            DownloadItemInfos.Add(new DownloadItemInfo
                            {
                                Id = _itemIndex,
                                Url = M3u8ItemInfos[_itemIdIndex].Loader.FilesList[i],
                                ShortName = _shortName,
                                FileSizeInfo = "0",
                                DownloadSizeInfo = "0",
                                ProgressInfo = "0",
                                UsedTime = "0",
                                Status = "等待下载"
                            });
                        }
                        //批量添加要下载文件
                        M3u8ItemInfos[_itemIdIndex].DownloadInstance.AddFile(M3u8ItemInfos[_itemIdIndex].Loader.FilesList[i], M3u8ItemInfos[_itemIdIndex].Md5Path, _itemIndex, _shortName);
                        _itemIndex += 1;
                    }

                    //启动实例的开始下载函数
                    M3u8ItemInfos[_itemIdIndex].DownloadInstance.StartDown(Core.M3u8FileDownMax, _itemId);
                }
                else
                {
                    M3u8ItemInfos[_itemIdIndex].Status = "下载失败";
                    BeginInvoke(new ShowLogDelegate(ShowLog), M3u8ItemInfos[_itemIdIndex].Loader.ErrorInfo);
                    //下载m3u8文件失败，开启下一个下载
                    DoStart();
                }
            }

            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            //BeginInvoke(new DownloadListRefreshDelegate(DownloadListRefresh));
        }

        //复制文件和清理临时文件线程函数
        public void ClearM3u8TempFiles(string _videoFile, string _md5Path, string _m3u8Source, string _keyFile)
        {
            //当最终视频目录有视频文件，且不为空时，清理相关临时文件
            if (chkDeleteTmp.Checked && File.Exists(_videoFile) && Core.GetFilesize(_videoFile) > 1000)
            {
                try
                {
                    Core.DeletePath(_md5Path, true);
                    DirectoryInfo dir = new DirectoryInfo(_md5Path);
                    dir.Delete(true); //删除临时目录

                    //删除 源m3u8 key 等文件
                    if (chkDeleteTmp.Checked)
                    {
                        if (File.Exists(_m3u8Source))
                        {
                            File.Delete(_m3u8Source);
                        }
                        if (File.Exists(_keyFile))
                        {
                            File.Delete(_keyFile);
                        }
                    }
                }
                catch (Exception e)
                {
                    BeginInvoke(new ShowLogDelegate(ShowLog), _videoFile + " 清理临时文件失败！" + e.Message);
                }
            }
        }

        public void RealAction(string StartFileName, string StartFileArg, int _itemId)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口 **
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = false;  // 重定向输入
            CmdProcess.StartInfo.RedirectStandardOutput = false; // 重定向标准输出
            CmdProcess.StartInfo.RedirectStandardError = false;  // 重定向错误输出
            CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件
            //这里使用匿名代理方式，可以传递参数
            CmdProcess.Exited += (s, arg) =>
            {
                CmdProcess_Exited(_itemId);
            };

            CmdProcess.Start();
            int _index = GetM3u8ItemIndexById(_itemId);
            if(_index >= 0) M3u8ItemInfos[_index].FfmpegProcessId = CmdProcess.Id;//获取ffmpeg.exe的进程ID
        }

        public void Stop(int _id)
        {
            AttachConsole(_id);
            SetConsoleCtrlHandler(IntPtr.Zero, true);
            GenerateConsoleCtrlEvent(0, 0);
            FreeConsole();
        }

        public void CmdProcess_Exited(int _itemId)
        {
            int _itemIdIndex = GetM3u8ItemIndexById(_itemId);
            if (_itemIdIndex < 0) return;
            Stop(M3u8ItemInfos[_itemIdIndex].FfmpegProcessId);

            BeginInvoke(new ShowLogDelegate(ShowLog), ".......................");
            string md5VideoFile = M3u8ItemInfos[_itemIdIndex].Loader.Md5VideoFilePath;
            string videoFile = Core.GetNumberFormatName(M3u8ItemInfos[_itemIdIndex].Loader.VideoFilePath);
            if (File.Exists(md5VideoFile) && Core.GetFilesize(md5VideoFile) > 1000)
            {
                BeginInvoke(new ShowLogDelegate(ShowLog), "恭喜恭喜！影片处理完成！");
                SetM3u8DownloadItemVideoStatus(M3u8ItemInfos[_itemIdIndex].Id, "处理完成");
                File.Copy(md5VideoFile, videoFile, true);
                ClearM3u8TempFiles(videoFile, M3u8ItemInfos[_itemIdIndex].Md5Path, M3u8ItemInfos[_itemIdIndex].Loader.M3u8Url, M3u8ItemInfos[_itemIdIndex].Loader.SourceKeyFilePath);
                M3u8ItemInfos[_itemIdIndex].DownloadInstance = null;
            }
            else
            {
                BeginInvoke(new ShowLogDelegate(ShowLog), "···【影片处理失败】···");
                SetM3u8DownloadItemVideoStatus(M3u8ItemInfos[_itemIdIndex].Id, "处理失败");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (!File.Exists(AppPath + @"\tools\ffmpeg.exe"))  //判断程序目录有无ffmpeg.exe
            {
                MessageBox.Show("没有找到tools\\ffmpeg.exe", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if ((Args != null) && (Args.Length > 0))
            {
                int length = Args.Length;
                for (int i = 0; i < length; i++)
                {
                    //判断文件后缀格式
                    string ext = Path.GetExtension(Args[i]);
                    if (ext == ".m3u8")
                    {
                        string _downloadPath = Path.GetDirectoryName(Args[i]);
                        M3U8ListAddItem(Path.GetFileName(Core.TrimFilename(Args[i])), _downloadPath, Args[i].Trim());
                    }
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            }
            this.LoadSettings();

            WindowsTaskbar.SetProgressState(TaskbarProgressBarState.Normal, this.Handle);
            WindowsTaskbar.SetProgressValue(0, 100, this.Handle);
        }

        private void LoadSettings()
        {
            this.Left = Convert.ToInt32(Ini.ReadIni("Settings", "FormLeft", "500", 255, this.ConfigFile));
            this.Top = Convert.ToInt32(Ini.ReadIni("Settings", "FormTop", "255", 255, this.ConfigFile));
            //this.Width = Convert.ToInt32(Ini.ReadIni("Settings", "FormWidth", "900", 255, this.ConfigFile));
            //this.Height = Convert.ToInt32(Ini.ReadIni("Settings", "FormHeight", "600", 255, this.ConfigFile));
            txtDownloadPath.Text = Ini.ReadIni("Settings", "DownloadPath", Application.StartupPath, 255, this.ConfigFile);
            txtTempPath.Text = Ini.ReadIni("Settings", "TempPath", @"C:\m3u8_getter_temp", 255, this.ConfigFile);
            chkDeleteTmp.Checked = Convert.ToBoolean(Ini.ReadIni("Settings", "DeleteTmp", "True", 255, this.ConfigFile));
            txtM3u8ListDownMax.Text = Ini.ReadIni("Settings", "M3u8ListDownMax", "1", 255, this.ConfigFile);
            txtM3u8FileDownMax.Text = Ini.ReadIni("Settings", "M3u8FileDownMax", "3", 255, this.ConfigFile);
            txtFileThreadMax.Text = Ini.ReadIni("Settings", "FileThreadMax", "1", 255, this.ConfigFile);
            txtFfmpegCommand.Text = Ini.ReadIni("Settings", "FfmpegCommand", "-allowed_extensions ALL -protocol_whitelist \"file,http,crypto,tcp\" -i \"{{m3u8filename}}\" -vcodec copy -acodec copy -absf aac_adtstoasc \"{{mp4filename}}\"", 255, this.ConfigFile);
            Core.command = txtFfmpegCommand.Text;
            this.TmpPath = txtTempPath.Text;
        }

        private void SaveSettings()
        {
            string delTmp = chkDeleteTmp.Checked ? "True" : "False";

            Ini.WriteIni("Settings", "FormLeft", this.Left.ToString(), this.ConfigFile);
            Ini.WriteIni("Settings", "FormTop", this.Top.ToString(), this.ConfigFile);
            Ini.WriteIni("Settings", "FormWidth", this.Width.ToString(), this.ConfigFile);
            Ini.WriteIni("Settings", "FormHeight", this.Height.ToString(), this.ConfigFile);
            Ini.WriteIni("Settings", "DownloadPath", txtDownloadPath.Text, this.ConfigFile);
            Ini.WriteIni("Settings", "TempPath", txtTempPath.Text, this.ConfigFile);
            Ini.WriteIni("Settings", "DeleteTmp", delTmp, this.ConfigFile);
            Ini.WriteIni("Settings", "M3u8ListDownMax", txtM3u8ListDownMax.Text, this.ConfigFile);
            Ini.WriteIni("Settings", "M3u8FileDownMax", txtM3u8FileDownMax.Text, this.ConfigFile);
            Ini.WriteIni("Settings", "FileThreadMax", txtFileThreadMax.Text, this.ConfigFile);
            Ini.WriteIni("Settings", "FfmpegCommand", txtFfmpegCommand.Text, this.ConfigFile);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Core.Started = false;
            timrRefresh.Enabled = false;

            BeginInvoke(new EnableBtnDelegate(EnableBtn), true);
            int _count = M3u8ItemInfos.Count;
            for (int i = 0; i < _count; i++)
            {
                if(M3u8ItemInfos[i].State != ItemInfoState.Finished) M3u8ItemInfos[i].State = ItemInfoState.Unstarted;
            }
            //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            btnStop.Text = "停止下载";
            BeginInvoke(new ShowLogDelegate(ShowLog), "停止下载！");
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            if (changeFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtDownloadPath.Text = changeFolderDialog.SelectedPath;
            }
        }

        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            Process.Start(txtDownloadPath.Text);
        }

        private void url_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void url_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void url_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
                //获取拖拽的文件地址
                string dropFilename = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                string extName = Path.GetExtension(dropFilename).ToLower();
                if(extName == ".m3u8")
                {
                    //将文件名填入地址栏中
                    txtUrl.Text = dropFilename.Trim();
                }
            }
        }

        private void url_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;
            if (e.KeyChar == (char)1)       // Ctrl-A 相当于输入了AscII=1的控制字符
            {
                textBox.SelectAll();
                e.Handled = true;      // 不再发出“噔”的声音
            }
        }

        private void btnRegFileType_Click(object sender, EventArgs e)
        {
            if ((int)MessageBox.Show(@"注册文件类型后，可双击""m3u8""文件类型打开直接下载，确定继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1) { return; }

            FileTypeRegInfo fileTypeRegInfo = new FileTypeRegInfo(".m3u8")
            {
                Description = "M3U8 文件",
                ExePath = Application.ExecutablePath,
                ExtendName = ".m3u8",
                IcoPath = Application.ExecutablePath //文件图标使用应用程序的
            }; //文件类型信息
            if (FileTypeRegister.FileTypeRegistered(".m3u8")) //如果文件类型没有注册，则进行注册
            {
                FileTypeRegister fileTypeRegister = new FileTypeRegister(); //注册
                FileTypeRegister.RegisterFileType(fileTypeRegInfo);
            }
            else
            {
                FileTypeRegister.UpdateFileTypeRegInfo(fileTypeRegInfo);
            }

            /*Process[] process = Process.GetProcesses(); //重启Explorer进程，使更新立即生效
            var p = (from proc in process
                        where proc.ProcessName.Equals("explorer")
                        select proc).FirstOrDefault();
            p.Kill();*/

            SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
            MessageBox.Show("文件类型注册成功！");
        }

        private void url_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = tb.Text.Trim();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel.Links[0].LinkData = MySiteLink;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        //消息处理
        public void SendMsgHander(ListenerMessage msg)
        {
            if (Core.Started == false) return;
            //msg.Id = 待下载列表的序号
            int _itemIdIndex = GetM3u8ItemIndexById(msg.M3u8ItemId);
            if (_itemIdIndex < 0) return;

            DownloadItemInfo _item = GetDownloadListItemById(msg.Id);
            if (_item == null) return;
            switch (msg.Status)
            {
                case ListenerStatus.Start:
                    //开始下载
                    _item.Status = "开始下载";
                    _item.State = ItemInfoState.Started;
                    break;

                case ListenerStatus.GetLength:
                    //连接成功 msg.ContentLengthInfo
                    _item.Status = "连接成功";
                    break;

                case ListenerStatus.End:
                case ListenerStatus.DownLoad:
                    _item.FileSizeInfo = msg.IsGziped ? "未知" : msg.ContentLengthInfo;
                    _item.DownloadSizeInfo = msg.DownLengthInfo;
                    _item.ProgressInfo = msg.IsGziped ? "未知" : msg.Progress.ToString() + "%";
                    _item.UsedTime = msg.UsedTimeSecond + "秒";

                    int _percent = M3u8ItemInfos[_itemIdIndex].DownloadInstance.FileCount <= 0 ? 0 : (M3u8ItemInfos[_itemIdIndex].DownloadInstance.DoneCount * 100 / M3u8ItemInfos[_itemIdIndex].DownloadInstance.FileCount);


                    // 下载中
                    _item.State = ItemInfoState.Downloading;
                    _item.Status = "下载中(重试 " + msg.TimeoutNums.ToString() + " 次)";
                    BeginInvoke(new SetM3u8DownloadItemStatusDelegate(SetM3u8DownloadItemStatus), msg.M3u8ItemId, "下载中(" + _percent.ToString() + "%)");
                    if (msg.Status == ListenerStatus.End)
                    {
                        // 下载完成
                        M3u8ItemInfos[_itemIdIndex].DownloadInstance.DoneCount += 1;
                        _item.Status = "下载完成";
                        _item.State = ItemInfoState.Finished;
                        BeginInvoke(new ShowLogDelegate(ShowLog), msg.Filename + " 下载完成！");
                        if (M3u8ItemInfos[_itemIdIndex].DownloadInstance.DoneCount >= M3u8ItemInfos[_itemIdIndex].DownloadInstance.FileCount && Core.GetFileCount(M3u8ItemInfos[_itemIdIndex].Md5Path, ".ts") >= M3u8ItemInfos[_itemIdIndex].DownloadInstance.FileCount)
                        {
                            M3u8ItemInfos[_itemIdIndex].State = ItemInfoState.Finished;
                            BeginInvoke(new SetM3u8DownloadItemStatusDelegate(SetM3u8DownloadItemStatus), msg.M3u8ItemId, "下载完成");
                            BeginInvoke(new CurrentDownListTextDelegate(CurrentDownListText), "当前下载队列");
                            BeginInvoke(new ShowProgressDelegate(ShowProgress), _percent);
                            //Task.Factory.StartNew(() => CheckAndMake(msg.M3u8ItemId));
                            CheckAndMake(msg.M3u8ItemId);
                            DoStart();  //开始下一个m3u8下载
                        }
                        else
                        {
                            BeginInvoke(new CurrentDownListTextDelegate(CurrentDownListText), "当前下载队列(剩余: " + Convert.ToString(DownloadItemInfos.Count) + ")");
                        }
                    }
                    BeginInvoke(new ShowProgressDelegate(ShowProgress), _percent);
                    break;

                case ListenerStatus.Error:
                    if (Core.Started)
                    {
                        _item.Status = msg.ErrMessage;
                        //下载失败
                        BeginInvoke(new ShowLogDelegate(ShowLog), msg.ErrMessage);
                        //将失败的文件重新加入下载队列
                        BeginInvoke(new ShowLogDelegate(ShowLog), msg.Filename + " 下载失败，重新下载！");
                        M3u8ItemInfos[_itemIdIndex].DownloadInstance.RetryFile(_item.Url, M3u8ItemInfos[_itemIdIndex].Md5Path, _item.Id, _item.ShortName);

                    }
                    break;

            } //switch
            if (_item.State == ItemInfoState.Finished)
            {
                //已完成的从下载队列中移除 RemoveDownloadItemByIdDelegate
                //RemoveDownloadItemById(_item.Id);
                BeginInvoke(new RemoveDownloadItemByIdDelegate(RemoveDownloadItemById), _item.Id);
                //BeginInvoke(new DownloadListRefreshDelegate(DownloadListRefresh));
            }
            else
            {
                BeginInvoke(new SortDownloadItemInfosDelegate(SortDownloadItemInfos));
                //BeginInvoke(new DownloadListRefreshDelegate(DownloadListRefresh));
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitAppFunc();
        }

        private void btnOpenTmpPath_Click(object sender, EventArgs e)
        {
            Process.Start(txtTempPath.Text);
        }

        private void btnChangeTmpPath_Click(object sender, EventArgs e)
        {
            if (changeFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtTempPath.Text = changeFolderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// 拖拽m3u8文件到列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvM3U8List_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                //获取拖拽的文件地址
                int _len = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).Length;
                for (int i = 0; i < _len; i++)
                {
                    string dropFilename = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(i).ToString();
                    string extName = Path.GetExtension(dropFilename).ToLower();
                    if (extName == ".m3u8")
                    {
                        M3U8ListAddItem(Path.GetFileName(dropFilename), Path.GetDirectoryName(dropFilename), dropFilename.Trim());
                    }
                }

                e.Effect = DragDropEffects.All;
                BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            }
        }

        private void lvM3U8List_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.All;
        }

        private void lvM3U8List_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// 添加URL到列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddToList_Click(object sender, EventArgs e)
        {
            M3U8ListAddItem(Path.GetFileName(Core.TrimFilename(txtUrl.Text)), txtDownloadPath.Text, txtUrl.Text.Trim());
            BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
        }

        private void tsmitMakeVideo_Click(object sender, EventArgs e)
        {
            if (lvwM3U8List.SelectedIndices.Count >= 1)
            {
                int _id = M3u8ItemInfos[lvwM3U8List.SelectedIndices[0]].Id;
                Task thd = new Task(() =>
                {
                    this.CheckAndMake(_id);
                });
                thd.Start();
            }
        }

        private void tsmitResetStatus_Click(object sender, EventArgs e)
        {
            if (lvwM3U8List.SelectedIndices.Count >= 1)
            {
                lock (M3u8ItemInfos)
                {
                    int _index = lvwM3U8List.SelectedIndices[0];
                    M3u8ItemInfos[_index].Status = "等待下载";
                    M3u8ItemInfos[_index].VideoStatus = "";
                    if (M3u8ItemInfos[_index].State != ItemInfoState.Finished) M3u8ItemInfos[_index].State = ItemInfoState.Unstarted;
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            }
        }

        private void tsmitOpenM3u8Folder_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwM3U8List.SelectedIndices.Count >= 1)
                {
                    lock (M3u8ItemInfos)
                    {
                        int _index = lvwM3U8List.SelectedIndices[0];
                        string filepath = Path.Combine(M3u8ItemInfos[_index].VideoPath, M3u8ItemInfos[_index].ShortName);
                        if (File.Exists(filepath))
                        {
                            ShowSelectedInExplorer.FileOrFolder(filepath);
                        }
                        else
                        {
                            BeginInvoke(new ShowLogDelegate(ShowLog), "M3U8文件不存在！" + filepath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 打开视频所在目录 菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmitOpenVideoFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwM3U8List.SelectedIndices.Count >= 1)
                {
                    lock (M3u8ItemInfos)
                    {
                        int _index = lvwM3U8List.SelectedIndices[0];
                        string videoFilepath = Path.Combine(M3u8ItemInfos[_index].VideoPath, M3u8ItemInfos[_index].ShortName);
                        videoFilepath = Path.ChangeExtension(videoFilepath, ".mp4");
                        if (File.Exists(videoFilepath))
                        {
                            ShowSelectedInExplorer.FileOrFolder(videoFilepath);
                        }
                        else
                        {
                            BeginInvoke(new ShowLogDelegate(ShowLog), "视频文件不存在！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 复制M3U8源地址 菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmitCopyM3u8Url_Click(object sender, EventArgs e)
        {
            if (lvwM3U8List.SelectedIndices.Count >= 1)
            {
                Clipboard.SetText(M3u8ItemInfos[lvwM3U8List.SelectedIndices[0]].Url);
                BeginInvoke(new ShowLogDelegate(ShowLog), "已复制到剪贴板！");
            }
        }

        /// <summary>
        /// 删除所选项 菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmitDeleteSelect_Click(object sender, EventArgs e)
        {
            lock (M3u8ItemInfos) {
                int count = lvwM3U8List.SelectedIndices.Count;
                List<int> _idxs = new List<int>();
                for (int i = count - 1; i >= 0; i--)
                {
                    int _index = lvwM3U8List.SelectedIndices[i];
                    if (Core.Started && (M3u8ItemInfos[_index].Status == "等待下载" || M3u8ItemInfos[_index].Status == "下载完成" || M3u8ItemInfos[_index].Status == "下载失败"))
                    {
                        _idxs.Add(_index);
                    }
                    else if(!Core.Started)
                    {
                        _idxs.Add(_index);
                    }
                }
                for(int i = 0; i < _idxs.Count; i++)
                {
                    M3u8ItemInfos[_idxs[i]].DownloadInstance = null;
                    M3u8ItemInfos.RemoveAt(_idxs[i]);
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
                this.BeginInvoke(new Action(() => {
                    lvwM3U8List.SelectedIndices.Clear();
                }));
            }
        }

        /// <summary>
        /// 清除所有项 菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmitClearAll_Click(object sender, EventArgs e)
        {
            lock (M3u8ItemInfos)
            {
                int _count = M3u8ItemInfos.Count;
                List<int> _idxs = new List<int>();
                for (int i = 0; i < _count; i++)
                {
                    if(Core.Started && (M3u8ItemInfos[i].Status == "等待下载" || M3u8ItemInfos[i].Status == "下载完成" || M3u8ItemInfos[i].Status == "下载失败"))
                    {
                        _idxs.Add(i);
                    }
                    else if (!Core.Started)
                    {
                        _idxs.Add(i);
                    }
                }
                for (int i = _idxs.Count - 1; i >= 0; i--)
                {
                    M3u8ItemInfos[_idxs[i]].DownloadInstance = null;
                    M3u8ItemInfos.RemoveAt(_idxs[i]);
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
                this.BeginInvoke(new Action(() => {
                    lvwM3U8List.SelectedIndices.Clear();
                }));
            }
        }

        public delegate void M3U8ListRefreshDelegate();
        private void M3U8ListRefresh()
        {
            if (M3u8ItemInfos.Count == 0) return;
            this.lvwM3U8List.VirtualListSize = M3u8ItemInfos.Count;
            this.lvwM3U8List.Refresh();
        }

        public delegate void DownloadListRefreshDelegate();
        private void DownloadListRefresh()
        {
            if (DownloadItemInfos.Count == 0) return;
            this.lvwStatusList.VirtualListSize = DownloadItemInfos.Count;
            this.lvwStatusList.Refresh();
        }

        public delegate void SortDownloadItemInfosDelegate();
        /// <summary>
        /// 对 DownloadItemInfos 进行排序
        /// </summary>
        private void SortDownloadItemInfos()
        {
            lock (DownloadItemInfos)
            {
                DownloadItemInfos = DownloadItemInfos.OrderByDescending(item => item.State).ToList();
            }
        }

        private void lvwM3U8List_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (M3u8ItemInfos == null && M3u8ItemInfos.Count == 0) return;
            M3u8ItemInfo ItemInfo = e.ItemIndex >= M3u8ItemInfos.Count ? null : M3u8ItemInfos[e.ItemIndex];
            if (ItemInfo != null)
            {
                try
                {
                    ListViewItem item = new ListViewItem(ItemInfo.Id.ToString());
                    item.SubItems.Add(ItemInfo.ShortName);
                    item.SubItems.Add(ItemInfo.VideoPath);
                    item.SubItems.Add(ItemInfo.Url);
                    item.SubItems.Add(ItemInfo.Status);
                    item.SubItems.Add(ItemInfo.VideoStatus);
                    item.ImageIndex = 0;

                    e.Item = item;
                }
                catch(Exception ex)
                {
                    e.Item = null;
                }
            }
            else
            {
                e.Item = null;
            }
        }

        private void lvwStatusList_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            lock (DownloadItemInfos)
            {
                if (DownloadItemInfos == null || DownloadItemInfos.Count == 0) return;

                DownloadItemInfo ItemInfo = e.ItemIndex >= DownloadItemInfos.Count ? null : DownloadItemInfos[e.ItemIndex];
                if(ItemInfo != null)
                {
                    try
                    {
                        ListViewItem item = new ListViewItem(ItemInfo.Id.ToString());
                        item.SubItems.Add(ItemInfo.ShortName);
                        item.SubItems.Add(ItemInfo.FileSizeInfo);
                        item.SubItems.Add(ItemInfo.DownloadSizeInfo);
                        item.SubItems.Add(ItemInfo.ProgressInfo);
                        item.SubItems.Add(ItemInfo.UsedTime);
                        item.SubItems.Add(ItemInfo.Status);
                        item.ImageIndex = 0;

                        e.Item = item;
                    }
                    catch(Exception ex)
                    {
                        e.Item = null;
                    }
                }
                else
                {
                    e.Item = null;
                }
            }
        }

        private void txtUrl_Click(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;
            textBox.SelectAll();
        }

        private void tsmitClearFinished_Click(object sender, EventArgs e)
        {
            lock (M3u8ItemInfos)
            {
                int count = M3u8ItemInfos.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (M3u8ItemInfos[i].Status == "下载完成" && M3u8ItemInfos[i].VideoStatus == "处理完成")
                    {
                        M3u8ItemInfos[i].DownloadInstance = null;
                        M3u8ItemInfos.RemoveAt(i);
                    }
                }
                //BeginInvoke(new M3U8ListRefreshDelegate(M3U8ListRefresh));
            }
        }

        private void btnResetFfmpegCommand_Click(object sender, EventArgs e)
        {
            txtFfmpegCommand.Text = "-allowed_extensions ALL -protocol_whitelist \"file,http,crypto,tcp\" -i \"{{m3u8filename}}\" -vcodec copy -acodec copy -absf aac_adtstoasc \"{{mp4filename}}\"";
        }

        private void timrRefresh_Tick(object sender, EventArgs e)
        {
            M3U8ListRefresh();
            DownloadListRefresh();
        }
    }

}
