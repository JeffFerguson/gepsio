using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// XML schema element support for Gepsio's SystemXmlLinq implementation.
    /// </summary>
    /// <remarks>
    /// The System.Xml.Linq namespace does not support XML schemas so this implementation
    /// continues to rely on the schema support in System.Xml.
    /// </remarks>
    internal class SchemaElement : ISchemaElement
    {
        private XmlSchemaElement thisSchemaElement;
        private IQualifiedName thisSchemaTypeName;
        private IQualifiedName thisSubstitutionGroup;
        private IAttributeList thisUnhandledAttributes;

        public string Id => thisSchemaElement.Id;

        public bool IsAbstract => thisSchemaElement.IsAbstract;

        public string Name => thisSchemaElement.Name;

        public IQualifiedName SchemaTypeName
        {
            get
            {
                if (thisSchemaTypeName == null)
                {
                    thisSchemaTypeName = new QualifiedName(thisSchemaElement.SchemaTypeName);
                }
                return thisSchemaTypeName;
            }
        }

        public IQualifiedName SubstitutionGroup
        {
            get
            {
                if (thisSubstitutionGroup == null)
                {
                    thisSubstitutionGroup = new QualifiedName(thisSchemaElement.SubstitutionGroup);
                }
                return thisSubstitutionGroup;
            }
        }

        public IAttributeList UnhandledAttributes
        {
            get
            {
                if (thisUnhandledAttributes == null)
                {
                    if (thisSchemaElement.UnhandledAttributes == null)
                        thisUnhandledAttributes = new AttributeList();
                    else
                        thisUnhandledAttributes = new AttributeList(thisSchemaElement.UnhandledAttributes);
                }
                return thisUnhandledAttributes;
            }
        }

        internal SchemaElement(XmlSchemaElement element)
        {
            thisSchemaElement = element;
            thisSchemaTypeName = null;
            thisSubstitutionGroup = null;
            thisUnhandledAttributes = null;
        }
    }
}
