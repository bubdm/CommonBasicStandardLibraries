using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.ContainerClasses;
using CommonBasicStandardLibraries.Exceptions;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public static class RandomSetHelpers //this could be needed since random is used so much.  this also gets your container as well.
    {
        public static void SetRandom(ref IResolver privateContainer, ref RandomGenerator rs)
        {
            if (rs != null)
            {
                return;
            }
            if (privateContainer == null)
            {
                if (cons == null && AutoUseMainContainer)
                {
                    cons = new ContainerMain();
                }

                privateContainer = cons!;
            }
            if (privateContainer != null)
            {
                rs = privateContainer.Resolve<RandomGenerator>();
            }
            else
            {
                throw new BasicBlankException("Unable to get random.  May have to create private random.  Not sure.  Rethink");
            }
        }
    }
}
