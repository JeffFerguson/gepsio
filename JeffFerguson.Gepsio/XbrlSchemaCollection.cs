using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System.Collections;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A collection of XBRL schemas.
    /// </summary>
    /// <remarks>
    /// This class contains, among other things, wrappers for methdods on the <see cref="XbrlSchema"/> class. The wrappers
    /// iterate through each schema to find requested information, freeing the caller from having to iterate through
    /// multiple schemas to find information.
    /// </remarks>
    public class XbrlSchemaCollection : IEnumerable<XbrlSchema>
    {
        internal List<XbrlSchema> SchemaList { get; private set; }

        /// <summary>
        /// The number of schemas in the collection.
        /// </summary>
        public int Count
        {
            get { return SchemaList.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public XbrlSchema this[int index]
        {
            get { return SchemaList[index]; }
            set { SchemaList.Insert(index, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<XbrlSchema> GetEnumerator()
        {
            return SchemaList.GetEnumerator();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal XbrlSchemaCollection()
        {
            SchemaList = new List<XbrlSchema>();
        }

        /// <summary>
        /// Adds a schema to the schema collection.
        /// </summary>
        /// <remarks>
        /// The supplied schema will not be added if its target namespace is already in the list. This will help
        /// with some of the XBRL instance documents in the XBRL Conformance Suite which uses both the "schemaLocation"
        /// attribute as well as a "schemaRef" node to specify the same schema. The "301-01-IdScopeValid.xml"
        /// instance document in the XBRL-CONF-CR5-2012-01-24 suite is one such example.
        /// </remarks>
        /// <param name="schemaToAdd">
        /// The schema to be added.
        /// </param>
        internal void Add(XbrlSchema schemaToAdd)
        {
            var targetNamespaceAlreadyInList = false;
            foreach (var currentSchema in SchemaList)
            {
                if (schemaToAdd.TargetNamespace.Equals(currentSchema.TargetNamespace) == true)
                    targetNamespaceAlreadyInList = true;
            }
            if (targetNamespaceAlreadyInList == false)
                SchemaList.Add(schemaToAdd);
        }

        /// <summary>
        /// Gets the schema containing the element with the given local name.
        /// </summary>
        /// <param name="elementLocalName">
        /// The local name of the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the schema containing the element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        public XbrlSchema GetSchemaContainingElement(string elementLocalName)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var FoundElement = CurrentSchema.GetElement(elementLocalName);
                if (FoundElement != null)
                    return CurrentSchema;
            }
            return null;
        }

        /// <summary>
        /// Finds an defined schema element with a given local name.
        /// </summary>
        /// <param name="elementLocalName">
        /// The local name of the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the matching element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        public Element GetElement(string elementLocalName)
        {
            var containingSchema = GetSchemaContainingElement(elementLocalName);
            return containingSchema?.GetElement(elementLocalName);
        }

        /// <summary>
        /// Gets the schema having the target namespace.
        /// </summary>
        /// <param name="targetNamespace">
        /// The namespace whose schema should be returned.
        /// </param>
        /// <param name="parentFragment">
        /// The fragment containing the schema reference.
        /// </param>
        /// <returns>
        /// A reference to the schema matching the target namespace. A null reference will be returned
        /// if no matching schema can be found. 
        /// </returns>
        public XbrlSchema GetSchemaFromTargetNamespace(string targetNamespace, XbrlFragment parentFragment)
        {
            return FindSchema(targetNamespace);
        }

        /// <summary>
        /// Gets the schema having the target namespace.
        /// </summary>
        /// <param name="targetNamespace">
        /// The namespace whose schema should be returned.
        /// </param>
        /// <returns>
        /// A reference to the schema matching the target namespace. A null reference will be returned
        /// if no matching schema can be found. 
        /// </returns>
        private XbrlSchema FindSchema(string targetNamespace)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                if (CurrentSchema.TargetNamespace.Equals(targetNamespace))
                    return CurrentSchema;
                if (CurrentSchema.TargetNamespaceAlias.Equals(targetNamespace))
                    return CurrentSchema;
            }
            return null;
        }

        /// <summary>
        /// Gets the data type for the supplied node.
        /// </summary>
        /// <param name="node">
        /// The node whose data type is returned.
        /// </param>
        /// <returns>
        /// The data type of the supplied node. A null reference will be returned
        /// if no matching element can be found for the supplied node.
        /// </returns>
        internal AnyType GetNodeType(INode node)
        {
            var containingSchema = GetSchemaContainingElement(node.LocalName);
            var matchingElement = containingSchema?.GetElement(node.LocalName);

            return matchingElement == null ? null : AnyType.CreateType(matchingElement.TypeName.Name, containingSchema);
        }

        /// <summary>
        /// Gets the data type for the supplied attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute whose data type is returned.
        /// </param>
        /// <returns>
        /// The data type of the supplied attribute. A null reference will be returned
        /// if no matching element can be found for the supplied attribute.
        /// </returns>
        internal AnyType GetAttributeType(IAttribute attribute)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingAttributeType = CurrentSchema.GetAttributeType(attribute);
                if (matchingAttributeType != null)
                {
                    return matchingAttributeType;
                }
            }
            return null;
        }

        /// <summary>
        /// Locates and element using an element locator.
        /// </summary>
        /// <param name="ElementLocator">
        /// A locator for the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the matching element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        internal Element LocateElement(Locator ElementLocator)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingElement = CurrentSchema.LocateElement(ElementLocator);
                if (matchingElement != null)
                    return matchingElement;
            }
            return null;
        }

        /// <summary>
        /// Finds the <see cref="RoleType"/> object having the given ID.
        /// </summary>
        /// <param name="RoleTypeId">
        /// The ID of the role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="RoleType"/> object having the given ID, or null if no
        /// object can be found.
        /// </returns>
        internal RoleType GetRoleType(string RoleTypeId)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingRoleType = CurrentSchema.GetRoleType(RoleTypeId);
                if (matchingRoleType != null)
                    return matchingRoleType;
            }
            return null;
        }

        /// <summary>
        /// Finds the <see cref="CalculationLink"/> object having the given role.
        /// </summary>
        /// <param name="CalculationLinkRole">
        /// The role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="CalculationLink"/> object having the given role, or
        /// null if no object can be found.
        /// </returns>
        public CalculationLink GetCalculationLink(RoleType CalculationLinkRole)
        {
            foreach (var currentSchema in SchemaList)
            {
                var calculationLinkCandidate = currentSchema.GetCalculationLink(CalculationLinkRole);
                if (calculationLinkCandidate != null)
                    return calculationLinkCandidate;
            }
            return null;
        }
    }
}
