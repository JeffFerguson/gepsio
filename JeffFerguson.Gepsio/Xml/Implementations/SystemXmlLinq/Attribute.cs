using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Globalization;
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
        private object thisTypedValue;

        public string LocalName { get; private set; }

        public string Prefix { get; private set; }

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        /// <remarks>
        /// This value is the original string-based value of the attribute as found in the
        /// XML node. No conversions or modifications have been made to this value, and it is
        /// returned to callers as it originally appeared in the node.
        /// </remarks>
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
            thisTypedValue = null;
        }

        internal Attribute(XmlAttribute attribute)
        {
            this.LocalName = attribute.LocalName;
            this.Prefix = attribute.Prefix;
            this.Value = attribute.Value;
            this.Name = attribute.Name;
            this.NamespaceURI = attribute.NamespaceURI;
            thisTypedValue = null;
        }

        /// <summary>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned, in which case TypedValue
        /// returns the same string as what the Value property returns.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The typed value of an attribute differs from the attribute's Value property.
        /// A typed value is the attribute's value coerced into the data type of the
        /// attribute as defined by the schema. For example, if an attribute's value
        /// is "4.56", and it is defined in an XML schema as being of type "double",
        /// then the typed value for the attribute will be a value of .NET type "double"
        /// and having a value of 4.56. The raw string value of the attribute's value
        /// is coerced into a datatype that matches the attribute's definition in a
        /// schema.
        /// </para>
        /// <para>
        /// Because the datatype of an attribute cannot be known until runtime, and,
        /// specifically, when an XML schema is read and parsed, this method cannot know
        /// at compile-time what datatype should be returned. Because of this, the
        /// method will just return the value in an object.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned.
        /// </returns>
        public object GetTypedValue(XbrlFragment containingFragment)
        {
            if(thisTypedValue == null)
            {
                InitializeTypedValue(containingFragment);
            }
            return thisTypedValue;
        }

        /// <summary>
        /// Initialize the attribute's typed value.
        /// </summary>
        /// <param name="containingFragment">
        /// The fragment containing the attribute.
        /// </param>
        private void InitializeTypedValue(XbrlFragment containingFragment)
        {
            var attributeType = containingFragment.Schemas.GetAttributeType(this);
            if (attributeType == null)
            {
                thisTypedValue = this.Value;
                return;
            }
            if (attributeType is Xsd.String)
            {
                thisTypedValue = this.Value;
                return;
            }
            if (attributeType is Xsd.Decimal)
            {
                thisTypedValue = Convert.ToDecimal(Value, CultureInfo.InvariantCulture);
                return;
            }
            if (attributeType is Xsd.Double)
            {
                thisTypedValue = Convert.ToDouble(Value, CultureInfo.InvariantCulture);
                return;
            }
            if (attributeType is Xsd.Boolean)
            {
                // The explicit checks for "1" and "0" are in place to satisfy conformance test
                // 331-equivalentRelationships-instance-13.xml. Convert.ToBoolean() does not convert these values
                //to Booleans.
                if (Value.Equals("1") == true)
                    thisTypedValue = true;
                else if (Value.Equals("0") == true)
                    thisTypedValue = false;
                else
                    thisTypedValue = Convert.ToBoolean(Value, CultureInfo.InvariantCulture);
                return;
            }
            thisTypedValue = this.Value;
        }

        /// <summary>
        /// Compares the typed value of this attribute with the typed value of another attribute.
        /// </summary>
        /// <param name="otherAttribute">
        /// The other attribute whose typed value is to be compared with this attribute's typed value.
        /// </param>
        /// <param name="containingFragment">
        /// The fragment containing the attributes.
        /// </param>
        /// <returns>
        /// True if the attributes have the same typed value; false otherwise.
        /// </returns>
        public bool TypedValueEquals(IAttribute otherAttribute, XbrlFragment containingFragment)
        {
            var thisTypedValue = this.GetTypedValue(containingFragment);
            var otherTypedValue = otherAttribute.GetTypedValue(containingFragment);
            if ((thisTypedValue == null) && (otherTypedValue == null))
            {
                return this.Value.Equals(otherAttribute.Value);
            }
            if ((thisTypedValue == null) || (otherTypedValue == null))
            {
                return false;
            }
            if (thisTypedValue.GetType() == otherTypedValue.GetType())
            {
                return thisTypedValue.Equals(otherTypedValue);
            }
            return false;
        }
    }
}
