using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "presentationLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class PresentationLink : XlinkNode
    {
        /// <summary>
        /// A list of locators used in this presentation link.
        /// </summary>
        public List<Locator> Locators { get; private set; }

        /// <summary>
        /// A list of presentation arcs used in this presentation link.
        /// </summary>
        /// <remarks>
        /// The arcs in this collection will not necessarily appear in the order in which they appear
        /// in the linkbase document. Instead, the will appear sorted in ascending order by the
        /// value of the arc's Order property.
        /// </remarks>
        public List<PresentationArc> PresentationArcs { get; private set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal PresentationLink(INode PresentationLinkNode) : base(PresentationLinkNode)
        {
            Locators = new List<Locator>();
            Locators.Capacity = PresentationLinkNode.ChildNodes.Count;
            PresentationArcs = new List<PresentationArc>();
            PresentationArcs.Capacity = PresentationLinkNode.ChildNodes.Count;
            foreach (INode CurrentChild in PresentationLinkNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("loc") == true)
                    Locators.Add(new Locator(CurrentChild));
                else if (CurrentChild.LocalName.Equals("presentationArc") == true)
                    PresentationArcs.Add(new PresentationArc(CurrentChild));
            }
            SortPresentationArcsInAscendingOrder();
            Locators.TrimExcess();
            PresentationArcs.TrimExcess();
        }

        //------------------------------------------------------------------------------------
        // Sorts the presentation arcs in ascending order by their Order value.
        //------------------------------------------------------------------------------------
        private void SortPresentationArcsInAscendingOrder()
        {
            PresentationArcs.Sort(
                delegate(PresentationArc firstArc, PresentationArc secondArc)
                {
                    if (firstArc.Order == secondArc.Order)
                        return 0;
                    if (firstArc.Order < secondArc.Order)
                        return -1;
                    return 1;
                }
                );
        }
    }
}
