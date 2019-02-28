using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleJson;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    //looks like the spacing problem is still happening sometimes.
    public static class NewtonJsonStrings
    {
        public static async Task<string> SerializeObjectAsync(object ThisObj) //decided to do async versions now. hopefully i don't regret this.
        {
            string ThisStr = default;
            await Task.Run(() => ThisStr = JsonConvert.SerializeObject(ThisObj));
            return ThisStr;
        }

        public static async Task<T> DeserializeObjectAsync<T>(string ThisStr)
        {
            T ThisT = default;
            await Task.Run(() => ThisT = JsonConvert.DeserializeObject<T>(ThisStr));
            return ThisT;
        }

		public static async Task<T> ConvertObjectAsync <T>(object ThisObj)
		{
			string ThisStr = await SerializeObjectAsync(ThisObj);
			return await  DeserializeObjectAsync<T>(ThisStr);
		}

    }
}
