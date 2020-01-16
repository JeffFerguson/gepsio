using System.IO;
using System.Threading.Tasks;

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
    public interface IDocument
    {
        /// <summary>
        /// Loads an XBRL document synchronously.
        /// </summary>
        /// <param name="path">
        /// The path to the document stored on disk.
        /// </param>
        void Load(string path);

        /// <summary>
        /// Loads an XBRL document synchronously.
        /// </summary>
        /// <param name="stream">
        /// A stream containing the XBRL data to be loaded.
        /// </param>
        void Load(Stream stream);

#if NETSTANDARD2_1
        /// <summary>
        /// Loads an XBRL document asynchronously.
        /// </summary>
        /// <param name="path">
        /// The path to the document stored on disk.
        /// </param>
        Task LoadAsync(string path);

        /// <summary>
        /// Loads an XBRL document asynchronously.
        /// </summary>
        /// <param name="stream">
        /// A stream containing the XBRL data to be loaded.
        /// </param>
        Task LoadAsync(Stream stream);
#endif
        /// <summary>
        /// Select a set of nodes from the document.
        /// </summary>
        /// <param name="xpath">
        /// The XPATH specification of the nodes to be found.
        /// </param>
        /// <param name="namespaceManager">
        /// The namespace manager to be used to resolve namespace
        /// information found in the given XPATH specification.
        /// </param>
        /// <returns>
        /// A list of nodes in the document matching the given XPATH specification.
        /// </returns>
        INodeList SelectNodes(string xpath, INamespaceManager namespaceManager);

        /// <summary>
        /// Select a node from the document.
        /// </summary>
        /// <param name="xPath">
        /// The XPATH specification of the nodes to be found.
        /// </param>
        /// <param name="namespaceManager">
        /// The namespace manager to be used to resolve namespace
        /// information found in the given XPATH specification.
        /// </param>
        /// <returns>
        /// A node in the document matching the given XPATH specification.
        /// </returns>
        INode SelectSingleNode(string xPath, INamespaceManager namespaceManager);
    }
}
