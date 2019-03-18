using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    //looks like the spacing problem is still happening sometimes.
    public static class NewtonJsonStrings
    {
        public static async Task<string> SerializeObjectAsync(object ThisObj) //decided to do async versions now. hopefully i don't regret this.
        {
            string ThisStr = default;
            JsonSettingsGlobals.PopulateSettings();
            await Task.Run(() => ThisStr = JsonConvert.SerializeObject(ThisObj, JsonSettingsGlobals.JsonSettingsData));
            return ThisStr;
        }

        public static async Task<T> DeserializeObjectAsync<T>(string ThisStr)
        {
            T ThisT = default;
            JsonSettingsGlobals.PopulateSettings();
            await Task.Run(() => ThisT = JsonConvert.DeserializeObject<T>(ThisStr, JsonSettingsGlobals.JsonSettingsData));
            return ThisT;
        }

		public static async Task<T> ConvertObjectAsync <T>(object ThisObj)
		{
			string ThisStr = await SerializeObjectAsync(ThisObj);
			return await  DeserializeObjectAsync<T>(ThisStr);
		}

    }
}
