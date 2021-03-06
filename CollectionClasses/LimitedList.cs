using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace CommonBasicStandardLibraries.CollectionClasses
{

    /// <summary>
    /// this is a list to be used to store a list that is limited.  For example, the default size is 5 so 5 items get stored..
    /// will show the newest one added first.
    /// but if there are too many, then deletes the oldest one.
    /// </summary>
    public class LimitedList<T> : IEnumerable<T>
    {
        private readonly T[] _values;
        private int _upTo;
        public static int MaximumAllowed = 5; //if you don't specify before doing list, too bad.

        public LimitedList()
        {
            if (MaximumAllowed <= 0)
            {
                throw new BasicBlankException("You cannot allow 0 items.  Rethink");
            }
            _values = new T[MaximumAllowed];

        }

        public void PopulateSavedList<R>(LimitedList<R> oldList)
        {
            int x = 0;
            foreach (var item in oldList)
            {
                _values[x] = (T) item;
                _upTo++;
                x++;
            }
        }

        public LimitedList(IEnumerable<T> list)
        {
            if (MaximumAllowed <= 0)
            {
                throw new BasicBlankException("You cannot allow 0 items.  Rethink");
            }
            _values = new T[MaximumAllowed];
            int r = 0;
            foreach (var item in list)
            {
                r++;
                if (r == MaximumAllowed)
                {
                    break;
                }
                //i think that if less are allowed later, then will get trimmed automatically.
            }
            r--;
            foreach (var item in list)
            {
                if (_upTo > MaximumAllowed - 1)
                {
                    return; //just return period.
                }
                _values[r] = item;
                r--;
                _upTo++;
            }
        }

        public void Add(T item)
        {
            if (item == null)
            {
                throw new BasicBlankException("You cannot add nothing.  Rethink");
            }
            if (_upTo <= MaximumAllowed - 1)
            {
                _values[_upTo] = item;
                _upTo++;
                return;
            }
            for (int i = 0; i < MaximumAllowed - 1; i++)
            {
                _values[i] = _values[i + 1];

            }
            _values[MaximumAllowed - 1] = item;
        }
        public T this[int index]
        {
            get
            {
                if (index > MaximumAllowed)
                {
                    throw new BasicBlankException($"Only up to {MaximumAllowed} are supported");
                }
                int upto = 0;
                int checkstart = _upTo - 1;
                if (_upTo == 0)
                {
                    return default!; //because there is none.
                }
                int maxs = MaximumAllowed;
                do
                {
                    if (upto == index || checkstart == 0)
                    {
                        return _values[upto];
                    }
                    upto++;
                    //if (upto == index || checkstart == 0)
                    //{
                    //    
                    //}
                    //checkstart--;
                    maxs--;
                    //upto++;
                    if (maxs == 0)
                    {
                        throw new BasicBlankException("I think its wrong.  Rethink");
                    }

                } while (true);
            }

        }
        public T MostRecent => this[0];

        public T OldestItem => this[MaximumAllowed - 1];

        

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal struct Enumerator : IEnumerator<T>
        {
            public Enumerator(LimitedList<T> list)
            {
                _l = list;
                _currentElement = default;
                _index = -1;
            }



            [AllowNull] private T _currentElement;

            private int _index;
            private readonly LimitedList<T> _l;

            public T Current
            {
                get
                {
                    //if (_index < 0)
                    //    ThrowEnumerationNotStartedOrEnded();
                    return _currentElement;
                }
            }
            object? IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_index > MaximumAllowed - 2)
                {
                    Reset();
                    return false;
                }
                if (_index >= _l._upTo - 1)
                {
                    Reset();
                    return false;
                }
                _index++;
                int item = _l._upTo - _index - 1;
                _currentElement = _l._values[item];
                return true;
            }

            public void Reset()
            {
                _index = -1;
                _currentElement = default;
            }

            public void Dispose()
            {
                Reset();
            }
        }


    }
}