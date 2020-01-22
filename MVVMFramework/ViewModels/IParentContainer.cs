using CommonBasicStandardLibraries.MVVMFramework.Conductors;

namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    public interface IParentContainer
    {
        IContentControl? ParentContainer { get; set; } //can be null.
    }
}