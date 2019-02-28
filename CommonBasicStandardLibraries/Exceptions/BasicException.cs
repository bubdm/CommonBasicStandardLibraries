using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Exceptions
{
     public class BasicBlankException : Exception
    {
        public BasicBlankException() : base(){ }


        public BasicBlankException(string Message) : base(Message) { }

        public BasicBlankException(string Message, Exception innerexception) : base(Message, innerexception) { }  //i think it needs to be flexible enough to not just do my custom exception.

        public override string Message =>  $"Custom Basic Exception.  Message Is {base.Message}"; 
    }

    public class CustomArgumentException : BasicBlankException
    {
        public CustomArgumentException() : base() { }

         
        public CustomArgumentException(string Message) : base(Message) { }

        public CustomArgumentException(string Message, Exception innerexception) : base(Message, innerexception) { }

        public virtual string ParamName { get; }

        //needs 2 more arguments 
        public CustomArgumentException(string ParamName, string Message) : base(Message)
        {
            this.ParamName = ParamName;
        }

        public CustomArgumentException(string ParamName, string Message, Exception innerexception) : base(Message, innerexception)
        {
            this.ParamName = ParamName;
        }

        public override string Message => $"Custom Argument Exception.  Message Is {base.Message}";

    }


    
    
}
