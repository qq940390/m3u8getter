using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3U8_GETTER.classes
{
    public class DownloadItemInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Id;

        /// <summary>
        /// Url
        /// </summary>
        public string Url;

        /// <summary>
        /// 短文件名，包含后缀
        /// </summary>
        public string ShortName;

        /// <summary>
        /// 文件大小信息
        /// </summary>
        public string FileSizeInfo;

        /// <summary>
        /// 已下载大小信息
        /// </summary>
        public string DownloadSizeInfo;

        /// <summary>
        /// 进度信息
        /// </summary>
        public string ProgressInfo;

        /// <summary>
        /// 已用时间，单位 秒
        /// </summary>
        public string UsedTime;

        /// <summary>
        /// 文件下载状态
        /// </summary>
        public string Status;

        /// <summary>
        /// 下载线程状态
        /// </summary>
        public ItemInfoState State = ItemInfoState.Unstarted;
    }
}
