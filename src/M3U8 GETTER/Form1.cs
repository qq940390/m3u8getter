using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Taskbar;
using M3U8_GETTER.Helper;
using M3U8_GETTER.classes;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;
using System.Collections;

namespace M3U8_GETTER
{

    // 1.定义委托  
    public delegate void DelReadStdOutput(string result);
    public delegate void DelReadErrOutput(string result);

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

        private Thread thd = null;

        /// <summary>
        /// ffmpeg 命令窗口的ID
        /// </summary>
        private int ffmpegid = -1;

        private int downloadFailNums = 0;

        public static string argFilename = "";

        private string appPath = "";

        private string mySiteLink = "http://wujinhai.cn";

        private const string APPINFOS = "d3VqaW5oYWl8fCg5NDAzOTBAcXEuY29tKXx8aHR0cDovL3d1amluaGFpLmNuL3Byb2R1Y3Rpb25zLzY2Lmh0bWx8fE0zVTggR0VUVEVSIDEuMCBCZXRhfHzngrnlh7vmo4Dmn6Xmm7TmlrB8fGtsODIzc3NrbGxrc2dkODk=";

        // 2.定义委托事件  
        public event DelReadStdOutput ReadStdOutput;
        public event DelReadErrOutput ReadErrOutput;

        //任务栏进度条的实现。
        private TaskbarManager windowsTaskbar = TaskbarManager.Instance;

        public Form1()
        {
            InitializeComponent();
            appPath = Path.GetDirectoryName(Application.ExecutablePath);
            Init();            
        }

        private void exitAppFunc()
        {
            if (thd != null && thd.IsAlive)
            {
                thd.Abort();
            }
            SaveSettings();
        }


        delegate void showLogCallback(string text);

        public void showLog(string text)
        {
            if (this.logBox.InvokeRequired)
            {
                showLogCallback d = new showLogCallback(showLog);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.logBox.AppendText(DateTime.Now.ToString() + "  " + text + "\n");
                this.logBox.Focus();
            }
        }

        delegate void showProgressCallback(int percent);

        public void showProgress(int percent)
        {
            if (this.DownloadProgress.InvokeRequired)
            {
                showProgressCallback d = new showProgressCallback(showProgress);
                this.Invoke(d, new object[] { percent });
            }
            else
            {
                this.DownloadProgress.Value = percent;
                windowsTaskbar.SetProgressValue(percent, 100, this.Handle);
            }
        }

        delegate void enableBtnCallback(bool v);

        public void enableBtn(bool v)
        {
            if (this.InvokeRequired)
            {
                enableBtnCallback d = new enableBtnCallback(enableBtn);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                this.url.ReadOnly = !v;
                this.downloadPath.ReadOnly = !v;
                this.changePathBtn.Enabled = v;
                this.startDownload.Enabled = v;
                this.makeVideo.Enabled = v;
            }
        }

        private string CheckDirAndAutoMake(string currentDir, string m3u8Url)
        {
            if (Directory.Exists(currentDir))
            {
                //取文件夹最后部分，转为数字并加 1
                int dirNum = int.Parse(currentDir.Split(Path.DirectorySeparatorChar).Last());
                dirNum += 1;
                string newDir = Regex.Replace(currentDir, @"(.*)\\.*$", "$1\\" + dirNum.ToString());

                //判断是否存在 _video_url.m3u8gtr
                if(File.Exists(currentDir + "\\" + core.GTR_NAME))
                { 
                    XmlDocument doc = new XmlDocument();
                    doc.Load(currentDir + "\\" + core.GTR_NAME);    //加载Xml文件  
                    XmlNode element = doc.SelectSingleNode("Settings");
                    if (element != null)
                    {
                        string existsUrl = element.SelectSingleNode("Url").InnerText;
                        if(existsUrl == m3u8Url)
                        {
                            return currentDir;
                        }
                        else
                        {
                            return CheckDirAndAutoMake(newDir, m3u8Url);
                        }
                    }
                    else
                    {
                        return currentDir;
                    }               
                }
                else
                {
                    return currentDir;
                }
            }
            else
            {
                return currentDir;
            }            
        }

