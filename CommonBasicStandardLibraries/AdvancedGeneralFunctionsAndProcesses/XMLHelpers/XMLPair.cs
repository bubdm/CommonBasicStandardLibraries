//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.XMLHelpers
{
    public class XMLPair
    {
        public string AttributeOrTag { get; private set; }
        public string Value { get; private set; }
        public XMLPair(string tag, string value)
        {
            Value = value;
            AttributeOrTag = tag;
        }
        public XMLPair(string tag, int value)
        {
            Value = value.ToString();
            AttributeOrTag = tag;
        }
    }
}