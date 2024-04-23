using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a Presentation linkbase document.
    /// </summary>
    public class PresentationLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="PresentationLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<PresentationLink> PresentationLinks { get; private set; }

        /// <summary>
        /// Construct a Presentation linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the Presentation linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the Presentation linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase Presentation reference.
        /// </param>
        internal PresentationLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            Initialize();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("presentationLink") == true)
                {
                    AddPresentationLink(CurrentChild);
                }
            }
        }

        /// <summary>
        /// Construct a Presentation linkbase document from a Presentation link node.
        /// </summary>
        /// <param name="PresentationLink">
        /// An XML node for a Presentation link.
        /// </param>
        internal PresentationLinkbaseDocument(INode PresentationLink)
        {
            Initialize();
            AddPresentationLink(PresentationLink);
        }

        /// <summary>
        /// Initializes the Presentation linkbase object.
        /// </summary>
        private void Initialize()
        {
            PresentationLinks = new List<PresentationLink>();
        }

        /// <summary>
        /// Add a Presentation link node.
        /// </summary>
        /// <param name="PresentationLink">
        /// An XML node for a Presentation link.
        /// </param>
        private void AddPresentationLink(INode PresentationLink)
        {
            this.PresentationLinks.Add(new PresentationLink(PresentationLink));
        }
    }
}
