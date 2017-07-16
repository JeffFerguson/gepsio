using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class Attribute : IAttribute
    {
        private XmlAttribute thisXmlAttribute;

        public string LocalName
        {
            get
            {
                return thisXmlAttribute.LocalName;
            }
        }

        public string Prefix
        {
            get
            {
                return thisXmlAttribute.Prefix;
            }
        }

        public string Value
        {
            get
            {
                return thisXmlAttribute.Value;
            }
        }

        public string Name
        {
            get
            {
                return thisXmlAttribute.Name;
            }
        }

        public string NamespaceURI
        {
            get
            {
                return thisXmlAttribute.NamespaceURI;
            }
        }

        internal Attribute(XmlAttribute xmlAttribute)
        {
            thisXmlAttribute = xmlAttribute;
        }
    }
}
