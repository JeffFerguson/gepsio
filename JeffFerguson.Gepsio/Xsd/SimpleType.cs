using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Text;

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

         private void CreateRestrictionType(INode CurrentChildNode, INamespaceManager namespaceManager)
        {
            string BaseValue = CurrentChildNode.Attributes["base"].Value;
            var BaseValueAsQualifiedName = Container.Resolve<IQualifiedName>();
            BaseValueAsQualifiedName.FullyQualifiedName = BaseValue;
            //var NamespaceUri = string.Empty;
            //if (string.IsNullOrEmpty(BaseValueAsQualifiedName.Namespace) == false)
            //    NamespaceUri = namespaceManager.LookupNamespace(BaseValueAsQualifiedName.Namespace);
            thisRestrictionType = AnyType.CreateType(BaseValueAsQualifiedName.Name, CurrentChildNode);
            if (thisRestrictionType == null)
            {
                string MessageFormat = AssemblyResources.GetName("UnsupportedRestrictionBaseSimpleType");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, BaseValue);
                //throw new XbrlException(MessageBuilder.ToString());
            }
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            if (thisRestrictionType != null)
                thisRestrictionType.ValidateFact(FactToValidate);
        }
    }
}
