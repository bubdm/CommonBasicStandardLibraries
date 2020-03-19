using System;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{



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




}