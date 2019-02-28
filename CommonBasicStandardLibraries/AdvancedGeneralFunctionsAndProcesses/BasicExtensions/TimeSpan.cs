using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class TimeSpanExtensions //decided to go ahead and rename.  because i can forsee a problem with it being ambigious.  since c# are all classes now.
    {
        public static string SongProgress(this System.TimeSpan ProgressSpan, System.TimeSpan DurationSpan)
        {
            // videos is over an hour.  if i do the videos in the new format, needs to be redone
            //looks like i have to do it this way in c#.  this means if special extensions are needed for timespans, then this is needed

            if (DurationSpan.Hours > 0)
                return string.Format("{0:00}:{1:00}:{2:00}/{3:00}:{4:00}:{5:00}", ProgressSpan.Hours, ProgressSpan.Minutes, ProgressSpan.Seconds, DurationSpan.Hours, DurationSpan.Minutes, DurationSpan.Seconds);
            else
                return string.Format("{0:00}:{1:00}/{2:00}:{3:00}", ProgressSpan.Minutes, ProgressSpan.Seconds, DurationSpan.Minutes, DurationSpan.Seconds);
        }
    }
}
