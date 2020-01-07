using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;
using System.Xml.Linq;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// Manages the properties of an XML attribute.
    /// </summary>
    /// <remarks>
    /// This class will be used by both document and schema code. The document support is in the
    /// LINQ-to-XML namespace while the schema support is from the XML namespace that originally
    /// shipped with .NET, since the LINQ-to-XML namespace does not support schemas. Therefore,
    /// this class supports initialization from both types of attributes, initializes properties
    /// from an attribute type, but does not hang on to the source attribute itself.
    /// </remarks>
    internal class Attribute : IAttribute
    {
        public string LocalName { get; private set; }

        public string Prefix { get; private set; }

        public string Value { get; private set; }

        public string Name { get; private set; }

        public string NamespaceURI { get; private set; }

        internal Attribute(XAttribute attribute)
        {
            this.LocalName = attribute.Name.LocalName;
            this.Prefix = attribute.Parent.GetPrefixOfNamespace(attribute.Name.Namespace);
            this.Value = attribute.Value;
            this.NamespaceURI = attribute.Name.Namespace.NamespaceName;
            if (string.IsNullOrEmpty(this.Prefix) == true)
            {
                this.Prefix = string.Empty;
                this.Name = this.LocalName;
            }
            else
            {
                this.Name = $"{this.Prefix}:{this.LocalName}";
            }
        }

        internal Attribute(XmlAttribute attribute)
        {
            this.LocalName = attribute.LocalName;
            this.Prefix = attribute.Prefix;
            this.Value = attribute.Value;
            this.Name = attribute.Name;
            this.NamespaceURI = attribute.NamespaceURI;
        }
    }
}
