using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public interface IResolver //if i decide that i need to have the possibilty for data, will do so.
    {
        T Resolve<T>();
    }

    public interface ISQLServer
    {
        string GetConnectionString(string DatabaseOrPath); //this way if implemented, then you can easily get the sql server connection. 
    }

   
	

}