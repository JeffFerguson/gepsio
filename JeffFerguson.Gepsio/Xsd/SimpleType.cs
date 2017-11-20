using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Text;
using System;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "simpleType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class SimpleType : AnySimpleType
    {
        private AnyType thisRestrictionType;

        internal INode SimpleTypeNode
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of the simple type.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

         internal SimpleType(INode SimpleTypeRootNode, INamespaceManager namespaceManager)
        {
            this.SimpleTypeNode = SimpleTypeRootNode;
            this.Name = this.SimpleTypeNode.GetAttributeValue("name");
            foreach (INode CurrentChildNode in SimpleTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("restriction") == true)
                    CreateRestrictionType(CurrentChildNode, namespaceManager);
            }
        }

         private void CreateRestrictionType(INode restrictionNode, INamespaceManager namespaceManager)
        {
            string BaseValue = restrictionNode.Attributes["base"].Value;
            var BaseValueAsQualifiedName = Container.Resolve<IQualifiedName>();
            BaseValueAsQualifiedName.FullyQualifiedName = BaseValue;
            thisRestrictionType = AnyType.CreateType(BaseValueAsQualifiedName.Name, restrictionNode);
            if (thisRestrictionType == null)
            {
                string MessageFormat = AssemblyResources.GetName("UnsupportedRestrictionBaseSimpleType");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, BaseValue);
            }
            foreach(INode childNode in restrictionNode.ChildNodes)
            {
                if (childNode.LocalName.Equals("attribute") == true)
                    ProcessRestrictionAttribute(childNode);
            }
        }

        private void ProcessRestrictionAttribute(INode restrictionAttributeNode)
        {
            var nameAttribute = restrictionAttributeNode.Attributes["name"];
            var useAttribute = restrictionAttributeNode.Attributes["use"];
            var fixedAttribute = restrictionAttributeNode.Attributes["fixed"];
            var typeAttribute = restrictionAttributeNode.Attributes["type"];
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            if (thisRestrictionType != null)
                thisRestrictionType.ValidateFact(FactToValidate);
        }
    }
}
