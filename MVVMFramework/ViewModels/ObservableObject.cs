using CommonBasicStandardLibraries.DatabaseHelpers.Attributes;
using CommonBasicStandardLibraries.MVVMFramework.Commands;
using CommonBasicStandardLibraries.MVVMFramework.EventArgClasses;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    public abstract class ObservableObject : INotifyPropertyChangedEx, INotifyCanExecuteChanged, IObservableObject, IMappable
    {
        [NotMapped] //so if any are used with dapper and sql, then will know this is never mapped.
        [JsonIgnore]
        public virtual bool IsNotifying { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged; //i propose even here not overridable.  if i am wrong, rethink
        public event CanExecuteChangedEventHandler? CanExecuteChanged; //i don't think should be overridable.

        public ObservableObject()
        {
            IsNotifying = true;
        }


        /// <summary>

        /// Sets the property.

        /// </summary>

        /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>

        /// <param name="backingStore">Backing store.</param>

        /// <param name="value">Value.</param>

        /// <param name="validateValue">Validates value.</param>

        /// <param name="propertyName">Property name.</param>

        /// <param name="onChanged">On changed.</param>

        /// <typeparam name="T">The 1st type parameter.</typeparam>

        protected virtual bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action? onChanged = null,
            Func<T, T, bool>? validateValue = null)

        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))

                return false;
            if (validateValue != null && !validateValue(backingStore, value))

                return false;
            onChanged?.Invoke();
            backingStore = value;
            //OnPropertyChanged(propertyName);
            NotifyOfPropertyChange(propertyName ?? string.Empty);
            ExtraStepInChangingProperty(propertyName);
            if (IsNotifying == false)
            {
                return false; //because not notifying so it won't repaint again.
            }
            return true;
        }
        protected virtual void ExtraStepInChangingProperty([CallerMemberName()] string? propertyName = null) { }


        protected void OnPropertyChanged(string property)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }


        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event directly.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name = "propertyName">Name of the property.</param>
        public virtual void NotifyOfPropertyChange([CallerMemberName] string? propertyName = null)
        {
            if (IsNotifying && PropertyChanged != null)
                OnUIThread(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
        }




        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <typeparam name = "TProperty">The type of the property.</typeparam>
        /// <param name = "property">The property expression.</param>
        public void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
        }


        public void NotifyOfCanExecuteChange([CallerMemberName] string? predicate = "")
        {
            //only if isnotifying.
            if (IsNotifying && CanExecuteChanged != null)
                OnUIThread(() => CanExecuteChanged.Invoke(this, new CanExecuteChangedEventArgs(predicate!)));
        }




        /// <summary>
        /// Executes the given action on the UI thread
        /// </summary>
        /// <remarks>An extension point for subclasses to customise how property change notifications are handled.</remarks>
        /// <param name="action"></param>
        protected virtual void OnUIThread(Action action) => action.OnUIThread();

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public virtual void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }
    }
}