        private void startDownload_Click(object sender, EventArgs e)
        {
            downloadFailNums = 0;
            string ext = "";
            if (videoTypeMP4.Checked == true) { ext = ".mp4"; }
            if (videoTypeMKV.Checked == true) { ext = ".mkv"; }
            if (videoTypeTS.Checked == true) { ext = ".ts"; }
            if (videoTypeFLV.Checked == true) { ext = ".flv"; }

            //如果勾选了 自动递增创建文件夹
            if (chkAutoMakeDir.Checked)
            {
                string newDir = CheckDirAndAutoMake(downloadPath.Text, url.Text.Trim());
                downloadPath.Text = newDir;
            }
            if (File.Exists(downloadPath.Text + "\\" + core.VIDEO_NAME + ext) && (int)MessageBox.Show(@"当前下载目录中已经存在 " + core.VIDEO_NAME + ext + " ，是否覆盖？", "覆盖提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1) { return; }                             

            this.logBox.Text = "";
            if (!Directory.Exists(downloadPath.Text))//若文件夹不存在则新建文件夹   
            {
                showLog("创建文件夹");
                Directory.CreateDirectory(downloadPath.Text); //新建文件夹   
            }

            //将信息写入下载目录
            #region core.GTR_NAME
            string ExtendName = "";
            if (videoTypeMP4.Checked == true) { ExtendName = "MP4"; }
            if (videoTypeMKV.Checked == true) { ExtendName = "MKV"; }
            if (videoTypeTS.Checked == true) { ExtendName = "TS"; }
            if (videoTypeFLV.Checked == true) { ExtendName = "FLV"; }

            XmlTextWriter xml = new XmlTextWriter(downloadPath.Text + "\\" + core.GTR_NAME, Encoding.UTF8);
            xml.Formatting = Formatting.Indented;
            xml.WriteStartDocument();
            xml.WriteStartElement("Settings");

            xml.WriteStartElement("Url"); xml.WriteCData(url.Text.Trim()); xml.WriteEndElement();
            xml.WriteStartElement("DownPath"); xml.WriteCData(downloadPath.Text); xml.WriteEndElement();
            xml.WriteStartElement("ExtendName"); xml.WriteCData(ExtendName); xml.WriteEndElement();

            xml.WriteEndElement();
            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();
            #endregion

            //复制 bat 文件到下载目录
            if (!File.Exists(downloadPath.Text + "\\_双击运行我删除多余文件最后删除我.bat"))
            {
                File.Copy(appPath + @"\tools\_双击运行我删除多余文件最后删除我.bat", downloadPath.Text + "\\_双击运行我删除多余文件最后删除我.bat");
            }

            showLog("开始处理");
            enableBtn(false);
            thd = new Thread(new ParameterizedThreadStart(ThreadFunc));
            thd.Start(new string[] { this.url.Text.Trim(), downloadPath.Text });
        }

        public void ThreadFunc(object text)
        {
            showLog("分析及下载m3u8文件");
            m3u8Loader loader = new m3u8Loader();
            bool rt = loader.Run((string[])text);
            if(rt == true && loader.errorInfo.Length == 0)
            {
                showLog("开始下载所有TS文件片段...");
                int _percent = 0;
                int tsCount = loader.list.Count;
                string shortName = "";
                int tryTimes = 0;
                for (int i = 0; i < tsCount; i++)
                {
                    shortName = Path.GetFileName(loader.list[i].ToString());
                    if (File.Exists(loader.downloadPath + "\\" + shortName))
                    {
                        showLog("TS片段 " + shortName + " 已存在！");
                        tryTimes = 0;
                    }
                    else
                    {
                        showLog("下载 " + shortName + "..." + (tryTimes > 0 ? " 重试第 " + tryTimes.ToString() + " 次" : ""));
                        if(!HTMLHelper.DownloadFile(loader.list[i].ToString(), loader.downloadPath + "\\" + shortName))
                        {
                            showLog(shortName + " 下载失败！");
                            if(tryTimes < 3)
                            {
                                tryTimes += 1;
                                i -= 1;
                            }
                            else
                            {
                                tryTimes = 0;
                                downloadFailNums += 1;
                            }
                        }
                        else
                        {
                            tryTimes = 0;
                        }
                    }
                    _percent = i * 100 / tsCount;
                    showProgress(_percent);
                }
                Thread.Sleep(1000);
                //判断已下载的ts片段数量是否和m3u8中的一致
                if (Directory.GetFiles(loader.downloadPath + "\\", "*.ts").Length < tsCount)
                {
                    showLog(@"TS片段尚未完全下载！ " + downloadFailNums.ToString() + " 个片段未下载！");
                    showLog(@"请点击""开始下载""按钮重新下载！");
                }
                else
                {
                    showLog("TS片段全部下载完成！");
                    showLog("FFmpeg 处理中...");
                    showProgress(100);
                    this.runMake(loader.m3u8Filename, loader.downloadPath);
                }
            }
            else
            {
                showLog(loader.errorInfo);
            }
            enableBtn(true);
        }

        private void runMake(string m3u8Filename, string downloadPath)
        {
            string commandText = "";
            if (videoTypeMP4.Checked == true)
            {
                showLog("调用 FFmpeg 生成 " + core.VIDEO_NAME + ".mp4");
                commandText = "-threads 2 -i " + "\"" + m3u8Filename + "\"" + " -c copy -y -bsf:a aac_adtstoasc -movflags +faststart " + "\"" + downloadPath + "\\" + core.VIDEO_NAME + ".mp4" + "\"";
                showLog("命令： ffmpeg " + commandText);
                //启动进程执行相应命令,此例中以执行ffmpeg.exe为例  
                RealAction(appPath + @"\tools\ffmpeg.exe", commandText);
            }
            if (videoTypeMKV.Checked == true)
            {
                showLog("调用 FFmpeg 生成 " + core.VIDEO_NAME + ".mkv");
                commandText = "-threads 2 -i " + "\"" + m3u8Filename + "\"" + " -c copy -y -bsf:a aac_adtstoasc " + "\"" + downloadPath + "\\" + core.VIDEO_NAME + ".mkv" + "\"";
                showLog("命令： ffmpeg " + commandText);
                RealAction(appPath + @"\tools\ffmpeg.exe", commandText);
            }
            if (videoTypeTS.Checked == true)
            {
                showLog("调用 FFmpeg 生成 " + core.VIDEO_NAME + ".ts");
                commandText = "-threads 2 -i " + "\"" + m3u8Filename + "\"" + " -c copy -y -f mpegts " + "\"" + downloadPath + "\\" + core.VIDEO_NAME + ".ts" + "\"";
                showLog("命令： ffmpeg " + commandText);
                RealAction(appPath + @"\tools\ffmpeg.exe", commandText);
            }
            if (videoTypeFLV.Checked == true)
            {
                showLog("调用 FFmpeg 生成 " + core.VIDEO_NAME + ".flv");
                commandText = "-threads 2 -i " + "\"" + m3u8Filename + "\"" + " -c copy -y -f f4v -bsf:a aac_adtstoasc " + "\"" + downloadPath + "\\" + core.VIDEO_NAME + ".flv" + "\"";
                showLog("命令： ffmpeg " + commandText);
                RealAction(appPath + @"\tools\ffmpeg.exe", commandText);
            }            
        }

        private void RealAction(string StartFileName, string StartFileArg)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令  
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数  

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入  
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  

            CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件  
            CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件  

            CmdProcess.Start();
            ffmpegid = CmdProcess.Id;//获取ffmpeg.exe的进程ID
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。  
            //CmdProcess.WaitForExit();
            //CmdProcess.Close();
        }

        public void Stop()
        {
            AttachConsole(ffmpegid);
            SetConsoleCtrlHandler(IntPtr.Zero, true);
            GenerateConsoleCtrlEvent(0, 0);
            FreeConsole();
        }

        private void Init()
        {
            //3.将相应函数注册到委托事件中  
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
            ReadErrOutput += new DelReadErrOutput(ReadErrOutputAction);
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                // 4. 异步调用，需要invoke  
                this.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.Invoke(ReadErrOutput, new object[] { e.Data });
            }
        }

