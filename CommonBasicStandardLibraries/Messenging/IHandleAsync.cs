using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.Messenging
{
    public interface IHandleAsync<TMessage>
    {
        //string Tag { get; }
        Task HandleAsync(TMessage Message);
    }
}
