using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M3U8_GETTER
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                string filePath = "";
                for (int i = 0; i < args.Length; i++)
                {
                    filePath += " " + args[i];
                }
                Form1.argFilename = filePath.Trim();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
