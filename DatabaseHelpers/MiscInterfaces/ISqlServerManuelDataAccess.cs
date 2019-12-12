using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.DatabaseHelpers.MiscInterfaces
{
    /// <summary>
    /// This has methods used for cases where i access data manually and not doing automapping
    /// This is designed for sql server.
    /// The good news a standard library can invoke with no sql server dependency.  something else has to worry about that part.
    /// needs both async and regular methods.
    /// i did like the idea of returning list.  if you need one item, then just do .first().
    /// </summary>
    public interface ISqlServerManuelDataAccess
    {
        CustomBasicList<T> LoadData<T, U>(string sqlStatement,
                               U parameters,
                               string connectionStringName,
                               bool isStoredProcedure = false);
        Task<CustomBasicList<T>> LoadDataAsync<T, U>(string sqlStatement,
                               U parameters,
                               string connectionStringName,
                               bool isStoredProcedure = false);
        void SaveData<T>(string sqlStatement,
                               T parameters,
                               string connectionStringName,
                               bool isStoredProcedure = false);
        Task SaveDataAsync<T>(string sqlStatement,
                               T parameters,
                               string connectionStringName,
                               bool isStoredProcedure = false);
    }
}