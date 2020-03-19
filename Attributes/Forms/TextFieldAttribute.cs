using System;
namespace CommonBasicStandardLibraries.Attributes.Forms
{
    //this is intended to use for data entry.  which has label and field that goes along with it.
    public class TextFieldAttribute : Attribute
    {
        public string HeaderLabel { get; set; } = ""; //this means it will be textbox.
        public string MethodName { get; set; } = "";

        public TextFieldAttribute(string header, string methodName = "")
        {
            HeaderLabel = header;
            MethodName = methodName;

        }
        
    }
}