using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;
using System.Security.Policy;

namespace M3U8_GETTER.classes
{
    public class M3U8Loader
    {
        /// <summary>
        /// ts文件string List
        /// </summary>
        public List<string> FilesList = new List<string>();

        /// <summary>
        /// 索引初始位置
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 索引当前位置
        /// </summary>
        public int CurrentIndex { get; set; }
        
        /// <summary>
        /// 本地和m3u8同名的key文件地址
        /// </summary>
        public string SourceKeyFilePath { get; set; }

        /// <summary>
        /// 临时目录下的key文件地址
        /// </summary>
        public string Md5KeyFilePath { get; set; }

        /// <summary>
        /// 要下载的总数
        /// </summary>
        public int FilesCount { get; set; }

        /// <summary>
        /// 视频短文件名，不包含后缀
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 最终视频文件名,包含后缀
        /// </summary>
        public string VideoFilePath { get; set; }

        /// <summary>
        /// 最终视频文件格式后缀
        /// </summary>
        public string VideoExtension { get; set; }

        /// <summary>
        /// m3u8 URL地址
        /// </summary>
        public string M3u8Url { get; set; }

        /// <summary>
        /// m3u8临时目录文件地址
        /// </summary>
        public string M3u8Md5FilePath { get; set; }

        /// <summary>
        /// 临时目录视频文件名，包含后缀
        /// </summary>
        public string Md5VideoFilePath { get; set; }

        /// <summary>
        /// ts文件URL前缀
        /// </summary>
        public string UrlPrefix { get; set; }

        /// <summary>
        /// 下载到本地的目录
        /// </summary>
        public string DownloadPath { get; set; }

        /// <summary>
        /// 临时目录
        /// </summary>
        public string Md5Path { get; set; }

