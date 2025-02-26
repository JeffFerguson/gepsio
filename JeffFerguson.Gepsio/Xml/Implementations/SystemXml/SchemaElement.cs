using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class SchemaElement : ISchemaElement
    {
        private XmlSchemaElement thisSchemaElement;
        private IQualifiedName thisSchemaTypeName;
        private IQualifiedName thisSubstitutionGroup;
        private IAttributeList thisUnhandledAttributes;

        public string Id => thisSchemaElement.Id;
        public bool IsAbstract => thisSchemaElement.IsAbstract;
        public string Name => thisSchemaElement.Name;
        public string Default => thisSchemaElement.DefaultValue;
        public string Namespace => thisSchemaElement.QualifiedName.Namespace;
        public string SourceUri => thisSchemaElement.SourceUri;

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

        public List<ISchemaAttribute> SchemaAttributes => throw new NotImplementedException();

        internal SchemaElement(XmlSchemaElement element)
        {
            thisSchemaElement = element;
            thisSchemaTypeName = null;
            thisSubstitutionGroup = null;
            thisUnhandledAttributes = null;
        }
    }
}
