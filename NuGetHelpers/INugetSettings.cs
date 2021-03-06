using CommonBasicStandardLibraries.CollectionClasses; //i really want people to use my new custom class for the lists
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    public interface INugetSettings
    {
        /// <summary>
        /// Need to provide a list of the paths for the directories you are using with this system.  will be a function
        /// </summary>
        /// <returns></returns>
        Task<CustomBasicList<string>> GetProjectListsAsync();
        Task<string> GetDataPathAsync(); //this is where you will store all the files needed for the system to work.
        Task<string> GetKeyAsync(); //you need to provide the key which is used to upload packages.
        //decided to make it clear its async.  the good thing about this interface is if i decided to get the keys via keyvault, should still work.
    }
}