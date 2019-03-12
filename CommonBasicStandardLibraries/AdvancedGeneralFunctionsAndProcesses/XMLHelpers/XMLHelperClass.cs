using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.XMLHelpers
{
    public class XMLHelper
    {
        //i was going to do as static but i think i should not do that this time.
        private XElement ThisElement;
        private readonly XmlWriter Writes;
        private readonly CustomWriter Texts;
        private bool Closed;
        public async Task<XElement> GetElementAsync()
        {
            Check(); //i should check here too.
            Closed = true; //at this point, you should not add anything else to it.  the rest has to be done with the old fashioned xelement.
            await Writes.WriteEndDocumentAsync();
            await Writes.FlushAsync();
            Writes.Close();
            ThisElement = XElement.Parse(Texts.ToString());
            return ThisElement;
        }
        private void Check()
        {
            if (Closed == true)
                throw new BasicBlankException("You already closed. Therefore, can't add more to the writer.  Try creating a new object or waiting you are you truly finished before getting the xelement");
        }
        public async Task StartDocumentAsync(string Tag)
        {
            Check();
            await Writes.WriteStartDocumentAsync();
            await Task.Run(() =>
            {
                Writes.WriteStartElement(Tag);
            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tag">Starting Tag</param>
        /// <param name="AttributePairs">The attributes for the starting tag</param>
        /// <returns></returns>
        public async Task StartDocumentAsync(string Tag, params XMLPair[] AttributePairs)
        {
            Check();
            await StartDocumentAsync(Tag);
            CustomBasicList<XMLPair> ThisList = AttributePairs.ToCustomBasicList();
            await WriteAttributesAsync(ThisList);
        }
        private async Task WriteAttributesAsync(CustomBasicList<XMLPair> AttributePairs)
        {
            await AttributePairs.ForEachAsync(async Items =>
            {
                await Task.Run(() => { Writes.WriteAttributeString(Items.AttributeOrTag, Items.Value); });
            });
        }
        public async Task WriteStringAsync(string ThisStr)
        {
            Check();
            await Writes.WriteStringAsync(ThisStr);
        }
        private async Task WriteElementsAsync(CustomBasicList<XMLPair> ElementPairs)
        {
            await ElementPairs.ForEachAsync(async Items =>
            {
                await Task.Run(() =>
                {
                    Writes.WriteStartElement(Items.AttributeOrTag);
                    Writes.WriteString(Items.Value);
                    Writes.WriteEndElement();
                });
            });
        }
        //was trying to make a fluent design that was async but did not work.
        //public async Task WriteSingleElementAsync(string Tag, string Value)
        //{
        //    Check();
        //    CustomBasicList<XMLPair> ThisList = new CustomBasicList<XMLPair>() { new XMLPair(Tag, Value)};
        //    await WriteElementsAsync(ThisList);
        //}
        public async Task WriteSeveralElementsAsync(params XMLPair[] ElementPairs)
        {
            Check();
            CustomBasicList<XMLPair> ThisList = ElementPairs.ToCustomBasicList();
            await WriteElementsAsync(ThisList);
        }

        public async Task WriteSeveralElementsAsync(string CommonAttribute, string CommonValue, params XMLPair[] ElementPairs)
        {
            Check();
            CustomBasicList<XMLPair> ThisList = ElementPairs.ToCustomBasicList();
            await ThisList.ForEachAsync(async Items =>
            {
                await Task.Run(() =>
                {
                    Writes.WriteStartElement(Items.AttributeOrTag);
                    Writes.WriteAttributeString(CommonAttribute, CommonValue);
                    Writes.WriteString(Items.Value);
                    Writes.WriteEndElement();
                });
            });
        }

        /// <summary>
        /// This writes when you have an existing tag
        /// </summary>
        /// <param name="AttributePairs">Several Attributes</param>
        /// <returns></returns>
        public async Task WriteSeveralAttributesAsync(params XMLPair[] AttributePairs)
        {
            Check();
            CustomBasicList<XMLPair> ThisList = AttributePairs.ToCustomBasicList();
            await WriteAttributesAsync(ThisList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tag">This starts a new tag</param>
        /// <param name="AttributePairs">Several Attributes</param>
        /// <returns></returns>
        public async Task WriteSeveralAttributesAsync(string Tag, params XMLPair[] AttributePairs)
        {
            Check();
            await Task.Run(() =>
            {
                Writes.WriteStartElement(Tag);
            });
            CustomBasicList<XMLPair> ThisList = AttributePairs.ToCustomBasicList();
            await WriteAttributesAsync(ThisList);
        }
        public async Task WriteEndElementAsync()
        {
            Check();
            await Writes.WriteEndElementAsync();
        }
        public async Task WriteStartElementAsync(string Tag)
        {
            Check();
            await Task.Run(() =>
            {
                Writes.WriteStartElement(Tag);
            });
        }
        public async Task WriteAttributesAlone(string Tag, params XMLPair[] AttributePairs)
        {
            await WriteStartElementAsync(Tag);
            CustomBasicList<XMLPair> ThisList = AttributePairs.ToCustomBasicList();
            await WriteAttributesAsync(ThisList);
            await Writes.WriteStringAsync("");
            await Writes.WriteEndElementAsync();
        }
        public XMLHelper()
        {
            Texts = new CustomWriter();
            XmlWriterSettings sets = new XmlWriterSettings();
            sets.Indent = true;
            sets.Async = true; //i think this was needed as well.
            Writes = XmlWriter.Create(Texts, sets); //if you wanted to do async and there was no way around it, then you have to have a static method that will give you the object needed
        }
    }
}