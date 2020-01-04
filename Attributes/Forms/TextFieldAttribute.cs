using CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels;
using System;
namespace CommonBasicStandardLibraries.Attributes.Forms
{
    //this is intended to use for data entry.  which has label and field that goes along with it.
    public class TextFieldAttribute : Attribute
    {
        public string HeaderLabel { get; set; } = ""; //this means it will be textbox.
        public string CommandPath { get; set; } = "";
        public TextFieldAttribute(string header, string commandPath = "")
        {
            HeaderLabel = header;
            CommandPath = commandPath;
            
        }
        public TextFieldAttribute(string header, bool useSaveCommand)
        {
            HeaderLabel = header;
            if (useSaveCommand)
            {
                CommandPath = nameof(DataEntryViewModel.SaveCommand); //try this way.
            }
        }
    }
}