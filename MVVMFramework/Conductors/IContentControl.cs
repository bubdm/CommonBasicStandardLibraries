using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using System;
using System.Collections.Generic;
using System.Text;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    public interface IContentControl
    {
        void Close();
        void Add(IUIView view); //i think i will need to force anything being added to this system to implement iuview interface.
    }
}