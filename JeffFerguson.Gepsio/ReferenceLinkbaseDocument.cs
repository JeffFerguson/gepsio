using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a Reference linkbase document.
    /// </summary>
    public class ReferenceLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="ReferenceLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<ReferenceLink> ReferenceLinks { get; private set; }

        /// <summary>
        /// Construct a Reference linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the Reference linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the Reference linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase Reference reference.
        /// </param>
        internal ReferenceLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            Initialize();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("referenceLink") == true)
                {
                    AddReferenceLink(CurrentChild);
                }
            }
        }

        /// <summary>
        /// Construct a Reference linkbase document from a Reference link node.
        /// </summary>
        /// <param name="ReferenceLink">
        /// An XML node for a Reference link.
        /// </param>
        internal ReferenceLinkbaseDocument(INode ReferenceLink)
        {
            Initialize();
            AddReferenceLink(ReferenceLink);
        }

        /// <summary>
        /// Initializes the Reference linkbase object.
        /// </summary>
        private void Initialize()
        {
            ReferenceLinks = new List<ReferenceLink>();
        }

        /// <summary>
        /// Add a Reference link node.
        /// </summary>
        /// <param name="ReferenceLink">
        /// An XML node for a Reference link.
        /// </param>
        private void AddReferenceLink(INode ReferenceLink)
        {
            this.ReferenceLinks.Add(new ReferenceLink(ReferenceLink));
        }
    }
}
