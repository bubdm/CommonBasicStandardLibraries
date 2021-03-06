using Newtonsoft.Json;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    public static class NewtonJsonStrings
    {

        public static async Task<string> SerializeObjectAsync(object thisObj)
        {
            string thisStr = "";
            JsonSettingsGlobals.PopulateSettings();
            await Task.Run(() => thisStr = JsonConvert.SerializeObject(thisObj, JsonSettingsGlobals._jsonSettingsData));
            return thisStr;
        }
        public static async Task<T> DeserializeObjectAsync<T>(string thisStr)
        {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            T thisT = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            JsonSettingsGlobals.PopulateSettings();
            await Task.Run(() => thisT = JsonConvert.DeserializeObject<T>(thisStr, JsonSettingsGlobals._jsonSettingsData)!);
            return thisT!;
        }
        public static T ConvertObject<T>(object thisObj)
        {
            string thisStr = SerializeObject(thisObj);
            return DeserializeObject<T>(thisStr);
        }
        public static string SerializeObject(object thisObj)
        {
            JsonSettingsGlobals.PopulateSettings();
            return JsonConvert.SerializeObject(thisObj, JsonSettingsGlobals._jsonSettingsData);
        }

        public static T DeserializeObject<T>(string thisStr)
        {
            JsonSettingsGlobals.PopulateSettings();
            return JsonConvert.DeserializeObject<T>(thisStr, JsonSettingsGlobals._jsonSettingsData)!;
        }
        public static async Task<T> ConvertObjectAsync<T>(object thisObj)
        {
            string thisStr = await SerializeObjectAsync(thisObj);
            return await DeserializeObjectAsync<T>(thisStr);
        }
    }
}
