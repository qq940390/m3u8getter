using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            Application.ThreadException += ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Net.ServicePointManager.DefaultConnectionLimit = 5000;
            Application.Run(new Form1(args));
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }
    }
}