        /// <summary>
        /// 是否本地m3u8文件
        /// </summary>
        public bool IsLocalFile { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo { get; set; }

        /// <summary>
        /// m3u8文件里是否含有 #EXT-X-KEY
        /// </summary>
        public bool HasKey { get; set; }

        /// <summary>
        /// 加密方式 AES-128，或者 AES-256
        /// </summary>
        public string KeyMethod { get; set; }
        

        public void Init(string _url, string _path, string _md5Path, string _videoExt, int _startIndex = 0)
        {
            this.StartIndex = _startIndex;
            this.CurrentIndex = _startIndex;
            this.IsLocalFile = false;
            this.M3u8Url = _url;
            this.DownloadPath = _path;
            this.ShortName = Path.GetFileNameWithoutExtension(Regex.Replace(_url, @"(.*)\?.*", "$1")); //文件名去掉?后的内容
            this.Md5Path = _md5Path;
            this.VideoExtension = _videoExt;
            this.VideoFilePath = Path.Combine(_path, this.ShortName + _videoExt);
            this.M3u8Md5FilePath = Path.Combine(_md5Path, "_video.m3u8");
            this.Md5VideoFilePath = Path.Combine(_md5Path, Core.VIDEO_NAME + _videoExt);
            this.SourceKeyFilePath = Path.Combine(_path, this.ShortName + ".key");            
            this.Md5KeyFilePath = Path.Combine(_md5Path, "_key.m3u8");
        }

        public bool Run()
        {
            //判断是否是本地文件
            if (!Regex.IsMatch(this.M3u8Url, @"^http(s)?.*"))
            {
                if (File.Exists(this.M3u8Url))
                {
                    this.IsLocalFile = true;
                }
                else
                {
                    this.ErrorInfo = "本地文件不存在！";
                    return false;
                }
            }
            string fileContent = "";
            int _index = this.M3u8Url.LastIndexOf("/");
            this.UrlPrefix = this.M3u8Url.Substring(0, _index + 1);
            //Console.WriteLine(this.urlPrefix);

            if (this.IsLocalFile == false)
            {//网络文件
                //下载m3u8到本地目录                
                bool isOk = HTMLHelper.DownloadFile(this.M3u8Url, this.M3u8Md5FilePath);
                if(isOk == false)
                {
                    this.ErrorInfo = "读取m3u8文件内容失败！可能远程文件不存在。";
                    return false;
                }
                //如果是网络文件，同时复制一份到视频目录作为备份
                if (File.Exists(this.M3u8Md5FilePath))
                {
                    string m3u8Filename = Core.GetNumberFormatName(this.DownloadPath + "\\" + this.ShortName + ".m3u8");
                    this.ShortName = Path.GetFileName(m3u8Filename);
                    File.Copy(this.M3u8Md5FilePath, m3u8Filename, true);
                }
                fileContent = File.ReadAllText(this.M3u8Md5FilePath);
            }
            else
            {
                File.Copy(this.M3u8Url, this.M3u8Md5FilePath, true);
                fileContent = File.ReadAllText(this.M3u8Url);
            }

            //判断 EXT-X-KEY
            Regex _regKey = new Regex(@"#EXT-X-KEY:METHOD=(.*),URI=""(.*)""");
            Match _mchKey = _regKey.Match(fileContent);
            if (_mchKey.Success)
            {
                this.HasKey = true;
                this.KeyMethod = _mchKey.Groups[1].Value;
                if (Regex.IsMatch(_mchKey.Groups[2].Value, @"^http(s)?.*"))
                {
                    HTMLHelper.DownloadFile(_mchKey.Groups[2].Value, this.Md5KeyFilePath);
                }
                else
                {
                    HTMLHelper.DownloadFile(this.UrlPrefix + _mchKey.Groups[2].Value, this.Md5KeyFilePath);
                }
            }
            else
            {
                this.HasKey = false;
                this.KeyMethod = "";
                this.ErrorInfo = "没有发现 EXT-X-KEY";
            }

            //临时目录 _video.m3u8 文件内容替换
            if (File.Exists(this.M3u8Md5FilePath))
            {
                string strContent = File.ReadAllText(this.M3u8Md5FilePath);
                strContent = Regex.Replace(strContent, @"#EXT-X-KEY:METHOD=(.*),URI=""(.*)""", "#EXT-X-KEY:METHOD=$1,URI=\"" + this.Md5KeyFilePath.Replace("\\", "/") + "\"");
                
                MatchCollection matchs = Regex.Matches(strContent, @"(.*\.ts.*)");
                int _index2 = -1;
                string tmpItem = "";
                foreach (var item in matchs)
                {
                    tmpItem = item.ToString();
                    _index2 = tmpItem.LastIndexOf("/");
                    if (_index2 == -1)
                    {
                        strContent = strContent.Replace(tmpItem, this.Md5Path.Replace("\\", "/") + "/" +  Core.TrimFilename(tmpItem));
                    }
                    else
                    {
                        strContent = strContent.Replace(tmpItem, this.Md5Path.Replace("\\", "/") + Core.TrimFilename(tmpItem.Substring(_index2, tmpItem.Length - _index2)));
                    }
                }

                //如果尾部没有 #EXT-X-ENDLIST  则加上
                if (strContent.LastIndexOf("#EXT-X-ENDLIST") == -1)
                {
                    strContent += "\r\n#EXT-X-ENDLIST";
                }


                File.WriteAllText(this.M3u8Md5FilePath, strContent);
            }

            Regex reg = new Regex(@".*\.ts(\?.*)?");
            MatchCollection ms = reg.Matches(fileContent);            
            if (ms.Count > 0)
            {
                Regex http = new Regex(@"^http(s)?.*");
                string host = "";
                try
                {
                    host = new Uri(this.M3u8Url).Host;
                }
                catch(Exception e) { }

                foreach (Match m in ms)
                {
                    this.CurrentIndex += 1;
                    if (http.IsMatch(m.Value) == true)
                    {//片段是以http开头的，直接添加
                        this.FilesList.Add(m.Value);
                    }
                    else
                    {
                        if (m.Value.LastIndexOf("//") >= 0)
                        {//以 // 开头 去掉 //
                            string _tmp = Regex.IsMatch(m.Value, @"^\/(.*)") ? Regex.Replace(m.Value, @"^\/(.*)", "$1") : m.Value;
                            _tmp = Regex.IsMatch(m.Value, @"^\/(.*)") ? Regex.Replace(_tmp, @"^\/(.*)", "$1") : _tmp;
                            this.FilesList.Add("http://" + _tmp);
                        }
                        else if (m.Value.LastIndexOf("/") >= 0)
                        {//以 / 开头          
                            if (m.Value.IndexOf(host) >= 0)
                            {
                                string _tmp = Regex.IsMatch(m.Value, @"^\/(.*)") ? Regex.Replace(m.Value, @"^\/(.*)", "$1") : m.Value;
                                _tmp = Regex.IsMatch(m.Value, @"^\/(.*)") ? Regex.Replace(_tmp, @"^\/(.*)", "$1") : _tmp;
                                this.FilesList.Add("http://" + _tmp);
                            }
                            else
                            {
                                this.FilesList.Add("http://" + host + m.Value);
                            }
                        }
                        else
                        {
                            this.FilesList.Add(this.UrlPrefix + m.Value);
                        }
                    }
                }
                this.FilesCount = FilesList.Count;
                this.ErrorInfo = "";
                return true;
            }
            else
            {
                this.ErrorInfo = "没有发现任何ts文件！";
                return false;
            }
        }

        public static void CopyKeyFile(string _source, string _dest)
        {
            if (!File.Exists(_source)) return;
            string content = File.ReadAllText(_source);
            if(content.IndexOf("==") > 0)
            {
                byte[] byteStr = Convert.FromBase64String(content);
                //创建一个文件流
                FileStream fs = new FileStream(_dest, FileMode.Create);

                //将byte数组写入文件中
                fs.Write(byteStr, 0, byteStr.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fs.Close();
            } 
            else
            {
                File.Copy(_source, _dest, true);
            }            
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量128</param>
        /// <param name="strKey">key</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] inputdata, byte[] iv, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 32));
            des.IV = iv;
            byte[] decryptBytes = new byte[inputdata.Length];
            using (MemoryStream ms = new MemoryStream(inputdata))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return decryptBytes;
        }
    }
}
