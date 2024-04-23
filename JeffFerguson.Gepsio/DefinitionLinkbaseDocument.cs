using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a definition linkbase document.
    /// </summary>
    public class DefinitionLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="DefinitionLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<DefinitionLink> DefinitionLinks { get; private set; }

        /// <summary>
        /// Construct a definition linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the definition linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the definition linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase definition reference.
        /// </param>
        internal DefinitionLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            Initialize();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("definitionLink") == true)
                {
                    AddDefinitionLink(CurrentChild);
                }
            }
        }

        /// <summary>
        /// Construct a definition linkbase document from a definition link node.
        /// </summary>
        /// <param name="definitionLink">
        /// An XML node for a definition link.
        /// </param>
        internal DefinitionLinkbaseDocument(INode definitionLink)
        {
            Initialize();
            AddDefinitionLink(definitionLink);
        }

        /// <summary>
        /// Initializes the definition linkbase object.
        /// </summary>
        private void Initialize()
        {
            DefinitionLinks = new List<DefinitionLink>();
        }

        /// <summary>
        /// Add a definition link node.
        /// </summary>
        /// <param name="definitionLink">
        /// An XML node for a definition link.
        /// </param>
        private void AddDefinitionLink(INode definitionLink)
        {
            this.DefinitionLinks.Add(new DefinitionLink(definitionLink));
        }
    }
}
