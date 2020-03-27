
namespace JeffFerguson.Gepsio.Xml.Interfaces
{
    /// <summary>
    /// An interface to a specific XML implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface provides an abstraction to the actual XML service layer used by Gepsio. Different XML
    /// service layers may be used: the .NET 3.5 implementation may use the System.Xml classes, while a Portable
    /// Class Library implementation may use LINQ-to-XML. This interface abstracts away the XML implementation
    /// specifics so that the rest of Gepsio can use a standard interface to the XML service layer without
    /// knowledge of a specific implementation.
    /// </para>
    /// <para>
    /// The <see cref="JeffFerguson.Gepsio.IoC.Container"/> class provides a simple container mechanism for resolving interface types
    /// into a specific implementation.
    /// </para>
    /// </remarks>
    public interface IAttribute
    {
        /// <summary>
        /// The local name of the attribute.
        /// </summary>
        string LocalName { get; }

        /// <summary>
        /// The prefix of the attribute.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// The string-based value of the attribute.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// The name of the attribute.
        /// </summary>
        /// <reamrks>
        /// This name must be in the form "namespacePrefix:localName".
        /// </reamrks>
        string Name { get; }

        /// <summary>
        /// The namespace URI of the attribute.
        /// </summary>
        string NamespaceURI { get; }

        /// <summary>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned, in which case TypedValue
        /// returns the same string as what the Value property returns.
        /// </summary>
        /// <param name="containingFragment">
        /// The fragment containing the attributes.
        /// </param>
        /// <returns>
        /// The value of the attribute typed to the data type specified in
        /// the schema definition for the attribute. If no data type is available,
        /// then a string representation is returned.
        /// </returns>
        object GetTypedValue(XbrlFragment containingFragment);

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
        bool TypedValueEquals(IAttribute otherAttribute, XbrlFragment containingFragment);
    }
}
