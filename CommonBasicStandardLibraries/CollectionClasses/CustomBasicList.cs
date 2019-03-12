using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.Exceptions;
using  rs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomGenerator;
namespace CommonBasicStandardLibraries.CollectionClasses
{

    public class BlankListBehavior<T> : IListModifiers<T>
    {
        public void Add(T value)
        {
            //just do nothing
        }

        

        public void AddRange(IEnumerable<T> ThisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add)
        {
            //i am violating some princples.  but if i don't this time, its going to be too complex.
        }

        public void Clear()
        {
            
        }

        public void LoadStartLists(IEnumerable<T> ThisList)
        {
            
        }


        public bool RemoveSpecificItem(T value)
        {
            return true;
        }

        public void ReplaceAllWithGivenItem(T value)
        {
            
        }

        public void ReplaceItem(T OldItem, T NewItem)
        {
            
        }

		public void ReplaceRange(IEnumerable<T> ThisList)
		{
			
		}
	}

    public class CustomBasicList<T> : ICustomBasicList<T>
    {

        protected List<T> PrivateList; //was going to use ilist but we need features that only applies to the ilist.

        protected IListModifiers<T> Behavior;

        public CustomBasicList() { PrivateList = new List<T>(); LoadBehavior(); FactoryRequested = new SimpleCollectionFactory<T>
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
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        items.Add(enumerator.Current);
                    }
                }
            }
        }

        protected virtual void LoadBehavior() //done
        {
            Behavior = new BlankListBehavior<T>(); //so inherited version can load a different behavior.
        }

        public CustomBasicList(IEnumerable<T> TempList) //done
        {
            if (TempList == null)
                throw new ArgumentNullException("List");
            FactoryRequested = new SimpleCollectionFactory<T>
            {
                SendingType = GetType()
            };
			PrivateList = new List<T>(); //try this.
            CopyFrom(TempList);
            LoadBehavior();
            Behavior.LoadStartLists(TempList);
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

        public void Add(T value) //done i think
        {
            int Index = PrivateList.Count;
            InsertItem(Index, value);
        }

        //because i really don't have to have 2 different interfaces.  that causes problems too.
        public void AddRange(IEnumerable<T> ThisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add) //done
        {
            InsertRange(PrivateList.Count, ThisList);            
        }

        public void Clear() //i think this is done.
        {
            CheckReentrancy();
            PrivateList.Clear();
            Behavior.Clear();
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
            var ThisList = FactoryRequested.GetStartList();
            foreach(T ThisItem in PrivateList)
            {
                if (match(ThisItem) == true)
                    ThisList.Add(ThisItem);
            }
            return ThisList; //surprisingly this worked out well.
        }

		public T FindOnlyOne(Predicate<T> match)
		{
			ICustomBasicList<T> ThisList = FindAll(match);
			if (ThisList.Count > 1)
				throw new BasicBlankException("Found more than one item using FindOnlyOne");
			if (ThisList.Count == 0)
				throw new BasicBlankException("Did not find any items using FindOnlyOne");
			return ThisList.Single();
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

        public void ForEach(Action<T> action) //this is easy because i hook into the list  (done)
        {
            PrivateList.ForEach(action);
        }

        public bool ForSpecificItem(Predicate<T> match, Action<T> action, int HowManyToCheck = 0)
        {
            int PrivateCheck;
            if (HowManyToCheck == 0)
                PrivateCheck = PrivateList.Count;
            else
                PrivateCheck = HowManyToCheck;

            int PrivateCount = 0;
            foreach(T ThisItem in PrivateList)
            {
                PrivateCount++;
                if (PrivateCount > PrivateCheck)
                    break;
                if (match.Invoke(ThisItem) == true)
                {
                    action.Invoke(ThisItem);
                    return true;
                }

            }
            return false;

        }

        //we may be forced to go further (well see)


        /// <summary>
        /// 
        /// </summary>
        /// <param name="match1">This is the criteria for doing all</param>
        /// <param name="AllAction">This will be performed for all matching criteria</param>
        /// <param name="match2">This is the second criteria</param>
        /// <param name="SpecificAction">This is the specific action.  The first match it finds, performs the actions and stops</param>
        public void ComplexAction(Predicate<T> match1, Action<T> AllAction, Predicate<T> match2, Action<T> SpecificAction)
        {
            foreach(T ThisItem in PrivateList)
            {
				if (match1.Invoke(ThisItem) == true)
					//Console.WriteLine($"Trying To Invoke For Item {ThisItem.ToString()}");
                    AllAction.Invoke(ThisItem);
            }
            foreach (T ThisItem in PrivateList)
            {
                if (match1.Invoke(ThisItem) == true && match2.Invoke(ThisItem) == true)
                {
                    SpecificAction.Invoke(ThisItem);
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
			foreach (T ThisItem in PrivateList)
			{
				if (match.Invoke(ThisItem) == true)
					action.Invoke(ThisItem);
			}
		}

        public async Task ForConditionalItemsAsync(Predicate<T> match, BasicDataFunctions.ActionAsync<T> action)
        {
            foreach (T ThisItem in PrivateList)
            {
                if (match.Invoke(ThisItem) == true)
                    await action.Invoke(ThisItem);
            }
        }

        public async Task ForEachAsync(BasicDataFunctions.ActionAsync<T> action) //i think done.
        {
            foreach (T ThisItem in PrivateList)
            {
                await action.Invoke(ThisItem);
            }

        }

        public IEnumerator<T> GetEnumerator() //done
        {
            return PrivateList.GetEnumerator();
        }

        public T GetRandomItem() //okay.
        {
            return GetRandomItem(false);
        }

        public T GetRandomItem(bool RemovePrevious) //done
        {
            
            int Ask1;
            Ask1 = rs.GetRandomNumber(PrivateList.Count);
            T ThisItem = PrivateList[Ask1 - 1];
            if (RemovePrevious == true)
                RemoveItem(Ask1 - 1); //because 0 based.  i think this works
            
            return ThisItem;
        }

        public ICustomBasicList<T> GetRandomList(bool RemovePrevious, int HowManyInList) //done
        {
            List<int> RList = rs.GenerateRandomList(PrivateList.Count, HowManyInList);

            var ThisList = FactoryRequested.GetStartList();

            foreach (int Index in RList)
            {
                ThisList.Add(PrivateList[Index-1]); //because 0 based
            }

            if (RemovePrevious == false)
                return ThisList;

            RemoveGivenList(ThisList, NotifyCollectionChangedAction.Remove);
            return ThisList;
        }

        public void RemoveRandomItems(int HowMany) //done.
        {
            List<int> RList = rs.GenerateRandomList(PrivateList.Count, HowMany);

            List<T> ThisList = new List<T>();
            foreach (int Index in RList)
            {
                ThisList.Add(PrivateList[Index-1]);
            }
            RemoveGivenList(ThisList, NotifyCollectionChangedAction.Remove);
        }

		public ICustomBasicList<T> GetConditionalItems(Predicate<T> match)
		{
			ICustomBasicList <T> ThisList = FactoryRequested.GetStartList();
			foreach(T ThisItem in PrivateList)
			{
				if (match.Invoke(ThisItem) == true)
					ThisList.Add(ThisItem);
			}
			return ThisList;
		}

		public ICustomBasicList<T> GetRandomList() //done
        {
            return GetRandomList(false, PrivateList.Count);
        }

        public ICustomBasicList<T> GetRandomList(bool RemovePrevious) //done
        {
            return GetRandomList(RemovePrevious, PrivateList.Count);
        }

        public ICustomBasicList<T> GetRange(int index, int count) //done
        {
            var ThisList = FactoryRequested.GetStartList(); //if i am forced to have repeating code for performance, i don't expect this to change.
            for (int i = index; i < count; i++)
            {
                ThisList.Add(PrivateList[i]);
            }
            return ThisList;
        }

        public int IndexOf(T value) //done
        {
            return PrivateList.IndexOf(value);
        }

        public int IndexOf(T value, int Index) //done
        {
            return PrivateList.IndexOf(value, Index);
        }

        public int IndexOf(T value, int Index, int Count) //done
        {
            return PrivateList.IndexOf(value, Index, Count);
        }

        public void InsertBeginning(T value) //done i think
        {
            InsertItem(0, value); //i think this simple.
        }

        public void InsertMiddle(int Index, T value) //done i think
        {
            InsertItem(Index, value);
        }

        public ICustomBasicList<T> RemoveAllAndObtain(Predicate<T> match) //done i think
        {
            var ThisList = FactoryRequested.GetStartList();
            foreach(T Item in PrivateList)
            {
                if (match(Item) == true)
                    ThisList.Add(Item);
            }           
            if (ThisList.Count > 0)
                RemoveGivenList(ThisList, NotifyCollectionChangedAction.Remove);
            return ThisList;

        }

        public void RemoveAllOnly(Predicate<T> match) //done i think
        {
            List<T> TempList = new List<T>();
            foreach(T Item in PrivateList)
            {
                if (match(Item) == true)
                    TempList.Add(Item);
            }

            if (TempList.Count > 0)
                RemoveGivenList(TempList, NotifyCollectionChangedAction.Remove);
        }

		public void KeepConditionalItems(Predicate<T> match)
		{
			List<T> TempList = new List<T>();
			foreach(T Item in PrivateList)
			{
				if (match(Item) == false)
					TempList.Add(Item);
			}
			if (TempList.Count > 0)
				RemoveGivenList(TempList, NotifyCollectionChangedAction.Remove);
		}

		public void RemoveAt(int Index) //done
        {
            RemoveItem(Index);
        }

        public void RemoveFirstItem() //done
        {
            RemoveItem(0);
        }

        public void RemoveGivenList(IEnumerable<T> ThisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Reset) //done i think
        {
            if (notificationmode != NotifyCollectionChangedAction.Remove && notificationmode != NotifyCollectionChangedAction.Reset)
                throw new CustomArgumentException("Mode must be either remove or reset for removegivenlist", nameof(notificationmode));
            if (ThisList == null)
                throw new ArgumentNullException(nameof(ThisList));

            CheckReentrancy();
            if (notificationmode == NotifyCollectionChangedAction.Reset)
            {
                foreach (T ThisItem in ThisList)
                {
                    PrivateList.Remove(ThisItem);
                    Behavior.RemoveSpecificItem(ThisItem);
                }
                OnCollectionChanged(notificationmode);
                return;
            }

            List<T> ChangedList = new List<T>();
            bool rets;
            foreach (T ThisItem in ThisList)
            {
                rets = PrivateList.Remove(ThisItem);
                if (rets == true)
                {
                    Behavior.RemoveSpecificItem(ThisItem);
                    ChangedList.Add(ThisItem);
                }
            }
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(notificationmode, ChangedList, -1);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e) //won't do anything.  but the inherited version will.
        {

        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index)
        {

        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object item, int index, int oldIndex)
        {

        }


        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction e, object oldItem, object newItem, int index)
        {

        }


        protected virtual void PropertyCountChanged()
        {

        }

        protected virtual void PropertyItemChanged()
        {

        }

        protected virtual void CheckReentrancy()
        {
            //default does nothing
        }

        public void RemoveLastItem() //done
        {
            RemoveItem(PrivateList.Count - 1);
        }        

        public void RemoveRange(int index, int count) //done i think
        {
            List<T> NewList = PrivateList.GetRange(index, count);
            RemoveGivenList(NewList);
        }

        public bool RemoveSpecificItem(T value) //done.
        {
            int Index = PrivateList.IndexOf(value);
            if (Index == -1)
                return false; //because not even there.
            RemoveItem(Index);
            return true;
        }

        public void ReplaceAllWithGivenItem(T value) //hopefully no problem here (?)  i think done.
        {
            PrivateList.Clear();
            Behavior.Clear();
            Add(value);
        }

        public void ReplaceItem(T OldItem, T NewItem) //i think done
        {
            CheckReentrancy();
            int Index = PrivateList.IndexOf(OldItem);
            PrivateRemoveItem(Index, OldItem);
            PrivateInsertItem(Index, NewItem);  //hopefully this works.
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, NewItem, NewItem, Index);
        }

        public void Reverse() //i think
        {
            CheckReentrancy(); //i think this is needed too.
            PrivateList.Reverse();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, PrivateList, 0, 0); //obviously if reversing, then the entire thing changes.
        }

        public void ShuffleList() //i can try to test out this idea.
        {
            //i think this one would be a move command.  you are not really removing the entire list
            List<int> ThisList = rs.GenerateRandomList(PrivateList.Count); //i think
            List<T> RList = new List<T>(); //since they removed and added, then i think its best if i just remove the entire thing.   however, let them know it really moved.
            CheckReentrancy();
            foreach (int Index in ThisList)
            {
                RList.Add(PrivateList[Index - 1]); //because 0 based.
            }
            PrivateList.Clear();
            PrivateList.AddRange(RList);
            PropertyItemChanged(); //not sure but i think so
            OnCollectionChanged(NotifyCollectionChangedAction.Move, PrivateList, 0, 0);
        }

        public void ShuffleList(int HowMany) //done
        {
            CheckReentrancy();
            List<int> ThisList = rs.GenerateRandomList(PrivateList.Count, HowMany);
            List<T> RList = new List<T>();
            foreach (int Index in ThisList)
            {
                RList.Add(PrivateList[Index - 1]); //because 0 based.
            }
            PrivateList.Clear();
            InsertRange(0, RList, NotifyCollectionChangedAction.Reset); //i think this time, it will reset it.
        }

        public void Sort() //done
        {
            CheckReentrancy();
            List<T> ThisList = PrivateList.ToList();
            PrivateList.Sort();
            FinalSort(ThisList);
        }

        private void FinalSort(List<T> OriginalList) //done
        {

            //i think it should only call move for things that actually moved.  not everything
            bool rets= false;
            for (var i = 0; i < PrivateList.Count; i++)
            {
                var oldIndex = OriginalList.IndexOf(PrivateList[i]);
                var newIndex = i;
                if (oldIndex != newIndex)
                {
                    OnCollectionChanged(NotifyCollectionChangedAction.Move, OriginalList[oldIndex], newIndex, oldIndex);
                }
                        
                        //collection.Move(oldIndex, newIndex);
            }
            if (rets == true)
                PropertyItemChanged(); //i think only one is needed to notify the property item.  i don't there is a need to do over and over again for this.
        }

        public void Sort(Comparison<T> comparison) //done
        {
            CheckReentrancy();
            List<T> ThisList = PrivateList.ToList();
            PrivateList.Sort(comparison);
            FinalSort(ThisList);
        }

        public void Sort(int index, int count, IComparer<T> comparer) //done
        {
            CheckReentrancy();
            List<T> ThisList = PrivateList.ToList();
            PrivateList.Sort(index, count, comparer);
            FinalSort(ThisList);
        }

        public void Sort(IComparer<T> comparer) //done
        {
            CheckReentrancy();
            List<T> ThisList = PrivateList.ToList();
            PrivateList.Sort(comparer);
            FinalSort(ThisList);
        }

        public bool TrueForAll(Predicate<T> match) //done
        {
            return PrivateList.TrueForAll(match);
        }

        IEnumerator IEnumerable.GetEnumerator() //done
        {
            return PrivateList.GetEnumerator();
        }

        public void LoadStartLists(IEnumerable<T> ThisList) //done
        {
            CopyFrom(ThisList);
        }

        protected void SetItem(int index, T item)//done now.
        {
            CheckReentrancy();
            PrivateList[index] = item;

            //has to get the old item because the old one in the dictionary is being replaced.
            T OldItem = PrivateList[index];
            Behavior.ReplaceItem(OldItem, item);
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, OldItem, item, index);
        }

        protected void InsertItem(int index, T item) //done now.
        {
            CheckReentrancy();
            PrivateInsertItem(index, item);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        private void PrivateInsertItem(int index, T item) //done i think
        {
            PrivateList.Insert(index, item);
            Behavior.Add(item);
        }

        private void PrivateRemoveItem(int index, T OldItem) //this means if i want to do one at a time, that is an option.  however, no notifications.  this is because there are others and decided to do this way
        {
            PrivateList.RemoveAt(index);
            Behavior.RemoveSpecificItem(OldItem);
        }

        protected void RemoveItem(int index) //done again.
        {
            CheckReentrancy();
            T OldItem = PrivateList[index];
            PrivateRemoveItem(index, OldItem);
            PropertyCountChanged();
            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, OldItem, index);
        }

        protected void MoveItem(int oldIndex, int newIndex) //done again
        {
            CheckReentrancy();
            T removedItem = this[oldIndex];
            PrivateList.RemoveAt(oldIndex);
            PrivateList.Insert(newIndex, removedItem);

            PropertyItemChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        private void PrivateInsertRange(int index, IEnumerable<T> items) //just in case i have to do it without notifications.
        {
            PrivateList.InsertRange(index, items);
            Behavior.AddRange(items);
        }

        public void InsertRange(int index, IEnumerable<T> items, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add) //done i think.
        {
            if (notificationmode != NotifyCollectionChangedAction.Add && notificationmode != NotifyCollectionChangedAction.Reset)
                throw new CustomArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationmode));
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            CheckReentrancy();
            int StartIndex = PrivateList.Count();
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
            OnCollectionChanged(notificationmode, items, StartIndex);
        }

		public int HowMany(Predicate<T> match)
		{
			int y = 0;
			PrivateList.ForEach(Items =>
			{
				if (match.Invoke(Items) == true)
					y++;
			}
			);
			return y;
		}

		public void ReplaceRange(IEnumerable<T> ThisList)
		{
			if (ThisList == null)
				throw new CustomArgumentException("Collection Cannot Be Nothing When Replacing Range");
			PrivateList.Clear();
			CheckReentrancy();
			PrivateList.AddRange(ThisList); //i think
			Behavior.ReplaceRange(ThisList);
			PropertyCountChanged();
			PropertyItemChanged();
			OnCollectionChanged(NotifyCollectionChangedAction.Reset);
		}

        public void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action) //does not have to be there.  if not there, ignore
        {
            if (Exists(match) == false)
                return; //because there is none.
            T ThisItem;
            try
            {
                ThisItem = FindOnlyOne(match); //so if more than one is found, then will raise an exception
            }
            catch (BasicBlankException)
            {
                throw new BasicBlankException("RemoveOnlyOneAction Had An Error.  Most Likely, The Condition Had More Than Element Satisfying It");
            }
            catch (Exception ex)
            {
                throw new BasicBlankException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
            }
            action.Invoke(ThisItem);
            RemoveSpecificItem(ThisItem); //this way it can do all other things required.
            //foreach(T ThisItem in PrivateList)
            //{
            //    if (match.Invoke(ThisItem) == true)
            //    {
            //        action.Invoke(ThisItem);

            //    }
                    
            //}
        }
        public void RemoveSeveralConditionalItems(CustomBasicList<ConditionActionPair<T>> ThisList)
        {
            //in this case, will do one remove, not several.
            CustomBasicList<T> RList = new CustomBasicList<T>();
            ThisList.ForEach(FirstItem =>
            {
                if (Exists(FirstItem.Predicate) == true)
                {
                    T ThisItem;
                    try
                    {
                        ThisItem = FindOnlyOne(FirstItem.Predicate);
                    }
                    catch (BasicBlankException)
                    {
                        throw new BasicBlankException("RemoveSeveralConditionalItems Had An Error.  Most Likely, The Condition Had More Than Element Satisfying One Of The Condition Lists");
                    }
                    catch (Exception ex)
                    {
                        throw new BasicBlankException($"Other Exception Was Thrown.  The Error Was {ex.Message}");
                    }
                    FirstItem.Action.Invoke(ThisItem, FirstItem.Value);
                    RList.Add(ThisItem);
                }
            });
            RemoveGivenList(RList, NotifyCollectionChangedAction.Remove);
        }
        
    }
}