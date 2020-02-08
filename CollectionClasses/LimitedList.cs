using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;
namespace CommonBasicStandardLibraries.CollectionClasses
{
    //decided to be a list of objects.
    //because its intended to be used in lots of places and don't want to force the called to be generic as well.
    //may have to just do objects after all in order to deserialize properly.

    /// <summary>
    /// this is a list to be used to store a list of objects but only a limited number of them.
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

        public void Add(T item)
        {
            if (item == null)
            {
                throw new BasicBlankException("You cannot add nothing.  Rethink");
            }
            if (_upTo <= MaximumAllowed -1)
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

        public object? this[int index]
        {
            get
            {
                //needs to return a specific item.
                //if (index == 0)
                //{
                //    throw new ArgumentOutOfRangeException("If the most recent one was needed, should have used MostRecent method");
                //}

                if (index > MaximumAllowed - 1)
                {
                    throw new BasicBlankException($"Only up to {MaximumAllowed} are supported");
                }
                int upto = 0;
                int checkstart = _upTo - 1;
                if (_upTo == 0)
                {
                    return default; //because there is none.
                }
                int maxs = 5;
                do
                {
                    if (upto == index || checkstart == 0)
                    {
                        return _values[checkstart];
                    }
                    checkstart--;
                    maxs--;
                    upto++;
                    if (maxs == 0)
                    {
                        throw new BasicBlankException("I think its wrong.  Rethink");
                    }

                } while (true);
            }

        }
        public object? MostRecent => this[0];

        public object? OldestItem => this[MaximumAllowed - 1];

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
                    //throw new BasicBlankException("Only 5 are supported for list");
                }
                _values[r] = item;
                r--;
                _upTo++;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //hopefully does not have to be public.

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
                if (_index >= _l._upTo)
                {
                    Reset();
                    return false;
                }
                _index++;
                int item = _l._upTo - _index - 1;
                _currentElement = _l._values[item];
                return true;
            }
            //since its not generics, then no dispose is needed this time.

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
