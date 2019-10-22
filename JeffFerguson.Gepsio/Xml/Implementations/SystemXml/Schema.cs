using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

using JeffFerguson.Gepsio.IoC;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class Schema : ISchema
    {
        private XmlSchema thisSchema;
        private List<IQualifiedName> thisNamespaceList;

        internal XmlSchema XmlSchema
        {
            get
            {
                return thisSchema;
            }
        }

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
				var settings = new XmlReaderSettings { XmlResolver = Container.Resolve<XmlResolver>( ) };
				using( var schemaReader = XmlTextReader.Create( path, settings ) ) {
					this.thisSchema = XmlSchema.Read(schemaReader, null);
				}

				return true;
            }
            catch(XmlSchemaException)
            {
                return false;
            }
        }
    }
}
