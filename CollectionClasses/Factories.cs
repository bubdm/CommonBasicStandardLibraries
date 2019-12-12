using System;
namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class SimpleCollectionFactory<T> : IListFactory<T>
    {
        public Type? SendingType { get; set; }
        public IListFactory<U> GetNewFactory<U>()
        {
            return new SimpleCollectionFactory<U>(); //maybe this simple.  since this logic will be the same.
        }
        public ICustomBasicList<T> GetStartList()
        {
            return new CustomBasicList<T>();
        } 
    }
}