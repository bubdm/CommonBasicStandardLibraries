using aa = CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.ApplicationPath;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions
{
    public static class FileLocator
    {
        public static string MainLocation { get; set; } = "";
        public static string GetLocation(string name)
        {
            string path = aa.GetApplicationPath();
            int finds = path.IndexOf(MainLocation);
            string modified = path.Substring(0, finds);
            modified += MainLocation += @"\";
            modified += name;
            return modified;
        }
    }
}