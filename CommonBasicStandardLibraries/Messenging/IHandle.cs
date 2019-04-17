using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Messenging
{
    public interface IHandle<TMessage>
    {
        //string Tag { get; }
        //string Tag(TMessage Message);


        //this means no tag here.


        void Handle(TMessage Message);
    }
}
