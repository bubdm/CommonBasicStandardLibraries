using CommonBasicStandardLibraries.CollectionClasses;

namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    public interface IViewModelBase : IClearable
    {
        string this[string PropertyName] { get; }

        bool AttemptedToSubmitForm { get; set; }
        string DisplayName { get; set; }
        CustomBasicCollection<string> ErrorLists { get; }
        bool IsValid { get; }
    }
}