        private void ReadStdOutputAction(string result)
        {
            showLog(result);
        }

        private void ReadErrOutputAction(string result)
        {
            showLog(result);
        }

        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            showLog(".......................");            
            showLog("恭喜恭喜！影片处理完成！");            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] decodeStr = Regex.Split(Base64Helper.Decode(APPINFOS), @"\|\|", RegexOptions.IgnoreCase);
            ArrayList appInfos = new ArrayList(decodeStr);            
            this.mySiteLink = appInfos[2].ToString();
            this.Text = appInfos[3].ToString();
            this.linkLabel.Text = appInfos[0].ToString() + " " + appInfos[1].ToString() + " " + appInfos[4].ToString(); 
            windowsTaskbar.SetProgressState(TaskbarProgressBarState.Normal, this.Handle);
            windowsTaskbar.SetProgressValue(0, 100, this.Handle);
            if (!File.Exists(appPath + @"\tools\ffmpeg.exe"))  //判断程序目录有无ffmpeg.exe
            {
                MessageBox.Show("没有找到tools\\ffmpeg.exe", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Dispose();
                Application.Exit();
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M3U8_Getter_Settings.xml"))  //判断程序目录有无配置文件，并读取文件
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M3U8_Getter_Settings.xml");    //加载Xml文件  
                XmlNode element = doc.SelectSingleNode("Settings");
                downloadPath.Text = element.SelectSingleNode("DownPath").InnerText;
                string _extName = element.SelectSingleNode("ExtendName").InnerText;
                string _amd = element.SelectSingleNode("AutoMakeDir").InnerText;
                if (_extName == "MP4") { videoTypeMP4.Checked = true; }
                if (_extName == "MKV") { videoTypeMKV.Checked = true; }
                if (_extName == "TS") { videoTypeTS.Checked = true; }
                if (_extName == "FLV") { videoTypeFLV.Checked = true; }
                if(_amd == "1") { chkAutoMakeDir.Checked = true; }
            }
            else  //若无配置文件，获取当前程序运行路径，即为默认下载路径
            {
                string _path = Environment.CurrentDirectory;
                downloadPath.Text = _path;
            }

            if (argFilename != "")
            {
                //处理xml内容
                XmlDocument doc = new XmlDocument();
                doc.Load(argFilename);    //加载Xml文件  
                XmlNode element = doc.SelectSingleNode("Settings");
                if (element != null)
                {
                    url.Text = element.SelectSingleNode("Url").InnerText.Trim();
                    downloadPath.Text = element.SelectSingleNode("DownPath").InnerText;
                    string _extName = element.SelectSingleNode("ExtendName").InnerText;
                    if (_extName == "MP4") { videoTypeMP4.Checked = true; }
                    if (_extName == "MKV") { videoTypeMKV.Checked = true; }
                    if (_extName == "TS") { videoTypeTS.Checked = true; }
                    if (_extName == "FLV") { videoTypeFLV.Checked = true; }
                }
            }
        }
        private void SaveSettings()
        {
            string ExtendName = "";
            if (videoTypeMP4.Checked == true) { ExtendName = "MP4"; }
            if (videoTypeMKV.Checked == true) { ExtendName = "MKV"; }
            if (videoTypeTS.Checked == true) { ExtendName = "TS"; }
            if (videoTypeFLV.Checked == true) { ExtendName = "FLV"; }
            string amd = chkAutoMakeDir.Checked ? "1" : "0";

            XmlTextWriter xml = new XmlTextWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M3U8_Getter_Settings.xml", Encoding.UTF8);
            xml.Formatting = Formatting.Indented;
            xml.WriteStartDocument();
            xml.WriteStartElement("Settings");

            xml.WriteStartElement("DownPath"); xml.WriteCData(downloadPath.Text); xml.WriteEndElement();
            xml.WriteStartElement("ExtendName"); xml.WriteCData(ExtendName); xml.WriteEndElement();
            xml.WriteStartElement("AutoMakeDir"); xml.WriteCData(amd); xml.WriteEndElement();

            xml.WriteEndElement();
            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();
        }

        private void exitApp_Click(object sender, EventArgs e)
        {
            this.exitAppFunc();
            try
            {
                if (Process.GetProcessById(ffmpegid) != null)
                {
                    if (MessageBox.Show("已启动FFmpeg进程，确认退出吗？\n（这有可能是强制的）", "请确认您的操作", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Stop();
                        MessageBox.Show("已经发送命令！\n若进程仍然存在则强制结束！", "请确认");
                        try
                        {
                            if (Process.GetProcessById(ffmpegid) != null)  //如果进程还存在就强制结束它
                            {
                                Process.GetProcessById(ffmpegid).Kill();
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.exitAppFunc();
            try
            {
                if (Process.GetProcessById(ffmpegid) != null)
                {
                    if (MessageBox.Show("已启动FFmpeg进程，确认退出吗？\n（这有可能是强制的）", "请确认您的操作", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Stop();
                        MessageBox.Show("已经发送命令！\n若进程仍然存在则强制结束！", "请确认");
                        try
                        {
                            if (Process.GetProcessById(ffmpegid) != null)  //如果进程还存在就强制结束它
                            {
                                Process.GetProcessById(ffmpegid).Kill();
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

        private void changePathBtn_Click(object sender, EventArgs e)
        {
            if (changeFolderDialog.ShowDialog() == DialogResult.OK)
            {
                downloadPath.Text = changeFolderDialog.SelectedPath;
            }
        }

        private void openPathBtn_Click(object sender, EventArgs e)
        {
            Process.Start(downloadPath.Text);
        }

        private void makeVideo_Click(object sender, EventArgs e)
        {
            string ext = "";
            if (videoTypeMP4.Checked == true) { ext = ".mp4"; }
            if (videoTypeMKV.Checked == true) { ext = ".mkv"; }
            if (videoTypeTS.Checked == true) { ext = ".ts"; }
            if (videoTypeFLV.Checked == true) { ext = ".flv"; }

            if (File.Exists(downloadPath.Text + "\\" + core.VIDEO_NAME + ext) && (int)MessageBox.Show(@"当前下载目录中已经存在 " + core.VIDEO_NAME + ext + " ，是否覆盖？", "覆盖提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1) { return; }
            logBox.Text = "";
            if (File.Exists(downloadPath.Text + "\\video.m3u8"))
            {
                this.runMake(downloadPath.Text + "\\video.m3u8", downloadPath.Text);
            }
            else
            {
                MessageBox.Show("下载目录 video.m3u8 文件不存在，不能处理视频！");
            }
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
                if (extName == ".txt" || extName == ".m3u8gtr")
                {
                    string txtPath = Path.GetDirectoryName(dropFilename);
                    string fileContent = File.ReadAllText(dropFilename);
                    //如果文件内容是xml格式
                    if(Regex.Match(fileContent, @"<\?xml").Success)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(dropFilename);    //加载Xml文件  
                        XmlNode element = doc.SelectSingleNode("Settings");
                        if (element != null)
                        {
                            url.Text = element.SelectSingleNode("Url").InnerText.Trim();
                            downloadPath.Text = element.SelectSingleNode("DownPath").InnerText;
                            string _extName = element.SelectSingleNode("ExtendName").InnerText;
                            if (_extName == "MP4") { videoTypeMP4.Checked = true; }
                            if (_extName == "MKV") { videoTypeMKV.Checked = true; }
                            if (_extName == "TS") { videoTypeTS.Checked = true; }
                            if (_extName == "FLV") { videoTypeFLV.Checked = true; }
                        }
                    }
                    else
                    {
                        //将文件内容读取到地址栏中
                        url.Text = fileContent.Trim();
                        downloadPath.Text = txtPath;
                    }
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

        private void regFileTypeBtn_Click(object sender, EventArgs e)
        {
            if ((int)MessageBox.Show(@"注册文件类型后，可双击""m3u8gtr""文件类型打开直接下载，确定继续？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != 1) { return; }
            FileTypeRegInfo fileTypeRegInfo = new FileTypeRegInfo(".m3u8gtr"); //文件类型信息
            fileTypeRegInfo.Description = "M3U8 GETTER 视频下载启动文件";
            fileTypeRegInfo.ExePath = Application.ExecutablePath;
            fileTypeRegInfo.ExtendName = ".m3u8gtr";
            fileTypeRegInfo.IcoPath = Application.ExecutablePath; //文件图标使用应用程序的
            if (!FileTypeRegister.FileTypeRegistered(".m3u8gtr")) //如果文件类型没有注册，则进行注册
            {
                FileTypeRegister fileTypeRegister = new FileTypeRegister(); //注册
                FileTypeRegister.RegisterFileType(fileTypeRegInfo);

                /*Process[] process = Process.GetProcesses(); //重启Explorer进程，使更新立即生效
                var p = (from proc in process
                         where proc.ProcessName.Equals("explorer")
                         select proc).FirstOrDefault();
                p.Kill();*/
            }
            else
            {
                FileTypeRegister.UpdateFileTypeRegInfo(fileTypeRegInfo);
            }
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
            this.linkLabel.Links[0].LinkData = mySiteLink;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
