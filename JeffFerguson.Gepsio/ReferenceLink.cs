using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XML element "referenceLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class ReferenceLink :XlinkNode
    {
        /// <summary>
        /// A list of locators used in this reference link.
        /// </summary>
        public List<Locator> Locators { get; private set; }

        /// <summary>
        /// A list of reference arcs used in this reference link.
        /// </summary>
        public List<ReferenceArc> ReferenceArcs { get; private set; }

        /// <summary>
        /// A list of references used in this reference link.
        /// </summary>
        public List<Reference> References { get; private set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal ReferenceLink(INode ReferenceLinkNode) : base(ReferenceLinkNode)
        {
            Locators = new List<Locator>
            {
                Capacity = ReferenceLinkNode.ChildNodes.Count
            };
            ReferenceArcs = new List<ReferenceArc>();
            References = new List<Reference>();
            ReferenceArcs.Capacity = ReferenceLinkNode.ChildNodes.Count;
            foreach (INode CurrentChild in ReferenceLinkNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("loc") == true)
                    Locators.Add(new Locator(CurrentChild));
                else if (CurrentChild.LocalName.Equals("referenceArc") == true)
                    ReferenceArcs.Add(new ReferenceArc(CurrentChild));
                else if (CurrentChild.LocalName.Equals("reference") == true)
                    References.Add(new Reference(CurrentChild));
            }
            Locators.TrimExcess();
            ReferenceArcs.TrimExcess();
        }
    }
}
