using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
using js =  SimpleJson.JsonConvert;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers
{
    public static class FileHelpers
    {
        //public static void SaveObject(string Path, object ThisObject)
        //{
        //    var ThisText = JsonConvert.SerializeObject(ThisObject);
        //    WriteText(Path, ThisText, false);
        //}

        public static async Task SaveObjectAsync(string Path, object ThisObject)
        {
            string ThisText=default;
            await Task.Run(() => ThisText = js.SerializeObject(ThisObject));
            await WriteTextAsync(Path, ThisText, false);
            
        }

        //public static T RetrieveSavedObject<T>(string Path)
        //{
        //    var ThisText = AllTextFromFile(Path);
        //    return JsonConvert.DeserializeObject<T>(ThisText);
        //}

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


    }
}
