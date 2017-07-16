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

        internal CalculationLinkbaseDocument(XbrlSchema ContainingXbrlSchema, string DocumentPath)
            : base(ContainingXbrlSchema, DocumentPath)
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
    }
}
