using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.XMLHelpers
{
    //since this is not used too much, try to keep it async.  may change it later though.
    public class XMLHelper
    {
        //i was going to do as static but i think i should not do that this time.
        private XElement? _thisElement;
        private readonly XmlWriter _writes;
        private readonly CustomWriter _texts;
        private bool _closed;
        public async Task<XElement> GetElementAsync()
        {
            Check(); //i should check here too.
            _closed = true; //at this point, you should not add anything else to it.  the rest has to be done with the old fashioned xelement.
            await _writes.WriteEndDocumentAsync();
            await _writes.FlushAsync();
            _writes.Close();
            _thisElement = XElement.Parse(_texts.ToString());
            return _thisElement;
        }
        private void Check()
        {
            if (_closed == true)
                throw new BasicBlankException("You already closed. Therefore, can't add more to the writer.  Try creating a new object or waiting you are you truly finished before getting the xelement");
        }
        public async Task StartDocumentAsync(string tag)
        {
            Check();
            await _writes.WriteStartDocumentAsync();
            await Task.Run(() =>
            {
                _writes.WriteStartElement(tag);
            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag">Starting Tag</param>
        /// <param name="attributePairs">The attributes for the starting tag</param>
        /// <returns></returns>
        public async Task StartDocumentAsync(string tag, params XMLPair[] attributePairs)
        {
            Check();
            await StartDocumentAsync(tag);
            CustomBasicList<XMLPair> thisList = attributePairs.ToCustomBasicList();
            await WriteAttributesAsync(thisList);
        }
        private async Task WriteAttributesAsync(CustomBasicList<XMLPair> attributePairs)
        {
            await attributePairs.ForEachAsync(async items =>
            {
                await Task.Run(() => { _writes.WriteAttributeString(items.AttributeOrTag, items.Value); });
            });
        }
        public async Task WriteStringAsync(string thisStr)
        {
            Check();
            await _writes.WriteStringAsync(thisStr);
        }
        private async Task WriteElementsAsync(CustomBasicList<XMLPair> elementPairs)
        {
            await elementPairs.ForEachAsync(async items =>
            {
                await Task.Run(() =>
                {
                    _writes.WriteStartElement(items.AttributeOrTag);
                    _writes.WriteString(items.Value);
                    _writes.WriteEndElement();
                });
            });
        }
        public async Task WriteSeveralElementsAsync(params XMLPair[] elementPairs)
        {
            Check();
            CustomBasicList<XMLPair> thisList = elementPairs.ToCustomBasicList();
            await WriteElementsAsync(thisList);
        }

        public async Task WriteSeveralElementsAsync(string commonAttribute, string commonValue, params XMLPair[] elementPairs)
        {
            Check();
            CustomBasicList<XMLPair> thisList = elementPairs.ToCustomBasicList();
            await thisList.ForEachAsync(async items =>
            {
                await Task.Run(() =>
                {
                    _writes.WriteStartElement(items.AttributeOrTag);
                    _writes.WriteAttributeString(commonAttribute, commonValue);
                    _writes.WriteString(items.Value);
                    _writes.WriteEndElement();
                });
            });
        }

        /// <summary>
        /// This writes when you have an existing tag
        /// </summary>
        /// <param name="attributePairs">Several Attributes</param>
        /// <returns></returns>
        public async Task WriteSeveralAttributesAsync(params XMLPair[] attributePairs)
        {
            Check();
            CustomBasicList<XMLPair> thisList = attributePairs.ToCustomBasicList();
            await WriteAttributesAsync(thisList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag">This starts a new tag</param>
        /// <param name="attributePairs">Several Attributes</param>
        /// <returns></returns>
        public async Task WriteSeveralAttributesAsync(string tag, params XMLPair[] attributePairs)
        {
            Check();
            await Task.Run(() =>
            {
                _writes.WriteStartElement(tag);
            });
            CustomBasicList<XMLPair> thisList = attributePairs.ToCustomBasicList();
            await WriteAttributesAsync(thisList);
        }
        public async Task WriteEndElementAsync()
        {
            Check();
            await _writes.WriteEndElementAsync();
        }
        public async Task WriteStartElementAsync(string tag)
        {
            Check();
            await Task.Run(() =>
            {
                _writes.WriteStartElement(tag);
            });
        }
        public async Task WriteAttributesAlone(string tag, params XMLPair[] attributePairs)
        {
            await WriteStartElementAsync(tag);
            CustomBasicList<XMLPair> ThisList = attributePairs.ToCustomBasicList();
            await WriteAttributesAsync(ThisList);
            await _writes.WriteStringAsync("");
            await _writes.WriteEndElementAsync();
        }
        public XMLHelper()
        {
            _texts = new CustomWriter();
            XmlWriterSettings sets = new XmlWriterSettings();
            sets.Indent = true;
            sets.Async = true; //i think this was needed as well.
            _writes = XmlWriter.Create(_texts, sets); //if you wanted to do async and there was no way around it, then you have to have a static method that will give you the object needed
        }
    }
}