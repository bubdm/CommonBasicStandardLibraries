using CommonBasicStandardLibraries.Exceptions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// use the custom json if using custom lists.  make sure the server supports sending text and the controllers deserializes what is necessary
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostCustomJsonAsync(this HttpClient client, string uri, object value) //needs to be async.
        {
            string thisStr = JsonConvert.SerializeObject(value); //can't use await this time for the custom method to send to server.
            StringContent content = new StringContent(thisStr);
            return await client.PostAsync(uri, content);
        }
        public static async Task SaveDownloadFileAsync(this HttpClient client, string requesturi, string path) //so if it fails, you will know
        {
            //i like download file better.  this is when it does the standard way.  if using custom, then i can still do custom way.
            HttpResponseMessage output = await client.GetAsync(requesturi);
            if (output.IsSuccessStatusCode == false)
                throw new BasicBlankException($"Was not okay.  The code returned was {output.StatusCode}");
            using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            var results = await output.Content.ReadAsByteArrayAsync();
            await stream.WriteAsync(results, 0, results.Length);
        }
        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient client, string uri, T value)
        {
            string thisStr = await js.SerializeObjectAsync(value!); //take a risk here.
            StringContent content = new StringContent(thisStr, Encoding.UTF8, "application/json");
            return await client.PostAsync(uri, content);
        }
        public static async Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient client, string uri, T value)
        {
            string thisStr = await js.SerializeObjectAsync(value!);
            StringContent content = new StringContent(thisStr, Encoding.UTF8, "application/json");
            return await client.PutAsync(uri, content);
        }
        /// <summary>
        /// use the custom method if this contains a custom list and using put command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutCustomJsonAsync<T>(this HttpClient client, string uri, T value)
        {
            string thisStr = await js.SerializeObjectAsync(value!);
            StringContent content = new StringContent(thisStr);
            return await client.PutAsync(uri, content);
        }
        
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string uri) //looks like delete is no problem.  not sure what patch is about anyways.
        {
            
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode == false)
                throw new BasicBlankException("Failed to get async json data.  Rethink");
            string res = await response.Content.ReadAsStringAsync();
            response.Dispose();
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