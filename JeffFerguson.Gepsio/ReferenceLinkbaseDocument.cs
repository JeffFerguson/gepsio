using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a reference linkbase document.
    /// </summary>
    public class ReferenceLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="ReferenceLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<ReferenceLink> ReferenceLinks { get; private set; }

        internal ReferenceLinkbaseDocument(string ContainingDocumentUri, string DocumentPath)
            : base(ContainingDocumentUri, DocumentPath)
        {
            ReferenceLinks = new List<ReferenceLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("referenceLink") == true)
                    this.ReferenceLinks.Add(new ReferenceLink(CurrentChild));
            }
        }
    }
}
