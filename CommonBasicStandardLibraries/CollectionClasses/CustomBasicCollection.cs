using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class CustomBasicCollection<T> : CustomBasicList<T>, INotifyCollectionChanged, INotifyPropertyChanged //i am guessing that it can still use the basic factory because when you get items from it, no observable needed anyways
    {
        public CustomBasicCollection() : base() { }
        
        public CustomBasicCollection(IEnumerable<T> ThisList) : base(ThisList) { }

        //public event NotifyCollectionChangedEventHandler CollectionChanged;


        //------------------------------------------------------
        #region INotifyPropertyChanged implementation

        protected virtual event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }
        #endregion INotifyPropertyChanged implementation


        /// <summary>
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }



        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) //this is done
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. a event handler
        /// of the CollectionChanged event is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        private readonly SimpleMonitor _monitor = new SimpleMonitor();



        protected override void CheckReentrancy() //done i think
        {
            if (_monitor.Busy)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException("Reentracy Not Allowed.  This means threading problem.  Rethink");
                //throw new InvalidOperationException(SR.GetString(SR.ObservableCollectionReentrancyNotAllowed));
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedAction e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(e)); //i think
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index)
        {
            if (index > -1)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(e, item, index));
            else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(e, changedItems: (System.Collections.IList) item, -1));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(e, item, index, oldIndex));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedAction e, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(e, index, newItem, index));
        }

        protected override void PropertyCountChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        protected override void PropertyItemChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }


        private class SimpleMonitor : IDisposable
        {
            public void Enter()
            {
                ++_busyCount;
            }

            public void Dispose()
            {
                --_busyCount;
            }

            public bool Busy { get { return _busyCount > 0; } }

            int _busyCount;
        }
    }
}