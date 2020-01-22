namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    /// <summary>
    /// Denotes an instance which maintains an active item.
    /// </summary>
    public interface IHaveActiveViewModel : IScreen
    {
        /// <summary>
        /// The currently active item.
        /// </summary>
        IScreen? ActiveViewModel { get; set; }
    }
}