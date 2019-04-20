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

    public interface IAdvancedResolve : IResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Tag">This is extra info so it can more easily return the proper implementation  its an object so can represent anything</param>
        /// <returns></returns>
        T Resolve<T>(object Tag); //sometimes a person has a tag that will be used to resolve. was going to be string but decided to make it object.
        /// <summary>
        /// This is used in cases where the object was replaced.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NewObject"></param>
        void ReplaceObject<T>(T NewObject);
    }

    public interface ISQLServer
    {
        string GetConnectionString(string DatabaseOrPath); //this way if implemented, then you can easily get the sql server connection. 
    }

   
	

}