using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomSetHelpers;
namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class BlankListBehavior<T> : IListModifiers<T>
    {
        public void Add(T value) { }
        public void AddRange(IEnumerable<T> thisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add)
        {
            //i am violating some princples.  but if i don't this time, its going to be too complex.
        }
        public void Clear() { }
        public void LoadStartLists(IEnumerable<T> thisList) { }
        public bool RemoveSpecificItem(T value)
        {
            return true;
        }
        public void ReplaceAllWithGivenItem(T value) { }
        public void ReplaceItem(T oldItem, T newItem) { }
        public void ReplaceRange(IEnumerable<T> thisList) { }
    }
    public class CustomBasicList<T> : ICustomBasicList<T>
    {

        protected List<T> PrivateList; //was going to use ilist but we need features that only applies to the ilist.
        //if i chose to not use the List but instead use the array and more of my own thing, then watch this series.
        //https://www.youtube.com/watch?v=3L6Wv7AxjjI&list=PLRwVmtr-pp07QlmssL4igw1rnrttJXerL&index=20&t=0s
        //this showed indexers but has other videos in the series.
        //the beginnings of the custom list is
        //https://www.youtube.com/watch?v=u4yaUd4hWzQ&list=PLRwVmtr-pp07QlmssL4igw1rnrttJXerL&index=12
        protected IListModifiers<T>? Behavior;

        public CustomBasicList(int initCapacity = 5)
        {
            PrivateList = new List<T>(initCapacity); LoadBehavior(); FactoryRequested = new SimpleCollectionFactory<T>
            {
                SendingType = GetType()
            };
        } //will go ahead and always use the simple one unless another one is used

        protected bool IsStart = true;
        public IListFactory<T> FactoryRequested { get; set; }
        protected void CopyFrom(IEnumerable<T> collection) //done
        {
            IList<T> items = PrivateList;
            if (collection != null && items != null)
            {
                using IEnumerator<T> enumerator = collection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    items.Add(enumerator.Current);
                }
            }
        }
        public int Capacity { get => PrivateList.Capacity; set => PrivateList.Capacity = value; } //now have a new function.
        public void TrimExcess()
        {
            PrivateList.TrimExcess();
        }
        protected virtual void LoadBehavior()
        {
            Behavior = new BlankListBehavior<T>(); //so inherited version can load a different behavior.
        }
        public CustomBasicList(IEnumerable<T> TempList)
        {
            if (TempList == null)
                throw new ArgumentNullException("List");
            FactoryRequested = new SimpleCollectionFactory<T>
            {
                SendingType = GetType()
            };
            PrivateList = new List<T>(TempList.Count()); //telling them we know what to start with if sending a new list.
            CopyFrom(TempList);
            LoadBehavior();
            Behavior!.LoadStartLists(TempList);
        }

        public T this[int index] //not sure how changing this would affect things.
        {
            get { return PrivateList[index]; }
            set
            {

                if (index < 0 || index >= PrivateList.Count)
                {
                    throw new CustomArgumentException("Index", "When setting custom collection, out of range");
                }
                PrivateList[index] = value;

            }
        }
        public int Count => PrivateList.Count; //done
        internal RandomGenerator? _rs;
        private IResolver? _privateContainer; //hopefully still okay.
        public IResolver MainContainer { set => _privateContainer = value; }
        //there is a warning that insert is an expensive operation.
        public void Add(T value, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add) //done i think
        {
            int index = PrivateList.Count;
            if (notificationmode == NotifyCollectionChangedAction.Add)
                InsertItem(index, value);
            else if (notificationmode == NotifyCollectionChangedAction.Reset)
                InsertOnly(index, value);
            else
                throw new BasicBlankException("Only supported scenarios are add and reset for adding");
        }
        public void Add(T value)
        {
            Add(value, NotifyCollectionChangedAction.Add);
        }
        //because i really don't have to have 2 different interfaces.  that causes problems too.
        public void AddRange(IEnumerable<T> thisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add) //done
        {
            InsertRange(PrivateList.Count, thisList);
        }
        public void Clear()
        {
            CheckReentrancy();
            PrivateList.Clear();
            Behavior!.Clear();
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        public bool Contains(T item) //this uses the proxy because i am controlling access to the list instead of accessing it directly. done
        {
            return PrivateList.Contains(item);
        }
        public bool Exists(Predicate<T> match) //done
        {
            return PrivateList.Exists(match);
        }
        public T Find(Predicate<T> match) //done
        {
            return PrivateList.Find(match);
        }
        public ICustomBasicList<T> FindAll(Predicate<T> match) //done
        {
            var thisList = FactoryRequested.GetStartList();
            foreach (T thisItem in PrivateList)
            {
                if (match(thisItem) == true)
                    thisList.Add(thisItem);
            }
            return thisList; //surprisingly this worked out well.
        }
        public T FindOnlyOne(Predicate<T> match)
        {
            ICustomBasicList<T> thisList = FindAll(match);
            if (thisList.Count > 1)
                throw new BasicBlankException("Found more than one item using FindOnlyOne");
            if (thisList.Count == 0)
                throw new BasicBlankException("Did not find any items using FindOnlyOne");
            return thisList.Single();
        }
        public int FindFirstIndex(Predicate<T> match) //done
        {
            return PrivateList.FindIndex(match);
        }
        public int FindFirstIndex(int startIndex, Predicate<T> match) //done
        {
            return PrivateList.FindIndex(startIndex, match);
        }
        public int FindFirstIndex(int startIndex, int count, Predicate<T> match) //done
        {
            return PrivateList.FindIndex(startIndex, count, match);
        }
        public T FindLast(Predicate<T> match) //done
        {
            return PrivateList.FindLast(match);
        }
        public int FindLastIndex(Predicate<T> match) //done
        {
            return PrivateList.FindLastIndex(match);
        }
        public int FindLastIndex(int startIndex, Predicate<T> match) //done
        {
            return PrivateList.FindLastIndex(startIndex, match);
        }
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) //done
        {
            return PrivateList.FindLastIndex(startIndex, count, match);
        }
        public int LastIndexOf(T thisItem)
        {
            return PrivateList.LastIndexOf(thisItem);
        }
        public void ForEach(Action<T> action) //this is easy because i hook into the list  (done)
        {
            PrivateList.ForEach(action);
        }
        public bool ForSpecificItem(Predicate<T> match, Action<T> action, int howManyToCheck = 0)
        {
            int privateCheck;
            if (howManyToCheck == 0)
                privateCheck = PrivateList.Count;
            else
                privateCheck = howManyToCheck;

            int privateCount = 0;
            foreach (T thisItem in PrivateList)
            {
                privateCount++;
                if (privateCount > privateCheck)
                    break;
                if (match.Invoke(thisItem) == true)
                {
                    action.Invoke(thisItem);
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match1">This is the criteria for doing all</param>
        /// <param name="allAction">This will be performed for all matching criteria</param>
        /// <param name="match2">This is the second criteria</param>
        /// <param name="specificAction">This is the specific action.  The first match it finds, performs the actions and stops</param>
        public void ComplexAction(Predicate<T> match1, Action<T> allAction, Predicate<T> match2, Action<T> specificAction)
        {
            foreach (T thisItem in PrivateList)
            {
                if (match1.Invoke(thisItem) == true)
                    //Console.WriteLine($"Trying To Invoke For Item {ThisItem.ToString()}");
                    allAction.Invoke(thisItem);
            }
            foreach (T thisItem in PrivateList)
            {
                if (match1.Invoke(thisItem) == true && match2.Invoke(thisItem) == true)
                {
                    specificAction.Invoke(thisItem);
                    return; //at this point, can stop because it already found the second match and did the second action.
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match">Condition that must be matched in order to do something</param>
        /// <param name="action">Action to perform on the conditional items</param>
        public void ForConditionalItems(Predicate<T> match, Action<T> action)
        {
            foreach (T thisItem in PrivateList)
            {
                if (match.Invoke(thisItem) == true)
                    action.Invoke(thisItem);
            }
        }
        public async Task ForConditionalItemsAsync(Predicate<T> match, BasicDataFunctions.ActionAsync<T> action)
        {
            foreach (T thisItem in PrivateList)
            {
                if (match.Invoke(thisItem) == true)
                    await action.Invoke(thisItem);
            }
        }
        public async Task ForEachAsync(BasicDataFunctions.ActionAsync<T> action) //i think done.
        {
            foreach (T thisItem in PrivateList)
            {
                await action.Invoke(thisItem);
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return PrivateList.GetEnumerator();
        }
        public T GetRandomItem() //okay.
        {
            return GetRandomItem(false);
        }
        public T GetRandomItem(bool removePrevious)
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            int ask1;
            ask1 = _rs.GetRandomNumber(PrivateList.Count);
            T thisItem = PrivateList[ask1 - 1];
            if (removePrevious == true)
                RemoveItem(ask1 - 1); //because 0 based.  i think this works
            return thisItem;
        }
        public int GetSeed()
        {
            return _rs!.GetSeed();
        }
        public ICustomBasicList<T> GetRandomList(bool removePrevious, int howManyInList) //done
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            CustomBasicList<int> rList = _rs.GenerateRandomList(PrivateList.Count, howManyInList);
            var thisList = FactoryRequested.GetStartList();
            foreach (int index in rList)
            {
                thisList.Add(PrivateList[index - 1]); //because 0 based
            }
            if (removePrevious == false)
                return thisList;
            RemoveGivenList(thisList, NotifyCollectionChangedAction.Remove);
            return thisList;
        }
        public void RemoveRandomItems(int howMany)
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            CustomBasicList<int> rList = _rs.GenerateRandomList(PrivateList.Count, howMany);
            List<T> thisList = new List<T>();
            foreach (int index in rList)
            {
                thisList.Add(PrivateList[index - 1]);
            }
            RemoveGivenList(thisList, NotifyCollectionChangedAction.Remove);
        }
        public ICustomBasicList<T> GetConditionalItems(Predicate<T> match)
        {
            ICustomBasicList<T> thisList = FactoryRequested.GetStartList();
            foreach (T thisItem in PrivateList)
            {
                if (match.Invoke(thisItem) == true)
                    thisList.Add(thisItem);
            }
            return thisList;
        }
        public ICustomBasicList<T> GetRandomList()
        {
            return GetRandomList(false, PrivateList.Count);
        }
        public ICustomBasicList<T> GetRandomList(bool removePrevious)
        {
            return GetRandomList(removePrevious, PrivateList.Count);
        }
        public ICustomBasicList<T> GetRange(int index, int count)
        {
            var thisList = FactoryRequested.GetStartList(); //if i am forced to have repeating code for performance, i don't expect this to change.
            for (int i = index; i < count; i++)
            {
                thisList.Add(PrivateList[i]);
            }
            return thisList;
        }
        public int IndexOf(T value)
        {
            return PrivateList.IndexOf(value);
        }
        public int IndexOf(T value, int index)
        {
            return PrivateList.IndexOf(value, index);
        }
        public int IndexOf(T value, int index, int count)
        {
            return PrivateList.IndexOf(value, index, count);
        }
        public void InsertBeginning(T value)
        {
            InsertItem(0, value);
        }
        public void InsertMiddle(int index, T value)
        {
            InsertItem(index, value);
        }
        public ICustomBasicList<T> RemoveAllAndObtain(Predicate<T> match)
        {
            var thisList = FactoryRequested.GetStartList();
            foreach (T item in PrivateList)
            {
                if (match(item) == true)
                    thisList.Add(item);
            }
            if (thisList.Count > 0)
                RemoveGivenList(thisList, NotifyCollectionChangedAction.Remove);
            return thisList;
        }
        public void RemoveAllOnly(Predicate<T> match)
        {
            List<T> tempList = new List<T>();
            foreach (T item in PrivateList)
            {
                if (match(item) == true)
                    tempList.Add(item);
            }
            if (tempList.Count > 0)
                RemoveGivenList(tempList, NotifyCollectionChangedAction.Remove);
        }
        public void KeepConditionalItems(Predicate<T> match)
        {
            List<T> tempList = new List<T>();
            foreach (T item in PrivateList)
            {
                if (match(item) == false)
                    tempList.Add(item);
            }
            if (tempList.Count > 0)
                RemoveGivenList(tempList, NotifyCollectionChangedAction.Remove);
        }
        public void RemoveAt(int index)
        {
            RemoveItem(index);
        }
        public void RemoveFirstItem()
        {
            RemoveItem(0);
        }
        public void RemoveGivenList(IEnumerable<T> thisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Reset)
        {
            if (notificationmode != NotifyCollectionChangedAction.Remove && notificationmode != NotifyCollectionChangedAction.Reset)
                throw new CustomArgumentException("Mode must be either remove or reset for removegivenlist", nameof(notificationmode));
            if (thisList == null)
                throw new ArgumentNullException(nameof(thisList));
            CheckReentrancy();
            if (notificationmode == NotifyCollectionChangedAction.Reset)
            {
                foreach (T thisItem in thisList)
                {
                    PrivateList.Remove(thisItem);
                    Behavior!.RemoveSpecificItem(thisItem);
                }
                OnCollectionChanged(notificationmode);
                return;
            }
            List<T> changedList = new List<T>();
            bool rets;
            foreach (T thisItem in thisList)
            {
                rets = PrivateList.Remove(thisItem);
                if (rets == true)
                {
                    Behavior!.RemoveSpecificItem(thisItem);
                    changedList.Add(thisItem);
                }
            }
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(notificationmode, changedList, -1);
        }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e) { } //won't do anything.  but the inherited version will.
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index) { }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index, int oldIndex) { }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object oldItem, object newItem, int index) { }
        protected virtual void PropertyCountChanged() { }
        protected virtual void PropertyItemChanged() { }
        protected virtual void CheckReentrancy() { }
        public void RemoveLastItem()
        {
            RemoveItem(PrivateList.Count - 1);
        }
        public void RemoveRange(int index, int count)
        {
            List<T> newList = PrivateList.GetRange(index, count);
            RemoveGivenList(newList);
        }
        public bool RemoveSpecificItem(T value)
        {
            int index = PrivateList.IndexOf(value);
            if (index == -1)
                return false; //because not even there.
            RemoveItem(index);
            return true;
        }
        public void ReplaceAllWithGivenItem(T value) //hopefully no problem here (?)  i think done.
        {
            PrivateList.Clear();
            Behavior!.Clear();
            Add(value, NotifyCollectionChangedAction.Reset);
        }
        public void ReplaceItem(T oldItem, T newItem) //i think done
        {
            CheckReentrancy();
            int index = PrivateList.IndexOf(oldItem);
            PrivateRemoveItem(index, oldItem);
            PrivateInsertItem(index, newItem);  //hopefully this works.
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem!, newItem!, index); //hopefully does not cause a breaking change but has to take a risk.
        }
        public void Reverse() //i think
        {
            CheckReentrancy(); //i think this is needed too.
            PrivateList.Reverse();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, PrivateList, 0, 0); //obviously if reversing, then the entire thing changes.
        }
        public void ShuffleList()
        {
            if (Count == 0)
                return; //because there is nothing to shuffle.  so can't obviously.  better than runtime error.
            SetRandom(ref _privateContainer!, ref _rs!);
            CustomBasicList<int> thisList = _rs.GenerateRandomList(PrivateList.Count); //i think
            List<T> rList = new List<T>(); //since they removed and added, then i think its best if i just remove the entire thing.   however, let them know it really moved.
            CheckReentrancy();
            foreach (int index in thisList)
            {
                rList.Add(PrivateList[index - 1]); //because 0 based.
            }
            PrivateList.Clear();
            PrivateList.AddRange(rList);
            PropertyItemChanged(); //not sure but i think so
            OnCollectionChanged(NotifyCollectionChangedAction.Move, PrivateList, 0, 0);
        }
        public void ShuffleList(int howMany)
        {
            CheckReentrancy();
            SetRandom(ref _privateContainer!, ref _rs!);
            CustomBasicList<int> thisList = _rs.GenerateRandomList(PrivateList.Count, howMany);
            List<T> rList = new List<T>();
            foreach (int index in thisList)
            {
                rList.Add(PrivateList[index - 1]); //because 0 based.
            }
            PrivateList.Clear();
            InsertRange(0, rList, NotifyCollectionChangedAction.Reset); //i think this time, it will reset it.
        }
        public void Sort()
        {
            CheckReentrancy();
            PrivateList.Sort();
            FinalSort();
        }
        private void FinalSort()
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Move, PrivateList, 0, 0);
        }
        public void Sort(Comparison<T> comparison)
        {
            CheckReentrancy();
            PrivateList.Sort(comparison);
            FinalSort();
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            CheckReentrancy();
            PrivateList.Sort(index, count, comparer);
            FinalSort();
        }
        //for icomparer, 1 means greater than.  -1 means less than.  0 means equal.
        public void Sort(IComparer<T> comparer)
        {
            CheckReentrancy();
            PrivateList.Sort(comparer);
            FinalSort();
        }
        public bool TrueForAll(Predicate<T> match)
        {
            return PrivateList.TrueForAll(match);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return PrivateList.GetEnumerator();
        }
        public void LoadStartLists(IEnumerable<T> thisList)
        {
            CopyFrom(thisList);
        }
        protected void SetItem(int index, T item)//done now.
        {
            CheckReentrancy();
            PrivateList[index] = item;
            T oldItem = PrivateList[index];
            Behavior!.ReplaceItem(oldItem, item);
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem!, item!, index);
        }
        protected void InsertItem(int index, T item) //done now.
        {
            CheckReentrancy();
            PrivateInsertItem(index, item);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item!, index);
        }
        private void InsertOnly(int index, T item)
        {
            CheckReentrancy();
            PrivateInsertItem(index, item);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, null!, -1);
        }
        private void PrivateInsertItem(int index, T item) //done i think
        {
            PrivateList.Insert(index, item);
            Behavior!.Add(item);
        }
        private void PrivateRemoveItem(int index, T oldItem) //this means if i want to do one at a time, that is an option.  however, no notifications.  this is because there are others and decided to do this way
        {
            PrivateList.RemoveAt(index);
            Behavior!.RemoveSpecificItem(oldItem);
        }
        protected void RemoveItem(int index) //done again.
        {
            CheckReentrancy();
            T oldItem = PrivateList[index];
            PrivateRemoveItem(index, oldItem);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, oldItem!, index);
        }
        protected void MoveItem(int oldIndex, int newIndex) //done again
        {
            CheckReentrancy();
            T removedItem = this[oldIndex];
            PrivateList.RemoveAt(oldIndex);
            PrivateList.Insert(newIndex, removedItem);
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem!, newIndex, oldIndex);
        }
        private void PrivateInsertRange(int index, IEnumerable<T> items) //just in case i have to do it without notifications.
        {
            PrivateList.InsertRange(index, items);
            Behavior!.AddRange(items);
        }
        public void InsertRange(int index, IEnumerable<T> items, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add) //done i think.
        {
            if (notificationmode != NotifyCollectionChangedAction.Add && notificationmode != NotifyCollectionChangedAction.Reset)
                throw new CustomArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationmode));
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            CheckReentrancy();
            PrivateInsertRange(index, items);
            PropertyCountChanged();
            PropertyItemChanged();
            if (notificationmode == NotifyCollectionChangedAction.Reset)
            {
                if (index != 0)
                    throw new BasicBlankException("You cannot send reset for inserting range because the value is greater than 0.  This means there was something there");
                OnCollectionChanged(notificationmode);
                return;
            }
            List<T> added = items.ToList(); //unfortunately, can't use the custom list this time.
            OnCollectionChanged(notificationmode, added, -1);
        }
        public int HowMany(Predicate<T> match)
        {
            int y = 0;
            PrivateList.ForEach(items =>
            {
                if (match.Invoke(items) == true)
                    y++;
            }
            );
            return y;
        }
        public void ReplaceRange(IEnumerable<T> thisList)
        {
            if (thisList == null)
                throw new CustomArgumentException("Collection Cannot Be Nothing When Replacing Range");
            PrivateList.Clear();
            CheckReentrancy();
            PrivateList.AddRange(thisList); //i think
            Behavior!.ReplaceRange(thisList);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }
        public void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action) //does not have to be there.  if not there, ignore
        {
            if (Exists(match) == false)
                return; //because there is none.
            T thisItem;
            try
            {
                thisItem = FindOnlyOne(match); //so if more than one is found, then will raise an exception
            }
            catch (BasicBlankException)
            {
                throw new BasicBlankException("RemoveOnlyOneAction Had An Error.  Most Likely, The Condition Had More Than Element Satisfying It");
            }
            catch (Exception ex)
            {
                throw new BasicBlankException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
            }
            action.Invoke(thisItem);
            RemoveSpecificItem(thisItem);
        }
        public void RemoveSeveralConditionalItems(CustomBasicList<ConditionActionPair<T>> thisList)
        {
            CustomBasicList<T> rList = new CustomBasicList<T>();
            thisList.ForEach(firstItem =>
            {
                if (Exists(firstItem.Predicate) == true)
                {
                    T thisItem;
                    try
                    {
                        thisItem = FindOnlyOne(firstItem.Predicate);
                    }
                    catch (BasicBlankException)
                    {
                        throw new BasicBlankException("RemoveSeveralConditionalItems Had An Error.  Most Likely, The Condition Had More Than Element Satisfying One Of The Condition Lists");
                    }
                    catch (Exception ex)
                    {
                        throw new BasicBlankException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
                    }
                    firstItem.Action.Invoke(thisItem, firstItem.Value);
                    rList.Add(thisItem);
                }
            });
            RemoveGivenList(rList, NotifyCollectionChangedAction.Remove);
        }
        public ICustomBasicList<U> ConvertAll<U>(Converter<T, U> converter)
        {
            IListFactory<U> temps = FactoryRequested.GetNewFactory<U>();
            ICustomBasicList<U> news = temps.GetStartList();
            news.Capacity = PrivateList.Count; //use their count
            //in this case, you can access directly.
            //however, if you are doing an interface, then does not work so well.
            CustomBasicList<U>? fasts = news as CustomBasicList<U>;
            bool rets = false;
            if (fasts != null)
                rets = true;
            for (int i = 0; i < PrivateList.Count; i++)
            {
                if (rets == true)
                    fasts!.PrivateList.Add(converter(PrivateList[i]));
                else
                    news.Add(converter(PrivateList[i]));
            }
            if (news.Count != PrivateList.Count && rets == true)
                throw new BasicBlankException("ConvertAll Does Not Reconcile");
            return news;
        }
        public void ReplaceWithNewObjects(int howMany, Func<T> func)
        {
            CustomBasicList<T> newList = new CustomBasicList<T>();
            howMany.Times(() =>
            {
                newList.Add(func.Invoke());
            });
            ReplaceRange(newList);
        }
        public void ReplaceWithNewObjects(Func<T> func)
        {
            ReplaceWithNewObjects(Count, func);
        }
        public void MoveItem(T item, int newIndex)
        {
            int OldIndex = PrivateList.IndexOf(item);
            MoveItem(OldIndex, newIndex);
        }
    }
}