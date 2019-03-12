using System.IO;
using System.Text;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.XMLHelpers
{
    internal class CustomWriter : StringWriter //this guarantees it will be utf8
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}