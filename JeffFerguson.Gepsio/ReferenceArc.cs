using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XML element "referenceArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class ReferenceArc : XlinkNode
    {
        internal ReferenceArc(INode referenceArcNode) : base(referenceArcNode)
        {
            foreach(var CurrentChild in referenceArcNode.ChildNodes)
            {
            }
        }
    }
}
