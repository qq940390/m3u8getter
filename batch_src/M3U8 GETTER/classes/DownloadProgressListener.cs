using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8_GETTER.classes
{
    public class DownloadProgressListener : IDownloadProgressListener
    {
        public ListenerMessage Msg = null;

        public DownloadProgressListener(ListenerMessage _msg)
        {
            this.Msg = _msg;
        }

        public delegate void DlgSendMsg(ListenerMessage _msg);
        public DlgSendMsg DoSendMsg = null;

        public void NotifyDownloadedSize(long _downLength, uint _usedTime, int _timeoutNums)
        {
            if (Msg == null)
            {
                Msg = new ListenerMessage();
            }

            //下载用时，单位 秒
            Msg.UsedTimeSecond = Convert.ToString(Math.Round(((double)_usedTime / 1000)));

            Msg.TimeoutNums = _timeoutNums;

            //下载速度
            if (Msg.DownLength == 0)
            {
                Msg.Speed = _downLength;
            }
            else
            {
                Msg.Speed = (float)(_downLength - Msg.DownLength);
                
            }
            if (Msg.Speed == 0)
            {
                Msg.Surplus = -1;
                Msg.SurplusInfo = "未知";
            }
            else
            {
                Msg.ContentLengthInfo = Msg.GetSizeInfo(Msg.ContentLength);
                if (!Msg.IsGziped)
                {
                    Msg.Surplus = ((Msg.ContentLength - Msg.DownLength) / Msg.Speed);
                }
            }
            Msg.DownLength = _downLength;
           
            if (_downLength >= Msg.ContentLength)
            {
                //下载完成
                Msg.Status = ListenerStatus.End;
                Msg.SpeedInfo = "0 K";
                Msg.SurplusInfo = "已完成";
            }
            else
            {
                //下载中
                Msg.Status = ListenerStatus.DownLoad;                
            }            
            
            if (DoSendMsg != null) DoSendMsg(Msg);//通知具体调用者下载进度
        }
    }

    public enum ListenerStatus
    {
        Start,
        GetLength,
        DownLoad,
        End,
        Error
    }

    public class ListenerMessage
    {
        private long _ContentLength = 0;
        private long _DownLength = 0;
        private float _Speed = 0;
        private float _Surplus = 0;

        
        /// <summary>
        /// 是否是gzip压缩的
        /// </summary>
        public bool IsGziped { get; set; }

        public string UsedTimeSecond { get; set; }

        public int TimeoutNums { get; set; }

        public long ContentLength
        {
            get
            {
                return _ContentLength;
            }

            set
            {
                _ContentLength = value;
                ContentLengthInfo = GetSizeInfo(value);
            }
        }

        public int M3u8ItemId { get; set; }

        public int Id { get; set; }

        public string Url { get; set; }

        public string Filename { get; set; }

        public string Fullpath { get; set; }

        public ListenerStatus Status { get; set; }

        public long DownLength
        {
            get
            {
                return _DownLength;
            }

            set
            {
                _DownLength = value;
                DownLengthInfo = GetSizeInfo(value);
                if (ContentLength >= value)
                {
                    Progress = Math.Round((double)value / ContentLength * 100, 2);
                }
                else
                {
                    Progress = 0;
                }
            }
        }

        public float Speed
        {
            get
            {
                return _Speed;
            }

            set
            {
                _Speed = value;
                SpeedInfo = GetSizeInfo(Convert.ToUInt32(value));
            }
        }
        public string SpeedInfo { get; set; }

        public float Surplus
        {
            get
            {
                return _Surplus;
            }

            set
            {
                _Surplus = value;
                if (value > 0)
                {
                    SurplusInfo = GetDateName((int)Math.Round(value, 0));
                }
                
            }
        }

        public string ErrMessage { get; set; }

        public string DownLengthInfo { get; set; }

        public string ContentLengthInfo { get; set; }

        public double Progress { get; set; }

        public string SurplusInfo { get; set; }

        public string GetSizeInfo(long Len)
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

        private string GetDateName(int Second)
        {
            float temp = Second;
            string suf = "秒";
            if (Second>60)
            {
                suf = "分钟";
                temp = temp / 60;
                if (Second > 60)
                {
                    suf = "小时";
                    temp = temp / 60;
                    if (Second > 24)
                    {
                        suf = "天";
                        temp = temp / 24;
                        if (Second > 30)
                        {
                            suf = "月";
                            temp = temp / 30;
                            if (Second > 12)
                            {
                                suf = "年";
                                temp = temp / 12;
                            }
                        }
                      
                    }
                   
                }
                
            }
            
            return String.Format("{0:0} {1}", temp, suf);
        }
    }
}