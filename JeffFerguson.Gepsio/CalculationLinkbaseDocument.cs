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

        internal CalculationLinkbaseDocument(string ContainingDocumentUri, string DocumentPath)
            : base(ContainingDocumentUri, DocumentPath)
        {
            CalculationLinks = new List<CalculationLink>();
            foreach (INode CurrentChild in thisLinkbaseNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("calculationLink") == true)
                    this.CalculationLinks.Add(new CalculationLink(this, CurrentChild));
            }
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
