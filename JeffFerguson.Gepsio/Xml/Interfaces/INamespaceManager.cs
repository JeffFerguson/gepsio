
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
    public interface INamespaceManager
    {
        /// <summary>
        /// The XML document associated with this namespace manager.
        /// </summary>
        IDocument Document { get; set; }

        /// <summary>
        /// Adds a namespace to the namespace manager.
        /// </summary>
        /// <param name="prefix">
        /// The prefix of the namespace to be added.
        /// </param>
        /// <param name="uri">
        /// The URI of the namespace to be added.
        /// </param>
        void AddNamespace(string prefix, string uri);

        /// <summary>
        /// Looks up the prefix for a given URI.
        /// </summary>
        /// <param name="uri">
        /// The URI of the namespace whose prefix should be returned.
        /// </param>
        /// <returns>
        /// The prefix of the namespace with the given URI.
        /// </returns>
        string LookupPrefix(string uri);

        /// <summary>
        /// Looks up the URI for a given prefix.
        /// </summary>
        /// <param name="prefix">
        /// The prefix of the namespace whose URI should be returned.
        /// </param>
        /// <returns>
        /// The URI of the namespace with the given prefix.
        /// </returns>
        string LookupNamespace(string prefix);
    }
}
