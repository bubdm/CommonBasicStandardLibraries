using Newtonsoft.Json;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    public static class JsonSettingsGlobals
    {
        static internal JsonSerializerSettings JsonSettingsData = new JsonSerializerSettings();
        static public PreserveReferencesHandling PreserveReferencesHandling { get; set; } = PreserveReferencesHandling.Objects; //looks like i can preserve references for objects but not arrays  hopefully that works.
        static public TypeNameHandling TypeNameHandling { get; set; } = TypeNameHandling.All; //defaults to it but can change if necessary.
        static internal void PopulateSettings()
        {
            JsonSettingsData.PreserveReferencesHandling = PreserveReferencesHandling;
            JsonSettingsData.TypeNameHandling = TypeNameHandling;
            JsonSettingsData.Formatting = Formatting.Indented;
        }

    }
}
