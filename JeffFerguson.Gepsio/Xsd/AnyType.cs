using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "anyType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public abstract class AnyType
    {

        /// <summary>
        /// The value of the type, represented as a <see cref="string"/>.
        /// </summary>
        public virtual string ValueAsString
        {
            get;
            set;
        }

        /// <summary>
        /// Describes whether or not this type is a numeric type. Returns true if this type is a numeric type. Returns false if this
        /// type is not a numeric type.
        /// </summary>
        public abstract bool NumericType
        {
            get;
        }

        internal AnyType()
        {
        }

        internal virtual void ValidateFact(Item FactToValidate)
        {
        }

        /// <summary>
        /// Creates a XML schema type for use by the internally-implemented XML schema type system.
        /// </summary>
        /// <param name="Schema">
        /// The schema containing the definition of the type.
        /// </param>
        /// <param name="TypeName">
        /// The name of the type to be created. Specific types are created according to the following table:
        /// <list type="table">
        /// <listeader>
        /// <term>
        /// Type Name
        /// </term>
        /// <description>
        /// Specific Class of Returned Object
        /// </description>
        /// </listeader>
        /// <item>
        /// <term>
        /// token
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.Token"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// string
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.String"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:decimalItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.DecimalItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:monetaryItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.MonetaryItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:pureItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.PureItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:sharesItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.SharesItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:tokenItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.TokenItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:stringItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.StringItemType"/>
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// If a type name not shown above is supplied, then null will be returned.
        /// </para>
        /// </param>
        /// <returns>
        /// A type object representing the type referenced by the parameter, or null if the type name is
        /// not supported.
        /// </returns>
        public static AnyType CreateType(string TypeName, XbrlSchema Schema)
        {
            return AnyType.CreateType(TypeName, Schema.SchemaRootNode);
        }

        internal abstract decimal GetValueAfterApplyingPrecisionTruncation(int PrecisionValue);

        internal abstract decimal GetValueAfterApplyingDecimalsTruncation(int DecimalsValue);

        /// <summary>
        /// Creates a XML schema type for use by the internally-implemented XML schema type system.
        /// </summary>
        /// <param name="TypeName">
        /// The name of the type to be created. Specific types are created according to the following table:
        /// <list type="table">
        /// <listeader>
        /// <term>
        /// Type Name
        /// </term>
        /// <description>
        /// Specific Class of Returned Object
        /// </description>
        /// </listeader>
        /// <item>
        /// <term>
        /// token
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.Token"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// string
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.String"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:decimalItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.DecimalItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:monetaryItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.MonetaryItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:pureItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.PureItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:sharesItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.SharesItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:tokenItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.TokenItemType"/>
        /// </description>
        /// </item>
        /// <item>
        /// <term>
        /// xbrli:stringItemType
        /// </term>
        /// <description>
        /// <see cref="JeffFerguson.Gepsio.Xsd.StringItemType"/>
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// If a type name not shown above is supplied, then null will be returned.
        /// </para>
        /// </param>
        /// <param name="SchemaRootNode">
        /// The root node of the schema implementing the type.
        /// </param>
        /// <returns>
        /// A type object representing the type referenced by the parameter, or null if the type name is
        /// not supported.
        /// </returns>
        internal static AnyType CreateType(string TypeName, INode SchemaRootNode)
        {
            AnyType TypeToReturn;

            switch (TypeName)
            {
                case "token":
                    TypeToReturn = new Token(SchemaRootNode);
                    break;
                case "string":
                    TypeToReturn = new String(SchemaRootNode);
                    break;
                case "decimal":
                    TypeToReturn = new Decimal(SchemaRootNode);
                    break;
                case "decimalItemType":
                    TypeToReturn = new DecimalItemType();
                    break;
                case "monetaryItemType":
                    TypeToReturn = new MonetaryItemType();
                    break;
                case "pureItemType":
                    TypeToReturn = new PureItemType();
                    break;
                case "sharesItemType":
                    TypeToReturn = new SharesItemType();
                    break;
                case "tokenItemType":
                    TypeToReturn = new TokenItemType();
                    break;
                case "stringItemType":
                case "normalizedStringItemType":
                    TypeToReturn = new StringItemType();
                    break;
                case "integer":
                    TypeToReturn = new Integer(SchemaRootNode);
                    break;
                case "double":
                    TypeToReturn = new Double(SchemaRootNode);
                    break;
                default:
                    TypeToReturn = null;
                    break;
            }
            return TypeToReturn;
        }

        /// <summary>
        /// Determines whether or not the supplied string value can be converted to a data type
        /// consistent with the class type.
        /// </summary>
        /// <remarks>
        /// This method should be overrridden in derives classes to ensure that the supplied string
        /// value can be converted to a data type consistent with the data type managed by the
        /// derived class.
        /// </remarks>
        /// <param name="valueAsString">
        /// The original string-based value.
        /// </param>
        /// <returns>
        /// True if the supplied string value can be converted to a data type consistent with the
        /// class type; false otherwise.
        /// </returns>
        internal virtual bool CanConvert(string valueAsString)
        {
            return true;
        }
    }
}
