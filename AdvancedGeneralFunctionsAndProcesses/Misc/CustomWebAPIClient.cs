using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.ConfigProcesses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Net.Http;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public abstract class CustomWebAPIClient
    {
        //this is intended to have more simple processes for creating web api clients.
        /// <summary>
        /// this is for the path of the base part of the service.  examples include
        /// hotelservice/api/
        /// </summary>
        protected abstract string ServicePath { get; }
        protected Uri? BaseAddress;
        /// <summary>
        /// This is the key to get out to figure out the base url.
        /// </summary>
        protected abstract string Key { get; }
        protected HttpClient Client;
        public CustomWebAPIClient(ISimpleConfig sims, HttpClient client)
        {
            //return; //to experiment.  somehow not working currently.
            Client = client;
            SetUp(sims);
        }
        private async void SetUp(ISimpleConfig sims)
        {
            string firstPart = await sims.GetStringAsync(Key);
            Uri secondPart = new Uri(firstPart);
            BaseAddress = new Uri(secondPart, ServicePath);
        }
        protected async Task SaveResults(string extras, string errorMessage)
        {
            Uri finalAddress = new Uri(BaseAddress, extras);
            var results = await Client.GetAsync(finalAddress);
            if (results.IsSuccessStatusCode == false)
                throw new BasicBlankException(errorMessage);
        }
        protected async Task<T> GetResults<T>(string extras, string errorMessage)
        {
            Uri finalAddress = new Uri(BaseAddress, extras);
            return await Client.GetJsonAsync<T>(finalAddress, errorMessage);
        }
    }
}
