using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{
    public class DownloadThread
    {
        private string SaveFile;
        private long ThreadBlockSize;
        private int ThreadId = -1;
        private long FileSize;
        private FileDownloader Downloader;


        /// <summary>
        /// 下载是否完成
        /// </summary>
        /// <returns></returns>
        public bool Finish { get; set; }

        /// <summary>
        ///  已经下载的内容大小
        /// </summary>
        /// <returns>如果返回值为-1,代表下载失败</returns>
        public long DownLength { get; set; }

        public string Url { get; set; }

        public DownloadThread(FileDownloader _downloader, string _url, string _saveFile, long _fileSize, long _block, long _downLength, int _threadId)
        {
            this.Url = _url;
            this.SaveFile = _saveFile;
            this.ThreadBlockSize = _block;
            this.Downloader = _downloader;
            this.ThreadId = _threadId;
            this.FileSize = _fileSize;
            this.DownLength = _downLength;            
        }

        public void ThreadRun()
        {
            //使用Task启动异步下载线程
            Task.Factory.StartNew(this.ThreadRunMethod);
        }
        
        /// <summary>
        /// 异步Task方式执行
        /// </summary>
        /// <returns></returns>
        public void ThreadRunMethod()
        {
            if (this.DownLength < this.ThreadBlockSize)//未下载完成
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                WebResponse wb = null;
                try
                {
                    int startPos = (int)(this.ThreadBlockSize * (this.ThreadId - 1) + this.DownLength);//开始位置
                    int endPos = (int)(this.ThreadBlockSize * this.ThreadId - 1);//结束位置
                    //Console.WriteLine("TaskThread " + this.ThreadId + " start Download from position " + startPos + "  and endwith " + endPos);
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Referer = Url.ToString();
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                    request.ContentType = "application/octet-stream";
                    request.Accept = "text/html,application/xhtml+xml,image/webp,image/apng,image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                    request.Timeout = 10 * 1000;
                    request.ReadWriteTimeout = 30 * 1000;
                    request.AllowAutoRedirect = true;
                    request.AddRange(startPos, endPos);
                    //Console.WriteLine(request.Headers.ToString()); //输出构建的http 表头
                    response = (HttpWebResponse)request.GetResponse();
                    wb = request.GetResponse();
                    //这里超时会抛出异常
                    using (Stream _stream = wb.GetResponseStream())
                    {
                        byte[] buffer = new byte[1024 * 100]; //缓冲区大小 ，原始 new byte[1024 * 50]
                        long offset = -1;
                        using (Stream threadfile = new FileStream(this.SaveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) //设置文件以共享方式读写,否则会出现当前文件被另一个文件使用.
                        {
                            threadfile.Seek(startPos, SeekOrigin.Begin); //移动文件位置
                            while ((offset = _stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                //offset 实际下载流大小
                                this.Downloader.Append(offset); //更新已经下载当前总文件大小
                                threadfile.Write(buffer, 0, (int)offset);
                                this.DownLength += offset;  //设置当前线程已下载位置
                                this.Downloader.Update(this.ThreadId, this.DownLength);
                                if (!Core.Started) break;
                            }
                            threadfile.Close(); //using 用完后可以自动释放..手动释放一遍.木有问题的(其实是多余的)
                        }
                        _stream.Close();
                        //Console.WriteLine("TaskThread " + this.ThreadId + " Download Finish");
                        this.Finish = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("片段下载线程异常： " + this.ThreadId + ":" + e.Message);
                    this.DownLength = -1;
                }
                finally
                {
                    if (request != null) request.Abort();
                    if (response != null) {
                        response.Dispose();
                        response.Close();
                    }
                    if(wb != null)
                    {
                        wb.Dispose();
                        wb.Close();
                    }
                }
            }
        }
        
    }
}
