using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "complexType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class ComplexType : AnyType
    {
        private INode thisComplexTypeNode;
        private string thisName;
        private AnySimpleType thisSimpleContentType;
        private AttributeGroup thisAttributeGroup;

        /// <summary>
        /// The name of this type.
        /// </summary>
        public string Name
        {
            get
            {
                return thisName;
            }
        }

        /// <summary>
        /// The value of the type, represented as a <see cref="string"/>.
        /// </summary>
        public override string ValueAsString
        {
            get
            {
                return thisSimpleContentType.ValueAsString;
            }
            set
            {
                thisSimpleContentType.ValueAsString = value;
            }
        }

        /// <summary>
        /// Describes whether or not this type is a numeric type. Returns true if this type is a numeric type. Returns false if this
        /// type is not a numeric type.
        /// </summary>
        public override bool NumericType
        {
            get
            {
                return thisSimpleContentType.NumericType;
            }
        }

        //--------------------------------------------------------------------------------------------------------
        // This constructor is used to construct user-defined complex types defined in XBRL schemas.
        //--------------------------------------------------------------------------------------------------------
        internal ComplexType(INode ComplexTypeNode, INamespaceManager namespaceManager)
        {
            thisAttributeGroup = null;
            thisComplexTypeNode = ComplexTypeNode;
            thisName = ComplexTypeNode.GetAttributeValue("name");
            thisSimpleContentType = null;
            foreach (INode CurrentChildNode in ComplexTypeNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("simpleContent") == true)
                    thisSimpleContentType = new SimpleType(CurrentChildNode, namespaceManager);
            }
        }

        //--------------------------------------------------------------------------------------------------------
        // This constructor is used to construct built-in complex types defined in the XBRL specification.
        //--------------------------------------------------------------------------------------------------------
        internal ComplexType(string Name, AnySimpleType BaseSimpleType, AttributeGroup AttrGroup)
        {
            thisAttributeGroup = AttrGroup;
            thisComplexTypeNode = null;
            thisName = Name;
            thisSimpleContentType = BaseSimpleType;
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue)
        {
            return thisSimpleContentType.GetValueAfterApplyingPrecisionTruncation(PrecisionValue);
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue)
        {
            return thisSimpleContentType.GetValueAfterApplyingDecimalsTruncation(DecimalsValue);
        }

        //--------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
            if (thisSimpleContentType != null)
                thisSimpleContentType.ValidateFact(FactToValidate);
        }
    }
}
