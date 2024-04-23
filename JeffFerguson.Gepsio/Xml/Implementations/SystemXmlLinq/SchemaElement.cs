using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
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
        private List<ISchemaAttribute> thisSchemaAttributes;

        public string Id => thisSchemaElement.Id;
        public bool IsAbstract => thisSchemaElement.IsAbstract;
        public string Name => thisSchemaElement.Name;
        public string Default => thisSchemaElement.DefaultValue;

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
                        thisUnhandledAttributes = new AttributeList(thisSchemaElement.UnhandledAttributes, null);
                }
                return thisUnhandledAttributes;
            }
        }

        public List<ISchemaAttribute> SchemaAttributes
        {
            get
            {
                if (thisSchemaAttributes == null)
                {
                    thisSchemaAttributes = new List<ISchemaAttribute>();
                    AddElementSchemaTypeAttributes(thisSchemaAttributes);
                    AddContentModelAttributes(thisSchemaAttributes);
                }
                return thisSchemaAttributes;
            }
        }

        internal SchemaElement(XmlSchemaElement element)
        {
            thisSchemaElement = element;
            thisSchemaTypeName = null;
            thisSubstitutionGroup = null;
            thisUnhandledAttributes = null;
            thisSchemaAttributes = null;
        }

        /// <summary>
        /// Add any attributes defined by the element's schema type to the given list.
        /// </summary>
        /// <param name="thisAttributes">
        /// The list to which the attributes should be added.
        /// </param>
        private void AddElementSchemaTypeAttributes(List<ISchemaAttribute> thisAttributes)
        {
            if (thisSchemaElement.ElementSchemaType is XmlSchemaComplexType)
            {
                var elementSchemaComplexType = thisSchemaElement.ElementSchemaType as XmlSchemaComplexType;
                if (elementSchemaComplexType != null)
                {
                    AddAttributesFromSchemaObjectCollection(elementSchemaComplexType.Attributes, thisAttributes);
                }
            }
        }

        /// <summary>
        /// Add attributes to an attribute list from data in an XML Schema object collection.
        /// </summary>
        /// <param name="objectCollection">
        /// The object collection to use as the source of the data.
        /// </param>
        /// <param name="attributeList">
        /// The attribute list to fill with the attribute data in the object collection.
        /// </param>
        private void AddAttributesFromSchemaObjectCollection(XmlSchemaObjectCollection objectCollection, List<ISchemaAttribute> attributeList)
        {
            foreach (var currentObject in objectCollection)
            {
                if (currentObject is XmlSchemaAttribute)
                {
                    attributeList.Add(new SchemaAttribute(currentObject as XmlSchemaAttribute));
                }
            }
        }

        /// <summary>
        /// Add any attributes defined by the element's content model to the given list.
        /// </summary>
        /// <param name="thisAttributes">
        /// The list to which the attributes should be added.
        /// </param>
        private void AddContentModelAttributes(List<ISchemaAttribute> thisAttributes)
        {
            if (thisSchemaElement.ElementSchemaType is XmlSchemaComplexType)
            {
                var elementSchemaComplexType = thisSchemaElement.ElementSchemaType as XmlSchemaComplexType;
                if (elementSchemaComplexType == null)
                {
                    return;
                }
                var contentModel = elementSchemaComplexType.ContentModel as XmlSchemaContentModel;
                if (contentModel == null)
                {
                    return;
                }
                if (contentModel.Content is XmlSchemaComplexContentExtension)
                {
                    var content = contentModel.Content as XmlSchemaComplexContentExtension;
                    AddAttributesFromSchemaObjectCollection(content.Attributes, thisAttributes);
                }
                if (contentModel.Content is XmlSchemaComplexContentRestriction)
                {
                    var content = contentModel.Content as XmlSchemaComplexContentRestriction;
                    AddAttributesFromSchemaObjectCollection(content.Attributes, thisAttributes);
                }
                if (contentModel.Content is XmlSchemaSimpleContentExtension)
                {
                    var content = contentModel.Content as XmlSchemaSimpleContentExtension;
                    AddAttributesFromSchemaObjectCollection(content.Attributes, thisAttributes);
                }
                if (contentModel.Content is XmlSchemaSimpleContentRestriction)
                {
                    var content = contentModel.Content as XmlSchemaSimpleContentRestriction;
                    AddAttributesFromSchemaObjectCollection(content.Attributes, thisAttributes);
                }
            }
        }
    }
}
