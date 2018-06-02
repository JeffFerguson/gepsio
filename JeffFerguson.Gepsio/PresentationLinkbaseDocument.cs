using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a presentation linkbase document.
    /// </summary>
    public class PresentationLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="PresentationLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<PresentationLink> PresentationLinks { get; private set; }

        internal PresentationLinkbaseDocument(string ContainingDocumentUri, string DocumentPath)
            : base(ContainingDocumentUri, DocumentPath)
        {
            PresentationLinks = new List<PresentationLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("presentationLink") == true)
                    this.PresentationLinks.Add(new PresentationLink(CurrentChild));
            }
        }
    }
}
