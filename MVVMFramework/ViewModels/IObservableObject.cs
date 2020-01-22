using CommonBasicStandardLibraries.MVVMFramework.EventArgClasses;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    public interface IObservableObject
    {
        bool IsNotifying { get; set; }

        event CanExecuteChangedEventHandler CanExecuteChanged;
        event PropertyChangedEventHandler PropertyChanged;

        void NotifyOfCanExecuteChange([CallerMemberName] string? predicate = "");
        void NotifyOfPropertyChange([CallerMemberName] string? propertyName = null);
        void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property);
        void Refresh();
    }
}