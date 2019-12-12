using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using System.Collections.Generic;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
using CommonBasicStandardLibraries.ContainerClasses;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public static class RandomSetHelpers //this could be needed since random is used so much.  this also gets your container as well.
    {
        public static void SetRandom(ref IResolver privateContainer, ref RandomGenerator rs)
        {
            if (rs != null)
                return;
            if (privateContainer == null)
                privateContainer = cons!;
            if (privateContainer != null)
                rs = privateContainer.Resolve<RandomGenerator>();
            else
                throw new BasicBlankException("Unable to get random.  May have to create private random.  Not sure.  Rethink");
        }
    }
}
