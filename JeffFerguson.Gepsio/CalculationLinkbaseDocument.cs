using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a calculation linkbase document.
    /// </summary>
    public class CalculationLinkbaseDocument : LinkbaseDocument
    {
        /// <summary>
        /// A collection of <see cref="CalculationLink"/> objects defined by the linkbase document.
        /// </summary>
        public List<CalculationLink> CalculationLinks { get; private set; }

        /// <summary>
        /// Construct a calculation linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the calculation linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the calculation linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase calculation reference.
        /// </param>
        internal CalculationLinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
            : base(ContainingDocumentUri, DocumentPath, containingFragment)
        {
            Initialize();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("calculationLink") == true)
                {
                    AddCalculationLink(CurrentChild);
                }
            }
        }

        /// <summary>
        /// Construct a calculation linkbase document from a calculation link node.
        /// </summary>
        /// <param name="calculationLink">
        /// An XML node for a calculation link.
        /// </param>
        internal CalculationLinkbaseDocument(INode calculationLink)
        {
            Initialize();
            AddCalculationLink(calculationLink);
        }

        /// <summary>
        /// Initializes the calculation linkbase object.
        /// </summary>
        private void Initialize()
        {
            CalculationLinks = new List<CalculationLink>();
        }

        /// <summary>
        /// Add a calculation link node.
        /// </summary>
        /// <param name="calculationLink">
        /// An XML node for a calculation link.
        /// </param>
        private void AddCalculationLink(INode calculationLink)
        {
            this.CalculationLinks.Add(new CalculationLink(this, calculationLink));
        }

        /// <summary>
        /// Finds the <see cref="CalculationLink"/> object having the given role.
        /// </summary>
        /// <param name="CalculationLinkRole">
        /// The role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="CalculationLink"/> object having the given role, or
        /// null if no object can be found.
        /// </returns>
        public CalculationLink GetCalculationLink(RoleType CalculationLinkRole)
        {
            foreach (var currentCalculationLink in CalculationLinks)
            {
                if (currentCalculationLink.RoleUri.Equals(CalculationLinkRole.RoleUri) == true)
                    return currentCalculationLink;
            }
            return null;
        }

        /// <summary>
        /// Gets the calculation arc whose "to" reference matches the supplied locator.
        /// </summary>
        /// <param name="toLocator">
        /// The "to" locator to match.
        /// </param>
        /// <returns>
        /// The calculation arc whose "to" reference matches the supplied locator.
        /// If there is no match, then null is returned.
        /// </returns>
        public CalculationArc GetCalculationArc(Locator toLocator)
        {
            foreach (var currentCalculationLink in this.CalculationLinks)
            {
                var matchingArc = currentCalculationLink.GetCalculationArc(toLocator);
                if (matchingArc != null)
                {
                    return matchingArc;
                }
            }
            return null;
        }
    }
}
