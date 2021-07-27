using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace M3U8_GETTER.classes
{
    public class HTMLHelper
    {
        /// <summary>
        /// 获取CooKie
        /// </summary>
        /// <param name="loginUrl"></param>
        /// <param name="postdata"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static CookieContainer GetCooKie(string loginUrl, string postdata, HttpHeader header)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(loginUrl);
                request.Method = header.Method;
                request.ContentType = header.ContentType;
                byte[] postdatabyte = Encoding.UTF8.GetBytes(postdata);     //提交的请求主体的内容
                request.ContentLength = postdatabyte.Length;    //提交的请求主体的长度
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;

                //提交请求
                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length);     //带上请求主体
                stream.Close();

                //接收响应
                response = (HttpWebResponse)request.GetResponse();      //正式发起请求
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

                CookieCollection cook = response.Cookies;
                //Cookie字符串格式
                string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);

                return cc;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取html
        /// </summary>
        /// <param name="getUrl"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            System.Net.ServicePointManager.DefaultConnectionLimit = 30000;
            System.GC.Collect();
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.ContentType = "application/octet-stream";
            httpWebRequest.ServicePoint.ConnectionLimit = 30000;
            httpWebRequest.Referer = url;
            httpWebRequest.Accept = "text/html,application/xhtml+xml,image/webp,image/apng,image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            httpWebRequest.UserAgent = HttpHeader.USER_AGENT;
            httpWebRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 15000;
            httpWebRequest.Proxy = null;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ReadWriteTimeout = 150000;
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetHtml Failed! " + e.Message);
                return string.Empty;
            }
            finally
            {
                if(httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
            }
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destFilename"></param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string destFilename)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.DefaultConnectionLimit = 30000;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };
            System.GC.Collect();
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.ContentType = "application/octet-stream";
            httpWebRequest.ServicePoint.ConnectionLimit = 30000;
            httpWebRequest.Referer = url;
            httpWebRequest.Accept = "text/html,application/xhtml+xml,image/webp,image/apng,image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            httpWebRequest.UserAgent = HttpHeader.USER_AGENT;
            httpWebRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 15000;
            httpWebRequest.Proxy = null;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ReadWriteTimeout = 150000;
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                responseStream.ReadTimeout = 15000;
                Stream stream = new FileStream(destFilename, FileMode.Create);
                byte[] bArr = new byte[1024];
                int _downLength = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (_downLength > 0)
                {
                    stream.Write(bArr, 0, _downLength);
                    _downLength = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return true;
            }
            catch(Exception e)
            {
                httpWebRequest.Abort();
                Console.WriteLine("DownloadFile Failed! ：" + url + " ： " + e.Message);
                return false;
            }
            finally
            {
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
            }
        }


        public static bool IsExist(string url)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "HEAD";
                req.Timeout = 15000;
                res = (HttpWebResponse)req.GetResponse();
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                    res = null;
                }
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }            
        }

        public static long GetContentLength(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                request.Referer = url.ToString();
                request.Method = "HEAD";
                request.UserAgent = HttpHeader.USER_AGENT;
                request.ContentType = "application/octet-stream";
                request.Accept = "text/html,application/xhtml+xml,image/webp,image/apng,image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Timeout = 15000;
                request.AllowAutoRedirect = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return response.ContentLength;
                    }
                    else
                    {
                        throw new Exception("服务器返回状态失败,StatusCode:" + response.StatusCode);
                    }
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }
        }
    }

    public class HttpHeader
    {
        public const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

        public string ContentType { get; set; }

        public string Accept { get; set; }

        public string UserAgent { get; set; }

        public string Method { get; set; }

        public int MaxTry { get; set; }
    }
}
