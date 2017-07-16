using System.Collections;

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
    public interface INodeList : IEnumerable
    {
        /// <summary>
        /// The number of nodes in the node list.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// An indexer for the nosde list.
        /// </summary>
        /// <param name="i">
        /// The zero-based index of the node to be returned.
        /// </param>
        /// <returns>
        /// The node at the given position in the list.
        /// </returns>
        INode this[int i] { get; }

        /// <summary>
        /// Compare the nodes in the node list with the nodes in another
        /// node list, checking for structure-equals semantics.
        /// </summary>
        /// <remarks>
        /// Structure-equals is defined by the XBRL Specification as XML nodes
        /// that are either equal in the XML value space, or whose XBRL-relevant
        /// sub-elements and attributes are structure-equal.
        /// </remarks>
        /// <param name="OtherNodeList">
        /// A list of nodes to be compared with the nodes in the
        /// current list.
        /// </param>
        /// <returns>
        /// True if the nodes in the node list are all
        /// structure-equals to all of the nodes in another node
        /// list; false otherwise.
        /// </returns>
        bool StructureEquals(INodeList OtherNodeList);
    }
}
