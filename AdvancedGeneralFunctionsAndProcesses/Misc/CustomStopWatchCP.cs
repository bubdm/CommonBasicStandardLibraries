using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class CustomStopWatchCP
    {
        private readonly Stopwatch _thisStop;
        // try the built in way.  that worked.
        public int MaxTime { get; set; }
        private bool _isStarted;
        public event TimeUpEventHandler? TimeUp;
        public delegate void TimeUpEventHandler();
        public event ProgressEventHandler? Progress;
        public delegate void ProgressEventHandler(long TimeLeft); //has to be long now.
        private readonly IProgress<EnumCategory> _thisProgress;
        public enum EnumCategory
        {
            Progress = 1,
            TimeUp = 2
        }
        public bool IsRunning => _thisStop.IsRunning;
        public CustomStopWatchCP()
        {
            _thisStop = new Stopwatch();
            _thisProgress = new Progress<EnumCategory>((EnumCategory Category) =>
            {
                if (Category == EnumCategory.TimeUp)
                {
                    TimeUp?.Invoke(); // otherwise, threading problem
                    return;
                }
                if (Category == EnumCategory.Progress)
                {
                    Progress?.Invoke(MaxTime - _thisStop.ElapsedMilliseconds);
                    return;
                }
                throw new Exception("No category found");
            });
        }
        public void StartTimer()
        {
            _isStarted = true;
            _thisStop.Restart(); // i think
            LoopTimer();
        }
        private void LoopTimer()
        {
            Task.Run(() =>
            {
                do
                {
                    System.Threading.Thread.Sleep(50);
                    if (_thisStop.IsRunning == false)
                        return;
                    if (_thisStop.ElapsedMilliseconds > MaxTime)
                    {
                        _thisStop.Stop();
                        _thisProgress.Report(EnumCategory.TimeUp);
                        return;
                    }

                    _thisProgress.Report(EnumCategory.Progress);
                }
                while (true);// because times up.
            });
        }
        public void PauseTimer()
        {
            if (_isStarted == false)
                throw new Exception("Cannot pause the timer because it was never started");
            _thisStop.Stop(); // hopefully this simple.
        }
        public void ContinueTimer()
        {
            if (_isStarted == false)
                throw new Exception("The timer was never started");
            _thisStop.Start(); // i think this is time.
            LoopTimer(); // keep looping again now.
        }
        public void ManualStop(bool showEvent)
        {
            StopTimer(showEvent);
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
            return _thisStop.ElapsedMilliseconds; // try to use long instead.
        }
        private void StopTimer(bool showEvent)
        {
            _thisStop.Stop();
            if (showEvent == true)
                _thisProgress.Report(EnumCategory.TimeUp);
        }
    }
}