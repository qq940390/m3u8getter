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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M3U8_GETTER.classes
{
    class Core
    {
        public const string VIDEO_NAME = "_video";

        public static string command = "";
        
        /// <summary>
        /// 控制下载线程
        /// </summary>
        public static bool Started = false;
        
        /// <summary>
        /// m3u8文件列表最大下载数
        /// </summary>
        public static int M3u8ListDownMax = 1;

        /// <summary>
        /// 单个m3u8文件片段最大下载数
        /// </summary>
        public static int M3u8FileDownMax = 3;

        /// <summary>
        /// 单个片段最大线程数
        /// </summary>
        public static int FileThreadMax = 1;

        [DllImport("kernel32.dll")]
        private static extern uint GetTickCount();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms">延时毫秒数</param>
        /// <param name="rangeMax">最大延时随机数</param>
        public static void Delay(uint ms)
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <summary>
        /// 获取机器启动毫秒时间
        /// </summary>
        /// <returns>机器启动时间，单位：毫秒</returns>
        public static uint GetTickTime()
        {
            return GetTickCount();
        }

        public static void DeletePath(string _path, bool _allSub = false)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_path);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        if (_allSub == true)
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                        }
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 获得目录下所有指定类型文件总数
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extName">扩展名可以多个 例如 .mp3.wma.rm</param>
        /// <returns>int</returns>
        public static int GetFileCount(string path, string extName)
        {
            if (path == "") return 0;
            int count = 0;
            try
            {
                string[] dir = Directory.GetDirectories(path); //文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件   
                    {
                        if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                        {
                            count += 1;
                        }
                    }
                }
                return count;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long GetFilesize(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Length;
        }

        /// <summary>
        /// 获取一个URL路径的最后部分
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static string GetLastPath(string _path)
        {
            int _index = _path.LastIndexOf("/");
            if (_index == -1) return _path;
            else return _path.Substring(_index, _path.Length - _index);
        }

        /// <summary>
        /// 删除文件后缀后多余的 ?内容
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string TrimFilename(string filename)
        {
            return Regex.Replace(filename, @"(.*)(\?.*)", "$1");
        }

        public static string GetSizeInfo(long Len)
        {
            if (Len <= 0) return "未知";
            float temp = Len;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (temp >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                temp = temp / 1024;
            }
            return String.Format("{0:0.##} {1}", temp, sizes[order]);
        }

        /// <summary>
        /// 获取文件名重复后的数字格式化的文件名
        /// </summary>
        /// <param name="_filename"></param>
        /// <returns></returns>
        public static string GetNumberFormatName(string _filename)
        {
            if(File.Exists(_filename))
            {
                string _newFilename = "";
                FileInfo fi = new FileInfo(_filename);
                string _filenameNoExt = Path.GetFileNameWithoutExtension(_filename);
                for (int i = 1; i < 999999; i++)
                {
                    _newFilename = String.Format(fi.DirectoryName + "\\" + _filenameNoExt + @" ({0:G})" + fi.Extension, i.ToString());
                    //Console.WriteLine(_newFilename);
                    if (File.Exists(_newFilename)) continue;
                    else break;
                }
                return _newFilename;
            }
            else
            {
                return _filename;
            }
        }

        /// <summary>
        /// 返回ffmpeg可执行的命令
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static string GetCommandText(string src, string dest)
        {
            return Core.command.Replace("{{m3u8filename}}", src).Replace("{{mp4filename}}", dest);
        }

        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Gb2312ToUtf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Utf8ToGb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }
    }

}
