using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class Attribute : IAttribute
    {
        private XmlAttribute thisXmlAttribute;

        public string LocalName => thisXmlAttribute.LocalName;

        public string Prefix => thisXmlAttribute.Prefix;

        public string Value => thisXmlAttribute.Value;

        public string Name => thisXmlAttribute.Name;

        public string NamespaceURI => thisXmlAttribute.NamespaceURI;

        internal Attribute(XmlAttribute xmlAttribute)
        {
            thisXmlAttribute = xmlAttribute;
        }
    }
}
