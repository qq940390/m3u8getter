using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8_GETTER.classes
{
   public interface IDownloadProgressListener
    {
         void NotifyDownloadedSize(long _downLength, uint _usedTime, int outTimes);
    }
}
