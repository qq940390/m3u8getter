using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{

    public class M3u8ItemInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Id;

        /// <summary>
        /// 短文件名，包含后缀
        /// </summary>
        public string ShortName;

        /// <summary>
        /// 视频目录
        /// </summary>
        public string VideoPath;

        /// <summary>
        /// 临时文件夹中经过md5 hash 的子文件夹
        /// </summary>
        public string Md5Path = "";

        /// <summary>
        /// Url
        /// </summary>
        public string Url;

        /// <summary>
        /// 下载状态
        /// </summary>
        public string Status;

        /// <summary>
        /// 视频状态
        /// </summary>
        public string VideoStatus;

        /// <summary>
        /// 线程状态
        /// </summary>
        public ItemInfoState State = ItemInfoState.Unstarted;

        /// <summary>
        /// 处理视频对应的ffmpeg进程ID
        /// </summary>
        public int FfmpegProcessId = 0;

        /// <summary>
        /// m3u8文件解类析实例
        /// </summary>
        public M3U8Loader Loader = new M3U8Loader();

        /// <summary>
        /// 批量下载类实例
        /// </summary>
        public DownloadFile DownloadInstance = null;
    }
}
