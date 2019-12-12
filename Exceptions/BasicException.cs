using System;
namespace CommonBasicStandardLibraries.Exceptions
{
    public class BasicBlankException : Exception
    {
        public BasicBlankException() : base() { }
        public BasicBlankException(string message) : base(message) { }

        public BasicBlankException(string message, Exception innerexception) : base(message, innerexception) { }  //i think it needs to be flexible enough to not just do my custom exception.

        public override string Message => $"Custom Basic Exception.  Message Is {base.Message}";
    }
    public class CustomArgumentException : BasicBlankException
    {
        public CustomArgumentException() : base() { }
        public CustomArgumentException(string message) : base(message) { }
        public CustomArgumentException(string message, Exception innerexception) : base(message, innerexception) { }
        public virtual string ParamName { get; } = "";
        public CustomArgumentException(string paramName, string message) : base(message)
        {
            ParamName = paramName;
        }

        public CustomArgumentException(string paramName, string message, Exception innerexception) : base(message, innerexception)
        {
            ParamName = paramName;
        }
        public override string Message => $"Custom Argument Exception.  Message Is {base.Message}";
    }
}