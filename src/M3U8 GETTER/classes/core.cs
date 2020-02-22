using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using M3U8_GETTER.Helper;

namespace M3U8_GETTER.classes
{
    class core
    {
        public const string VIDEO_NAME = "_video";

        public const string KEY_NAME = "_key";

        public const string GTR_NAME = "_video_url.m3u8gtr";
    }

    class m3u8Loader
    {

        /// <summary>
        /// m3u8文件里是否含有 #EXT-X-KEY
        /// </summary>
        public bool hasKey = false;

        /// <summary>
        /// 加密方式 AES-128，或者 AES-256
        /// </summary>
        public string keyMethod = "";

        /// <summary>
        /// 解密key
        /// </summary>
        public string key = "";

        /// <summary>
        /// 本地key文件地址
        /// </summary>
        public string keyFilename = "";

        /// <summary>
        /// ts文件string List
        /// </summary>
        public List<string> list;

        /// <summary>
        /// m3u8 URL地址
        /// </summary>
        public string m3u8Url = "";

        /// <summary>
        /// 本地m3u8文件地址
        /// </summary>
        public string m3u8Filename = "";

        /// <summary>
        /// ts文件URL前缀
        /// </summary>
        public string urlPrefix = "";

        /// <summary>
        /// 下载到本地的目录
        /// </summary>
        public string downloadPath = "";


        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorInfo = "";

        public bool Run(string[] args)
        {
            this.m3u8Url = args[0];
            this.downloadPath = args[1];
            this.m3u8Filename = this.downloadPath + "\\" + core.VIDEO_NAME + ".m3u8";
            if (HTMLHelper.IsExist(this.m3u8Url) == false)
            {
                this.errorInfo = "远程文件不存在！";
                return false;
            }
            Regex rg = new Regex(@"(.*)/.*\.m3u8(.*)?");
            Match mch = rg.Match(this.m3u8Url);
            if (mch.Success)
            {
                this.urlPrefix = mch.Groups[1].Value + "/";
            }
            else
            {
                this.errorInfo = "没有发现m3u8文件信息！";
                return false;
            }
            
            HttpHeader header = new HttpHeader();
            header.accept = "text/plain;";
            header.method = "GET";
            header.userAgent = HttpHeader.USER_AGENT;
            header.maxTry = 300;            
            string html = HTMLHelper.GetHtml(this.m3u8Url, new CookieContainer(), header);
            if (html.Length <= 0)
            {
                this.errorInfo = "读取m3u8文件内容失败！";
                return false;
            }
            else
            {
                //下载m3u8到本地目录
                HTMLHelper.DownloadFile(this.m3u8Url, this.m3u8Filename);

                //判断 EXT-X-KEY
                Regex _regKey = new Regex(@"#EXT-X-KEY:METHOD=(.*),URI=""(.*)""");
                Match _mchKey = _regKey.Match(html);
                if(_mchKey.Success)
                {
                    this.hasKey = true;
                    this.keyMethod = _mchKey.Groups[1].Value;
                    this.keyFilename = this.downloadPath + "\\" + core.KEY_NAME + ".m3u8";
                    HTMLHelper.DownloadFile(this.urlPrefix + _mchKey.Groups[2].Value, this.keyFilename);
                }
                else
                {
                    this.hasKey = false;
                    this.keyMethod = "";
                    this.key = "";
                    this.keyFilename = "";
                }

                //本地 video.m3u8 文件内容替换
                if (File.Exists(this.m3u8Filename))
                {
                    string strContent = File.ReadAllText(this.m3u8Filename);
                    strContent = Regex.Replace(strContent, @"#EXT-X-KEY:METHOD=(.*),URI=""(.*)""", "#EXT-X-KEY:METHOD=$1,URI=\"" + this.keyFilename.Replace("\\", "/") + "\"");
                    strContent = Regex.Replace(strContent, @"(.*\.ts)(.*)?", this.downloadPath.Replace("\\", "/") + "/$1");
                    File.WriteAllText(this.m3u8Filename, strContent);
                }

                Regex reg = new Regex(@".*\.ts");
                MatchCollection ms = reg.Matches(html);
                if (ms.Count > 0)
                {
                    this.list = new List<string> { };
                    foreach (Match m in ms)
                    {
                        this.list.Add(this.urlPrefix + m.Value);
                    }
                    this.errorInfo = "";
                    return true;
                }
                else
                {
                    this.errorInfo = "没有发现任何ts文件！";
                    return false;
                }
            }
        }
    }

}
