using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// XML schema support for Gepsio's SystemXmlLinq implementation.
    /// </summary>
    /// <remarks>
    /// The System.Xml.Linq namespace does not support XML schemas so this implementation
    /// continues to rely on the schema support in System.Xml.
    /// </remarks>
    internal class Schema : ISchema
    {
        private XmlSchema thisSchema;
        private List<IQualifiedName> thisNamespaceList;

        internal XmlSchema XmlSchema => thisSchema;

        public List<IQualifiedName> Namespaces
        {
            get
            {
                if (thisNamespaceList == null)
                {
                    var newList = new List<IQualifiedName>();
                    var xmlNamespaces = thisSchema.Namespaces.ToArray();
                    newList.Capacity = xmlNamespaces.Length;
                    foreach (var entry in xmlNamespaces)
                    {
                        var newItem = new QualifiedName(entry);
                        newList.Add(newItem);
                    }
                    thisNamespaceList = newList;
                }
                return thisNamespaceList;
            }
        }
        public IEnumerable< ISchemaAppInfo > AppInfo => thisSchema
            .Items.OfType< XmlSchemaAnnotation >( )
            .SelectMany( xmlSchemaAnnotation => xmlSchemaAnnotation
                .Items.OfType< XmlSchemaAppInfo >( )
                .Select( xmlSchemaAppInfo => new SchemaAppInfo( xmlSchemaAppInfo ) )
            );
        public string SourceUri => thisSchema.SourceUri;

        public Schema()
        {
            thisSchema = null;
            thisNamespaceList = null;
        }

        internal Schema(XmlSchema schema)
        {
            thisSchema = schema;
            thisNamespaceList = null;
        }

        public bool Read(string path)
        {
            try
            {
                var schemaReader = XmlTextReader.Create(path);
                thisSchema = XmlSchema.Read(schemaReader, null);
                return true;
            }
            catch (XmlSchemaException)
            {
                return false;
            }
        }
    }
}
