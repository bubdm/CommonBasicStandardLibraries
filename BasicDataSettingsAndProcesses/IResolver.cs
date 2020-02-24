using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{


    //we may decide to do more than just resolve (?)
    //on the other hand, this is very basic so maybe resolve is the best thing to do here.
    //the good news is we already have the old 2.0 standard version that will keep working forever.

    //for now, get rid of this interface.  we may do something else later.
    //because we don't know what will be added
    //this could be a good candidate for autoimplemented interfaces
    //so if a person does not implement, then just won't do anything for that part.

    public interface IResolver //if i decide that i need to have the possibilty for data, will do so.
    {
        T Resolve<T>();
        T Resolve<T>(object tag);
        object GetInstance(Type type); //needs this as well.
        void RegisterInstanceType(Type type); //i need this too.
        /// <summary>
        /// the purpose of this would be so something can tell if its not registered and can act accordingly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool RegistrationExist<T>();
    }

    //we have to keep the iresolver because the custom lists rely on it for the random functions.
    //they 

    //decided to do something different for now.
    //we could bring it back.


    //public interface IAdvancedResolve : IResolver
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="Tag">This is extra info so it can more easily return the proper implementation  its an object so can represent anything</param>
    //    /// <returns></returns>
    //    T Resolve<T>(object Tag); //sometimes a person has a tag that will be used to resolve. was going to be string but decided to make it object.
    //    /// <summary>
    //    /// This is used in cases where the object was replaced.
    //    /// 
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="NewObject"></param>
    //    void ReplaceObject<T>(T NewObject);
    //}


}