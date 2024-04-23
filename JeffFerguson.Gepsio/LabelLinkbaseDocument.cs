using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a Label linkbase document.
    /// </summary>
    public class LabelLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="LabelLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<LabelLink> LabelLinks { get; private set; }

        /// <summary>
        /// Construct a Label linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the Label linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the Label linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase Label reference.
        /// </param>
        internal LabelLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            Initialize();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("labelLink") == true)
                {
                    AddLabelLink(CurrentChild);
                }
            }
        }

        /// <summary>
        /// Construct a Label linkbase document from a Label link node.
        /// </summary>
        /// <param name="LabelLink">
        /// An XML node for a Label link.
        /// </param>
        internal LabelLinkbaseDocument(INode LabelLink)
        {
            Initialize();
            AddLabelLink(LabelLink);
        }

        /// <summary>
        /// Initializes the Label linkbase object.
        /// </summary>
        private void Initialize()
        {
            LabelLinks = new List<LabelLink>();
        }

        /// <summary>
        /// Add a Label link node.
        /// </summary>
        /// <param name="LabelLink">
        /// An XML node for a Label link.
        /// </param>
        private void AddLabelLink(INode LabelLink)
        {
            this.LabelLinks.Add(new LabelLink(LabelLink));
        }
    }
}
