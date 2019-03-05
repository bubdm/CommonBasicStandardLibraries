using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using js = Newtonsoft.Json.JsonConvert;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    public static class FileHelpers
    {
        public static async Task SaveObjectAsync(string Path, object ThisObject)
        {
            string ThisText=default;
            await Task.Run(() => ThisText = js.SerializeObject(ThisObject, Newtonsoft.Json.Formatting.Indented));
            await WriteTextAsync(Path, ThisText, false);
        }

        public static async Task<T> RetrieveSavedObjectAsync<T>(string Path)
        {
            T ThisT;
            ThisT = default;
            string ThisText;
            ThisText = await AllTextAsync(Path);


            await Task.Run(() =>
            {
                ThisT = js.DeserializeObject<T>(ThisText);
            });
            return ThisT;
        }

        //decided to make it all async.  hopefully its a good decision.  could end up being forced to have a non async version.  well see what happens.
        //decided to go ahead and use the newton one.  since it will download it anyways.
        //plus there are other packages that require it so i might as well use it.

    }
}
