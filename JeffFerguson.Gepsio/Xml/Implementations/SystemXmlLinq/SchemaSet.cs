using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// XML schema set support for Gepsio's SystemXmlLinq implementation.
    /// </summary>
    /// <remarks>
    /// The System.Xml.Linq namespace does not support XML schemas so this implementation
    /// continues to rely on the schema support in System.Xml.
    /// </remarks>
    internal class SchemaSet : ISchemaSet
    {
        private XmlSchemaSet thisSchemaSet;
        private LinkbaseDocumentCollection thisLinkbaseDocuments = new( );
        private Dictionary<IQualifiedName, ISchemaElement> thisGlobalElements;
        private Dictionary<IQualifiedName, ISchemaType> thisGlobalTypes;
        private Dictionary<IQualifiedName, ISchemaAttribute> thisGlobalAttributes;

        public LinkbaseDocumentCollection LinkbaseDocuments => thisLinkbaseDocuments;

        public Dictionary<IQualifiedName, ISchemaElement> GlobalElements
        {
            get
            {
                if (thisGlobalElements == null)
                {
                    var newDictionary = new Dictionary<IQualifiedName, ISchemaElement>();
                    foreach (DictionaryEntry currentEntry in thisSchemaSet.GlobalElements)
                    {
                        var key = new QualifiedName(currentEntry.Key as XmlQualifiedName);
                        var value = currentEntry.Value as XmlSchemaElement;
                        var convertedValue = new SchemaElement(value);
                        newDictionary.Add(key, convertedValue);
                    }
                    thisGlobalElements = newDictionary;
                }
                return thisGlobalElements;
            }
        }

        public Dictionary<IQualifiedName, ISchemaType> GlobalTypes
        {
            get
            {
                if (thisGlobalTypes == null)
                {
                    var newDictionary = new Dictionary<IQualifiedName, ISchemaType>();
                    foreach (DictionaryEntry currentEntry in thisSchemaSet.GlobalTypes)
                    {
                        var key = new QualifiedName(currentEntry.Key as XmlQualifiedName);
                        var value = currentEntry.Value as XmlSchemaType;
                        var convertedValue = new SchemaType(value);
                        newDictionary.Add(key, convertedValue);
                    }
                    thisGlobalTypes = newDictionary;
                }
                return thisGlobalTypes;
            }
        }

        public Dictionary<IQualifiedName, ISchemaAttribute> GlobalAttributes
        {
            get
            {
                if (thisGlobalAttributes == null)
                {
                    thisGlobalAttributes = new Dictionary<IQualifiedName, ISchemaAttribute>();
                    foreach (DictionaryEntry currentEntry in thisSchemaSet.GlobalAttributes)
                    {
                        var key = new QualifiedName(currentEntry.Key as XmlQualifiedName);
                        var value = currentEntry.Value as XmlSchemaAttribute;
                        var convertedValue = new SchemaAttribute(value);
                        thisGlobalAttributes.Add(key, convertedValue);
                    }
                }
                return thisGlobalAttributes;
            }
        }

        public IEnumerable< ISchema > Schemas => thisSchemaSet.Schemas( ).Cast< XmlSchema >( ).Select( xmlSchema => new Schema( xmlSchema ) );

        public void Add(ISchema schemaToAdd)
        {
            var schemaImplementation = schemaToAdd as Schema;
            thisSchemaSet.Add(schemaImplementation.XmlSchema);
        }

        public void Compile()
        {
            thisSchemaSet.Compile();
        }

        public SchemaSet()
        {
            thisSchemaSet = new XmlSchemaSet();
            // NEW BEGIN
            // Possible issue:
            // http://stackoverflow.com/questions/7500636/xml-validation-error-using-nested-xsd-schema-type-not-declared
            //
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
            thisSchemaSet.XmlResolver = resolver;
            // NEW END
            thisGlobalElements = null;
            thisGlobalTypes = null;
        }
    }
}
