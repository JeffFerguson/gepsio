using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a label linkbase document.
    /// </summary>
    public class LabelLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="LabelLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<LabelLink> LabelLinks { get; private set; }

        internal LabelLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
        {
            LabelLinks = new List<LabelLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("labelLink") == true)
                    this.LabelLinks.Add(new LabelLink(CurrentChild));
            }
        }
    }
}
