using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class HttpExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient client, string uri, T value)
        {
            string thisStr = await js.SerializeObjectAsync(value);
            StringContent content = new StringContent(thisStr, Encoding.UTF8, "application/json");
            return await client.PostAsync(uri, content);
        }
        public static async Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient client, string uri, T value)
        {
            string thisStr = await js.SerializeObjectAsync(value);
            StringContent content = new StringContent(thisStr, Encoding.UTF8, "application/json");
            return await client.PutAsync(uri, content);
        }
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string uri) //looks like delete is no problem.  not sure what patch is about anyways.
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode == false)
                throw new BasicBlankException("Failed to get async json data.  Rethink");
            string res = await response.Content.ReadAsStringAsync();
            try
            {
                return await js.DeserializeObjectAsync<T>(res);
            }
            catch (Exception)
            {
                throw new BasicBlankException("Failed to get the json.  Most likely, the page returned an error or the link was wrong");
            }
        }
    }
}