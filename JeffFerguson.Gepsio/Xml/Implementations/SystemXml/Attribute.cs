using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
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

        /// <summary>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned, in which case TypedValue
        /// returns the same string as what the Value property returns.
        /// </summary>
        /// <returns>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned.
        /// </returns>
        public object GetTypedValue(XbrlFragment containingFragment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares the typed value of this attribute with the typed value of another attribute.
        /// </summary>
        /// <param name="otherAttribute">
        /// The other attribute whose typed value is to be compared with this attribute's typed value.
        /// </param>
        /// <param name="containingFragment">
        /// The fragment containing the attribute.
        /// </param>
        /// <returns>
        /// True if the attributes have the same typed value; false otherwise.
        /// </returns>
        public bool TypedValueEquals(IAttribute otherAttribute, XbrlFragment containingFragment)
        {
            var thisTypedValue = this.GetTypedValue(containingFragment);
            var otherTypedValue = otherAttribute.GetTypedValue(containingFragment);
            if (thisTypedValue.GetType() == otherTypedValue.GetType())
            {
                return thisTypedValue.Equals(otherTypedValue);
            }
            return false;
        }
    }
}
