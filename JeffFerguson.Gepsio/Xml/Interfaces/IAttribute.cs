
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
        /// The value of the attribute.
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
    }
}
