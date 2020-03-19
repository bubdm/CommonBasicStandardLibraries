using CommonBasicStandardLibraries.Exceptions;
using System;
using static CommonBasicStandardLibraries.ContainerClasses.ContainerMain;
namespace CommonBasicStandardLibraries.ContainerClasses
{
    public class ContainerData : IComparable<ContainerData>
    {

        public Func<object>? SimpleFunction;
        public object? ThisObject { get; set; }
        public Type? TypeIn { get; set; }
        public Type? TypeOut { get; set; }
        public Type? RequestedType { get; set; }
        public bool IsSingle { get; set; }
        public bool WasCustomFunction { get; set; }
        public int Priority { get; set; }
        public int ID { get; set; } //so other ones can be done.
        public object? ExtraDetails { get; set; } //this is used so other factories can decide what to do.
        public bool IntendedTypeOnly { get; set; }


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
            if (WasCustomFunction == true && other.WasCustomFunction == true)
                throw new BasicBlankException("You can't have 2 custom functions comparing.  If that is needed, then rethink");
            if (WasCustomFunction == true && other.WasCustomFunction == false)
                return -1;
            if (other.WasCustomFunction == true && WasCustomFunction == false)
                return 1;
            throw new BasicBlankException("I think that rethinking is required");
        }
    }
}
