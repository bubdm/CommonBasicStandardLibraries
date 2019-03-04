using System;
using System.Collections.Generic;
using System.Text;
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
        Task<CustomBasicList<string>> GetProjectLists();
        Task<string> GetDataPath(); //this is where you will store all the files needed for the system to work.
        Task<string> GetKey(); //you need to provide the key which is used to upload packages.

    }
}
