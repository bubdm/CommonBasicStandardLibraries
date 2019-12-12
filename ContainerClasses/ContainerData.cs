using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using static CommonBasicStandardLibraries.ContainerClasses.ContainerMain;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.ContainerClasses
{
    public class ContainerData : IComparable<ContainerData>
    {

        public Func<object>? SimpleFunction;
        //public Func<object, object> ComplexFunction;
        //i think you may need to inherit a factory.

        //decided to risk not doing factory.


        //public IContainerFactory ThisFact;
        public object? ThisObject { get; set; }
        //public Type ThisType; //i want it to be smart enough for types.
        public Type? TypeIn { get; set; }
        public Type? TypeOut { get; set; }
        public Type? RequestedType { get; set; }
        public bool IsSingle { get; set; }
        public bool WasCustomFunction { get; set; }
        public int Priority { get; set; }
        public int ID { get; set; } //so other ones can be done.
        public object? ExtraDetails { get; set; } //this is used so other factories can decide what to do.
        public bool IntendedTypeOnly { get; set; }
        //public void Check()
        //{
        //    if (ThisFact != null && SimpleFunction != null)
        //        throw new BasicBlankException("You cannot have both simple function and factory.  Choose one.  Otherwise, rethink");
        //}


        int IComparable<ContainerData>.CompareTo(ContainerData other)
        {
            if (IntendedTypeOnly == true && other.IntendedTypeOnly == true)
                throw new BasicBlankException("You can't register 2 types directly that are intended to be types.  Otherwise too complex");
            if (IntendedTypeOnly == true)
                return -1;
            else if (other.IntendedTypeOnly == true)
                return 1; //those has to have first priority period.
            if (ResolveCategory == EnumResolveCategory.ManuelPriority)
                return Priority.CompareTo(other.Priority);
            if (ResolveCategory == EnumResolveCategory.FIFO)
                return ID.CompareTo(other.ID);
            if (ResolveCategory == EnumResolveCategory.LIFO)
                return other.ID.CompareTo(ID);
            //if (ThisFact != null && other.ThisFact != null)
            //    throw new BasicBlankException("You can't have 2 factories.  Otherwise, rethink");
            //if (ThisFact != null)
            //    return -1; //because that comes first.
            //if (other.ThisFact != null)
            //    return 1; //because that one comes first
            if (WasCustomFunction == true && other.WasCustomFunction == true)
                throw new BasicBlankException("You can't have 2 custom functions comparing.  If that is needed, then rethink");
            if (WasCustomFunction == true && other.WasCustomFunction == false)
                return -1;
            if (other.WasCustomFunction == true && WasCustomFunction == false)
                return 1;
            //if (ResolveCategory == EnumResolveCategory.Factory)
            //{
            //    return 0; //try this way (?)
            //}
            throw new BasicBlankException("I think that rethinking is required");
        }
    }
}
