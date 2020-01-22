using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.CollectionClasses
{
    public interface IListFactory<T>
    {
        Type? SendingType { get; set; } //we have to risk runtime errors unless somebody knows something else.
        ICustomBasicList<T> GetStartList(); //based on the requested type, will create this new one.  will use some logic statments based on factory chosen.
        IListFactory<U> GetNewFactory<U>(); //maybe i have to figure out how to get the new factory.  doing the same thing but producing another generic type.
    }
    public interface IListModifiers<T>
    {
        //this will allow the possibilities of having at least some shared behaviors.
        //this will be used for the game classes  the base class will use blank behaviors (if not the game package).
        void Add(T value);
        void AddRange(IEnumerable<T> thisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add);
        void LoadStartLists(IEnumerable<T> thisList);
        bool RemoveSpecificItem(T value);
        void ReplaceRange(IEnumerable<T> thisList);
        void ReplaceAllWithGivenItem(T value); //this means that the item that is sent in, the list will be set to the new item.
        void ReplaceItem(T oldItem, T newItem);
        void Clear();
    }
    public interface ICountCollection
    {
        int Count { get; }
    }
    public interface ISimpleList<T> : IEnumerable<T>, ICountCollection, IMappable //i do want to be able to map lists.
    {
        //these are all things that can't add or remove from list if you are using this and not the custom list.
        Task ForEachAsync(ActionAsync<T> action);
        //its possible for there to be an await.
        void ForEach(Action<T> action);
        void ForConditionalItems(Predicate<T> match, Action<T> action);
        Task ForConditionalItemsAsync(Predicate<T> match, ActionAsync<T> action);
        bool Exists(Predicate<T> match);
        bool Contains(T item);
        T Find(Predicate<T> match); //the first one it finds, will be this one.
        T FindOnlyOne(Predicate<T> match); //this means it must find only one item.
        ICustomBasicList<T> FindAll(Predicate<T> match);
        int FindFirstIndex(Predicate<T> match);
        int FindFirstIndex(int startIndex, Predicate<T> match);
        int FindFirstIndex(int startIndex, int count, Predicate<T> match);
        T FindLast(Predicate<T> match);
        int FindLastIndex(Predicate<T> match);
        int FindLastIndex(int startIndex, Predicate<T> match);
        int FindLastIndex(int startIndex, int count, Predicate<T> match);
        int HowMany(Predicate<T> match);
        int IndexOf(T value);
        int IndexOf(T value, int Index);
        int IndexOf(T value, int Index, int Count);
        bool TrueForAll(Predicate<T> match); //i like this idea
    }
    public interface ICustomBasicList<T> : ISimpleList<T>, IListModifiers<T>
    {
        T this[int index]
        {
            get;
            set;
        }
        IListFactory<T> FactoryRequested { get; set; }
        IResolver MainContainer { set; } //private read though.
        void RemoveAt(int index);
        void RemoveGivenList(IEnumerable<T> thisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Reset); //this means if you have a list and anything on this list needs to be removed, it can be done.
        void RemoveRange(int index, int count);
        void ShuffleList(); //i think we can't go wrong with this.   whether i do lots of copy/paste, or find a routine, does not matter.
        void ShuffleList(int howMany); //this means you only keep a certain number
        T GetRandomItem();
        T GetRandomItem(bool removePrevious);
        void RemoveRandomItems(int howMany);
        ICustomBasicList<T> GetRandomList(bool removePrevious, int howManyInList);
        ICustomBasicList<T> GetRandomList();
        ICustomBasicList<T> GetRandomList(bool removePrevious);
        ICustomBasicList<T> GetConditionalItems(Predicate<T> match); //if you wanted to remove, there is another method that gives you the items and removes.
        void Sort(); //i do want all to have the ability to sort.
        //that way i can decide whether a behavior does it or if the list does it.
        //if there is something in common but not completely, then the isort.
        //otherwise, the abstract class will do it.
        void Sort(Comparison<T> comparison);
        void Sort(int index, int count, IComparer<T> comparer);
        void Sort(IComparer<T> comparer);
        void InsertRange(int index, IEnumerable<T> items, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match1">This is the criteria for doing all</param>
        /// <param name="AllAction">This will be performed for all matching criteria</param>
        /// <param name="match2">This is the second criteria</param>
        /// <param name="SpecificAction">This is the specific action.  The first match it finds, performs the actions and stops</param>
        void ComplexAction(Predicate<T> match1, Action<T> AllAction, Predicate<T> match2, Action<T> SpecificAction);
        bool ForSpecificItem(Predicate<T> match, Action<T> action, int HowManyToCheck = 0); //if 0, then it means that it won't check any
        int Capacity { get; set; }
        void TrimExcess();
        void InsertMiddle(int index, T value);
        void InsertBeginning(T value); //this means it adds to the beginning.  i like this idea.
        void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action);
        void RemoveSeveralConditionalItems(CustomBasicList<ConditionActionPair<T>> ThisList);
        void RemoveAllOnly(Predicate<T> match); //looks like i can't return anything after all.
        ICustomBasicList<U> ConvertAll<U>(Converter<T, U> converter);
        ICustomBasicList<T> RemoveAllAndObtain(Predicate<T> match); //this means whatever the condition is, it removes and you receive it.
        void KeepConditionalItems(Predicate<T> match);
        ICustomBasicList<T> GetRange(int index, int count);
        void MoveItem(T item, int newIndex); //this will move an existing item to a new index.
        void Reverse();
        void RemoveFirstItem();
        void RemoveLastItem();
    }
}