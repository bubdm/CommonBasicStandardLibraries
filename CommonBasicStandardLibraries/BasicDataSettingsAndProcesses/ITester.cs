using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    //i think will be forced this time to use enums
    //because i like the idea of using enms
    //since we don't know about secondary one, then not necessary
    public interface ITester<T> : IDisposable where T: Enum
    {
        void RemoveDate(T MainCategory, int? SecondaryCategory);
        DateTime GetDate(DateTime DateToCheck, T MainCategory, int? SecondaryCategory);
        void SaveDate(DateTime ThisDate, T MainCategory, int? SecondaryCategory);
    }
}