using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class SimpleCollectionFactory<T> : IListFactory<T>
    {
        public Type SendingType { get; set; }

        public IListFactory<U> GetNewFactory<U>()
        {
            return new SimpleCollectionFactory<U>(); //maybe this simple.  since this logic will be the same.
        }

        public ICustomBasicList<T> GetStartList()
        {
            //this one does not matter because its always going to produce a basic list no matter what.
            //if you want something else, you have to specify.

            return new CustomBasicList<T>();
        } //this one will not care about type.  because it can't do any other anyways.

        

    }
    
}
