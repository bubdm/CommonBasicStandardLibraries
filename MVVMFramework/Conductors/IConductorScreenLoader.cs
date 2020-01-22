using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    /// <summary>
    /// this uses the beginnings of conductors and allows for loading screens.
    /// realized that all conductors unless you need beginnings needs to be able to load screens.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConductorScreenLoader<T>
    {
        Task LoadScreenAsync(T viewModel); //i like the idea that i activate after i call the method.
    }
}
