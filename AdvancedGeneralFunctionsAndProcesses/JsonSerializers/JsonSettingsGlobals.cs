using Newtonsoft.Json;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    public static class JsonSettingsGlobals
    {
        static internal JsonSerializerSettings _jsonSettingsData = new JsonSerializerSettings();
        static public PreserveReferencesHandling PreserveReferencesHandling { get; set; } = PreserveReferencesHandling.Objects; //looks like i can preserve references for objects but not arrays  hopefully that works.
        static public TypeNameHandling TypeNameHandling { get; set; } = TypeNameHandling.All; //defaults to it but can change if necessary.
        static internal void PopulateSettings()
        {
            _jsonSettingsData.PreserveReferencesHandling = PreserveReferencesHandling;
            _jsonSettingsData.TypeNameHandling = TypeNameHandling;
            _jsonSettingsData.Formatting = Formatting.Indented;
        }

    }
}
