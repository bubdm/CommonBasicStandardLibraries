using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{

    public interface IConductorSingleBegins<T> : IConductorScreenLoader<T>, IHaveActiveViewModel, IConductorGuarantee
    {
        //because the collection does not close child since there can be several of them.
        Task CloseChildAsync();
    }
}
