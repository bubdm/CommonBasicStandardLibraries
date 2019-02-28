using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class CustomStopWatchCP
    {
        private Stopwatch ThisStop;
        // try the built in way.  that worked.

        public int MaxTime { get; set; }
        private bool IsStarted;
        public event TimeUpEventHandler TimeUp;

        public delegate void TimeUpEventHandler();

        public event ProgressEventHandler Progress;

        public delegate void ProgressEventHandler(long TimeLeft); //has to be long now.

        private IProgress<EnumCategory> ThisProgress;

        public enum EnumCategory
        {
            Progress = 1,
            TimeUp = 2
        }

        public CustomStopWatchCP()
        {
            ThisStop = new Stopwatch();
            ThisProgress = new Progress<EnumCategory>((EnumCategory Category) =>
            {
                // i think this means its done.
                if (Category == EnumCategory.TimeUp)
                {
                    TimeUp?.Invoke(); // otherwise, threading problem
                    return;
                }
                if (Category == EnumCategory.Progress)
                {
                    // well see how long left
                    Progress?.Invoke(MaxTime - ThisStop.ElapsedMilliseconds);
                    return;
                }
                throw new Exception("No category found");
            });
        }

        public void StartTimer()
        {
            IsStarted = true;
            ThisStop.Restart(); // i think
            LoopTimer();
        }

        private void LoopTimer()
        {
            Task.Run(() =>
            {
                do
                {
                    System.Threading.Thread.Sleep(50);
                    if (ThisStop.IsRunning == false)
                        return;
                    if (ThisStop.ElapsedMilliseconds > MaxTime)
                    {
                        ThisStop.Stop();
                        ThisProgress.Report(EnumCategory.TimeUp);
                        return;
                    }

                    ThisProgress.Report(EnumCategory.Progress);
                }
                while (true)// because times up.
    ;
            });
        }

        public void PauseTimer()
        {
            if (IsStarted == false)
                throw new Exception("Cannot pause the timer because it was never started");
            ThisStop.Stop(); // hopefully this simple.
        }

        public void ContinueTimer()
        {
            if (IsStarted == false)
                throw new Exception("The timer was never started");
            ThisStop.Start(); // i think this is time.
            LoopTimer(); // keep looping again now.
        }

        public void ManualStop(bool ShowEvent)
        {
            StopTimer(ShowEvent);
        }

        public void ManualStop()
        {
            StopTimer();
        }

        private void StopTimer()
        {
            StopTimer(true);
        }

        public long TimeTaken()
        {
            return ThisStop.ElapsedMilliseconds; // try to use long instead.
        }

        private void StopTimer(bool ShowEvent)
        {
            ThisStop.Stop();
            if (ShowEvent == true)
                ThisProgress.Report(EnumCategory.TimeUp);
        }
    }
}
