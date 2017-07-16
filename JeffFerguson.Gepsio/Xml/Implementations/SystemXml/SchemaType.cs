using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    /// <summary>
    /// An encapsulation of a global type defined in an XML schema. The class supports both simple and complex types.
    /// </summary>
    internal class SchemaType : ISchemaType
    {
        private XmlSchemaType schemaType;
        private IQualifiedName thisQualifiedName;
        private ISchemaType thisBaseSchemaType;
        private XmlSchemaComplexType thisComplexType;
        private List<ISchemaAttribute> thisSchemaAttributes;

        public IQualifiedName QualifiedName
        {
            get
            {
                if (thisQualifiedName == null)
                    thisQualifiedName = new QualifiedName(schemaType.QualifiedName);
                return thisQualifiedName;
            }
        }

        public string Name
        {
            get
            {
                return schemaType.Name;
            }
        }

        public bool IsNumeric
        {
            get
            {
                switch (schemaType.TypeCode)
                {
                    case XmlTypeCode.Decimal:
                    case XmlTypeCode.Double:
                    case XmlTypeCode.Float:
                    case XmlTypeCode.Int:
                    case XmlTypeCode.Integer:
                    case XmlTypeCode.Long:
                    case XmlTypeCode.NegativeInteger:
                    case XmlTypeCode.NonNegativeInteger:
                    case XmlTypeCode.NonPositiveInteger:
                    case XmlTypeCode.PositiveInteger:
                    case XmlTypeCode.Short:
                    case XmlTypeCode.UnsignedInt:
                    case XmlTypeCode.UnsignedLong:
                    case XmlTypeCode.UnsignedShort:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsComplex
        {
            get;
            private set;
        }

        public bool DerivedByRestriction
        {
            get;
            private set;
        }

        public ISchemaType BaseSchemaType
        {
            get
            {
                if (DerivedByRestriction == false)
                    return null;
                if (thisBaseSchemaType == null)
                    thisBaseSchemaType = new SchemaType(thisComplexType.BaseXmlSchemaType);
                return thisBaseSchemaType;
            }
        }

        internal SchemaType(XmlSchemaType type)
        {
            schemaType = type;
            IsComplex = false;
            DerivedByRestriction = false;
            thisComplexType = null;
            thisSchemaAttributes = new List<ISchemaAttribute>();
            if (schemaType is XmlSchemaComplexType)
            {
                IsComplex = true;
                thisComplexType = schemaType as XmlSchemaComplexType;
                if (this.thisComplexType.DerivedBy == XmlSchemaDerivationMethod.Restriction)
                    DerivedByRestriction = true;
                thisSchemaAttributes.Capacity = thisComplexType.AttributeUses.Count;
                foreach (System.Collections.DictionaryEntry currentEntry in thisComplexType.AttributeUses)
                {
                    var newAttribute = new SchemaAttribute(currentEntry.Value as XmlSchemaAttribute);
                    thisSchemaAttributes.Add(newAttribute);
                }
            }
            thisQualifiedName = null;
            thisBaseSchemaType = null;
        }

        public ISchemaAttribute GetAttribute(string name)
        {
            foreach(var currentAttribute in thisSchemaAttributes)
            {
                if(string.IsNullOrEmpty(currentAttribute.Name) == false)
                {
                    if (currentAttribute.Name.Equals(name) == true)
                        return currentAttribute;
                }
            }
            return null;
        }
    }
}
