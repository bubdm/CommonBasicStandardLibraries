using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using static  CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
namespace CommonBasicStandardLibraries.CollectionClasses
{

    public interface IListFactory<T>
    {

        Type SendingType { get; set; } //we have to risk runtime errors unless somebody knows something else.

        ICustomBasicList<T> GetStartList(); //based on the requested type, will create this new one.  will use some logic statments based on factory chosen.

        IListFactory<U> GetNewFactory<U>(); //maybe i have to figure out how to get the new factory.  doing the same thing but producing another generic type.

        //L ICustomBasicList<T> GetStartList(); //this specializes in figuring out which list to start creating.
         
        //this needs to be a list which represents which list is using this.
    }

    public interface IListModifiers<T>
    {
        //this will allow the possibilities of having at least some shared behaviors.
        //this will be used for the game classes  the base class will use blank behaviors (if not the game package).
        void Add(T value);

        //void AddRange(IEnumerable<T> ThisList, Optional notificationmode As NotifyCollectionChangedAction = NotifyCollectionChangedAction.Add); //sometimes we will want to add a range.

        void AddRange(IEnumerable<T> ThisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Add);

        void LoadStartLists(IEnumerable<T> ThisList);

        //this for sure modifies behavior.
        //no need to insert in specific places because this is for hidden dictionaries.
        bool RemoveSpecificItem(T value);



		void ReplaceRange(IEnumerable<T> ThisList);

        //its needed for this interface so if there is a list of items being removed, then sends to this list.

        void ReplaceAllWithGivenItem(T value); //this means that the item that is sent in, the list will be set to the new item.

        void ReplaceItem(T OldItem, T NewItem);
        void Clear();
        //this can do more but this part does not need to know about it.
        //since we never know how this will come about, needs to allow anything for those.
    }
    public interface ISimpleList<T> : IEnumerable<T>
    {
        //these are all things that can't add or remove from list if you are using this and not the custom list.
        Task ForEachAsync(ActionAsync<T> action);
        //its possible for there to be an await.

        void ForEach(Action<T> action);

        void ForConditionalItems(Predicate<T> match, Action<T> action);
        Task ForConditionalItemsAsync(Predicate<T> match, ActionAsync<T> action);
        int Count { get; }

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

        //T GetSpecificItem(int Index);

        //bool IsReadOnly { get; }

        //i don't know if i need this to mark as readonly or not (?)
        //i can always change it if necessary.  for now, lets not.

        //for now, don't worry about IsReadOnly

        int IndexOf(T value);

        int IndexOf(T value, int Index);

        int IndexOf(T value, int Index, int Count);
        bool TrueForAll(Predicate<T> match); //i like this idea


    }
    public interface ICustomBasicList<T> : ISimpleList<T>, IListModifiers<T> 
    {
        //has to decide on everything that is needed.
        //this is everything that needs to be implemented if it wants to be a custom list.
        //i think that random should be done elsewhere.
        
        //can go ahead and risk allowing anybody to send in a factory.

        IListFactory<T> FactoryRequested { get; set; }
        IResolver MainContainer { set; } //private read though.
        void RemoveAt(int Index);
        void RemoveGivenList(IEnumerable<T> ThisList, NotifyCollectionChangedAction notificationmode = NotifyCollectionChangedAction.Reset); //this means if you have a list and anything on this list needs to be removed, it can be done.
        //looks like maybe not needed because i usually look through anyways.

        void RemoveRange(int index, int count);
        void ShuffleList(); //i think we can't go wrong with this.   whether i do lots of copy/paste, or find a routine, does not matter.

        void ShuffleList(int HowMany); //this means you only keep a certain number

        //i think its best to make this a separate interface.
        //so if i ever have a specialized way to do random, this can be done.
        //as long as is does the things required, it will be a random interface.

        //if i use separately, then this class is responsible to have something to hold on to so it can do the function.
        //if i decide to not implement it, then won't
        //we may have to see what patterns we can use for this problem.
        T GetRandomItem();
        T GetRandomItem(bool RemovePrevious);

        void RemoveRandomItems(int HowMany);

        ICustomBasicList<T> GetRandomList(bool RemovePrevious, int HowManyInList);

        ICustomBasicList<T> GetRandomList();

        ICustomBasicList<T> GetRandomList(bool RemovePrevious);

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

        //this means once the condition is met, will invoke the action.
        //if nothing is found, do nothing
        //needs to return so a person knows if not found, then do nothing

        bool ForSpecificItem(Predicate<T> match, Action<T> action, int HowManyToCheck = 0); //if 0, then it means that it won't check any


        //i don't think we need to skip any


        //so there are 2 ways to do the actions.

        //object SyncRoot { get; } //this was a hint from the icollection class.

        //somehow i could not find it anymore.



        int Capacity { get; set; }
        void TrimExcess();

        //its obvious you have to be ienumerable.
        //i think that everything that implements from this needs to get the index.

        T this[int index]
        {
            get;
            set;
        }


        void InsertMiddle(int Index, T value);

        void InsertBeginning(T value); //this means it adds to the beginning.  i like this idea.


        void RemoveOnlyOneAfterAction(Predicate<T> match, Action<T> action);
        void RemoveSeveralConditionalItems(CustomBasicList<ConditionActionPair<T>> ThisList);

        void RemoveAllOnly(Predicate<T> match); //looks like i can't return anything after all.
        ICustomBasicList<U> ConvertAll<U>(Converter<T, U> converter);



        ICustomBasicList<T> RemoveAllAndObtain(Predicate<T> match); //this means whatever the condition is, it removes and you receive it.


		//i do like the idea of the opposite where it only keeps certain ones.
		//if its only going to keep, then should not be a need to actually get the list.  if i am wrong, can add

		void KeepConditionalItems(Predicate<T> match);

        ICustomBasicList<T> GetRange(int index, int count);

        



        //will keep here.  if there is repeating because of inheritance problem, then can put to another interface and this one one can implement it as well.

        //the following could be iffy.  not sure though.
        void Reverse();


        

        void RemoveFirstItem();
        void RemoveLastItem();
        //if i need more behaviors, i can do this as well.
        //well see.
    }
}
