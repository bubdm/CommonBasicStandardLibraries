using System;

namespace CommonBasicStandardLibraries.ContainerClasses
{
    public class ContainerResults
    {
        public bool IsSingle { get; set; }
        public Type? TypeFrom { get; set; }
        public Type? TypeTo { get; set; }
        public object? PayLoad { get; set; }
    }
}
