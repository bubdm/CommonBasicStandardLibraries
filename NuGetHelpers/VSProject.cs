using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    //this has to be public so it can serialize/deserialize
    public enum EnumStatus
    {
        None,New,NeedCreate,NeedsToUpload
    }

    public enum EnumNet
    {
        None, CoreStandard, Traditional
    }
    public class VSProject
    {
        public EnumStatus Status { get; set; }
        public string ProjectDirectory { get; set; } = "";
        public DateTime? LastModified { get; set; }
        public string LastVersion { get; set; } = "";
        public string CSPath { get; set; } = "";
        public string DLLPath { get; set; } = "";
        public string NugetPath { get; set; } = "";
        public EnumNet NetVersion { get; set; }
        
    }
}
