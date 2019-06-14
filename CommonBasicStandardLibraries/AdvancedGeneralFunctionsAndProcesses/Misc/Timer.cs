using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class Timer
    {
        private readonly System.ComponentModel.BackgroundWorker _backs;
        public Timer()
        {
            _backs = new System.ComponentModel.BackgroundWorker();
            _backs.WorkerSupportsCancellation = true;
            _backs.DoWork += Backs_DoWork;
        }
        private bool Processing;

        private bool IsPaused;
        private int Elapsed;
        private bool DidStart;
        private EnumStartStatus StartStatus;

        public void StartTimer()
        {
            if (DidStart == true)
                throw new Exception("The timer has already started.  Must stop the timer first");
            Elapsed = 0;
            if (Processing == true)
            {
                StartStatus = EnumStartStatus.Over;
                return;
            }
            _backs.RunWorkerAsync();
        }

        public void PauseTimer()
        {
            if (IsPaused == true)
                return;
            IsPaused = true;
            _backs.CancelAsync();
        }

        public void ContinueTimer()
        {
            IsPaused = false;
            StartStatus = EnumStartStatus.FromPause;
        }

        public void StopTimer()
        {
            DidStart = false;
            if (Processing == true)
                _backs.CancelAsync();
        }

        public float TimeElapsed => Elapsed / 1000;
        

        private void Backs_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Processing = true;
            do
            {
                if (e.Cancel == true)
                {
                    Processing = false;
                    return;
                }

                System.Threading.Thread.Sleep(10);
                Elapsed += 10;
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
            if (StartStatus == EnumStartStatus.None)
                return;
            if (StartStatus == EnumStartStatus.Over)
                Elapsed = 0;
            _backs.RunWorkerAsync();
        }



    }
}
