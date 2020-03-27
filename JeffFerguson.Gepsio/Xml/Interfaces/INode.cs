using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public interface INode
    {
        /// <summary>
        /// The list of attributes used on the node.
        /// </summary>
        IAttributeList Attributes { get; }

        /// <summary>
        /// The list of child nodes for this node.
        /// </summary>
        INodeList ChildNodes { get; }

        /// <summary>
        /// The namespace URI of this node.
        /// </summary>
        string NamespaceURI { get; }

        /// <summary>
        /// The base URI of this node.
        /// </summary>
        string BaseURI { get; }

        /// <summary>
        /// The inner text of this node.
        /// </summary>
        string InnerText { get; }

        /// <summary>
        /// The name of this node.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The local name of this node.
        /// </summary>
        string LocalName { get; }

        /// <summary>
        /// True if this node is an XML comment; false otherwise.
        /// </summary>
        bool IsComment { get; }

        /// <summary>
        /// The parent node of this node.
        /// </summary>
        /// <remarks>
        /// This value will be null if the node has no parent.
        /// </remarks>
        INode ParentNode { get; }

        /// <summary>
        /// The prefix of this node.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// The first child of this node.
        /// </summary>
        /// <remarks>
        /// This value will be null if the node has no child.
        /// </remarks>
        INode FirstChild { get; }

        /// <summary>
        /// The value of this node.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Select a set of child nodes from the current nodes.
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
        /// Gets an attribute value from an attribute on the node.
        /// </summary>
        /// <param name="AttributeName">
        /// The name of the attribute whose value should be returned.
        /// </param>
        /// <returns>
        /// The value of the attribute with the supplied name. The emprty string will be
        /// retrurned if no attribute is available with the given name.
        /// </returns>
        string GetAttributeValue(string AttributeName);

        /// <summary>
        /// Gets an attribute value from an attribute on the node.
        /// </summary>
        /// <param name="AttributeNamespaceUri">
        /// The URI of the attribute whose value should be returned.
        /// </param>
        /// <param name="AttributeLocalName">
        /// The local name of the attribute whose value should be returned.
        /// </param>
        /// <returns>
        /// The value of the attribute with the supplied name. The emprty string will be
        /// retrurned if no attribute is available with the given name.
        /// </returns>
        string GetAttributeValue(string AttributeNamespaceUri, string AttributeLocalName);

        /// <summary>
        /// Compare the node with another node, checking for structure-equals semantics.
        /// </summary>
        /// <remarks>
        /// Structure-equals is defined by the XBRL Specification as XML nodes
        /// that are either equal in the XML value space, or whose XBRL-relevant
        /// sub-elements and attributes are structure-equal.
        /// </remarks>
        /// <param name="OtherNode">
        /// A node to be compared with the current node.
        /// </param>
        /// <param name="containingFragment">
        /// The fragment containing the node.
        /// </param>
        /// <returns>
        /// True if the node is structure-equals to another node; false otherwise.
        /// </returns>
        bool StructureEquals(INode OtherNode, XbrlFragment containingFragment);

        /// <summary>
        /// Compare the node with another node, checking for parent-equals semantics.
        /// </summary>
        /// <remarks>
        /// Parent-equals is defined by the XBRL Specification as XML nodes
        /// having the same parent.
        /// </remarks>
        /// <param name="OtherNode">
        /// A node to be compared with the current node.
        /// </param>
        /// <returns>
        /// True if the node is parent-equals to another node; false otherwise.
        /// </returns>
        bool ParentEquals(INode OtherNode);
    }
}
