using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.ContainerClasses
{
    public interface IContainerFactory
    {
        object GetReturnObject(CustomBasicList<ContainerData> PossibleResults, ContainerData ThisCurrent); //hopefully this works.
        //if you don't need it then ignore.
        //otherwise, too difficult to maintain in a different way.
        //object GetReturnObject(Dictionary<SendInfo, ResultInfo> ThisDict);
        //was going to be a dictionary but that does not work so well.


        object GetReturnObject(CustomBasicList<ContainerData> PossibleResults, Type TypeRequested); //if there is no match here, this will raise the exception.

        bool CanAcceptObject(CustomBasicList<ContainerData> PossibleResults, Type TypeRequested); //that way if none accept, then will raise exception.
    }
}
