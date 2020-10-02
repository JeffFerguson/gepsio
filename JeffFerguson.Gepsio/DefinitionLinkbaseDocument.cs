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

        internal DefinitionLinkbaseDocument(string ContainingDocumentUri, string DocumentPath)
            : base(ContainingDocumentUri, DocumentPath)
        {
            DefinitionLinks = new List<DefinitionLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("definitionLink") == true)
                    this.DefinitionLinks.Add(new DefinitionLink(CurrentChild));
            }
        }
    }
}
