using System;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParser
{
    public enum EnumMethod
    {
        GetSomeInfo = 1,
        GetTopInfo = 2,
        GetBottomInfo = 3,
        GetList = 4
    }
    public class ParserException : Exception
    {
        public string OriginalBody { get; set; } = ""; // this is the html text.  there is link.  but that is not correct because its not a link
        public string FirstTag { get; set; } = "";
        public string SecondTag { get; set; } = "";
        public string RemainingHtml { get; set; } = "";
        public EnumMethod Method { get; }
        public ParserException(string message, EnumMethod method) : base(message)
        {
            Method = method;
        }
    }


    public enum EnumLocation
    {
        Beginning = 1,
        Ending = 2
    }

    public class MissingTags : Exception
    {
        public EnumLocation Location { get; }
        public override string Message
        {
            get
            {
                if (Location == EnumLocation.Beginning)
                    return "Must have the start tag filled out first";
                if (Location == EnumLocation.Ending)
                    return "Must have the end tag filled out first";
                throw new Exception("Can't figure out the message");
            }
        }

        public MissingTags(EnumLocation tagLocation)
        {
            Location = tagLocation;
        }
    }
}
