using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{
    class Ini
    {
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filepath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="size"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static String ReadIni(string section, string key, string def, int size, string filepath)
        {
            StringBuilder sb = new StringBuilder(4096);
            GetPrivateProfileString(section, key, def, sb, size, filepath);
            return sb.ToString();
        }
        /// <summary>
        /// 写入数据到配置文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static long WriteIni(string section, string key, string val, string filepath)
        {
            return WritePrivateProfileString(section, key, val, filepath);
        }
        // 删除ini文件下所有段落及键值
        public void ClearAllSection(string filepath)
        {
            WriteIni(null, null, null, filepath);
        }
        //删除ini文件下Section段落下的所有键
        public void ClearSection(string Section, string filepath)
        {
            WriteIni(Section, null, null, filepath);
        }
    }
}
