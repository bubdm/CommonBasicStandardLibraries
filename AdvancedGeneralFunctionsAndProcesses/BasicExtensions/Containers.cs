using CommonBasicStandardLibraries.ContainerClasses;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Containers
    {
        public static void RegisterNonWindows(this ContainerMain container)
        {
            container.RegisterInstance<ISimpleUI, NonWindowClass>();
            container.RegisterInstance<IFocusOnFirst, NonWindowClass>();
            //if there is anything else, i have it.
        }
    }
}