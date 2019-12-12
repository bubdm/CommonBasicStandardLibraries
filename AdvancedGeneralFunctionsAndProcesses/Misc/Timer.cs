using System;
using System.ComponentModel;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class Timer
    {
        private readonly BackgroundWorker _backs;
        public Timer()
        {
            _backs = new BackgroundWorker();
            _backs.WorkerSupportsCancellation = true;
            _backs.DoWork += Backs_DoWork;
            _backs.RunWorkerCompleted += Backs_RunWorkerCompleted;
        }
        private bool _processing;
        private bool _isPaused;
        private int _elapsed;
        private bool _didStart;
        private EnumStartStatus _startStatus;
        public void StartTimer()
        {
            if (_didStart == true)
                throw new Exception("The timer has already started.  Must stop the timer first");
            _elapsed = 0;
            if (_processing == true)
            {
                _startStatus = EnumStartStatus.Over;
                return;
            }
            _backs.RunWorkerAsync();
        }
        public void PauseTimer()
        {
            if (_isPaused == true)
                return;
            _isPaused = true;
            _backs.CancelAsync();
        }
        public void ContinueTimer()
        {
            _isPaused = false;
            _startStatus = EnumStartStatus.FromPause;
        }
        public void StopTimer()
        {
            _didStart = false;
            if (_processing == true)
                _backs.CancelAsync();
        }
        public float TimeElapsed => (float)_elapsed / 1000;
        private void Backs_DoWork(object sender, DoWorkEventArgs e)
        {
            _processing = true;
            do
            {
                if (e.Cancel == true)
                {
                    _processing = false;
                    return;
                }
                System.Threading.Thread.Sleep(10);
                _elapsed += 10;
            }
            while (true);
        }
        private enum EnumStartStatus
        {
            None = 0,
            FromPause = 1,
            Over = 2
        }
        private void Backs_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_startStatus == EnumStartStatus.None)
                return;
            if (_startStatus == EnumStartStatus.Over)
                _elapsed = 0;
            _backs.RunWorkerAsync();
        }
    }
}