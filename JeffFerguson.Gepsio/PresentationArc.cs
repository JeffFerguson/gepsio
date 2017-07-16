using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "presentationArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    public class PresentationArc : XlinkNode
    {
        /// <summary>
        /// The order of the presentation arc amongst all of the arcs in the same presentation link.
        /// </summary>
        /// <remarks>
        /// This value is populated from the value of the arc's "order" attribute.
        /// </remarks>
        public double Order { get; private set; }

        /// <summary>
        /// The URI of the preferred label for this presentation arc.
        /// </summary>
        public Uri PreferredLabelUri { get; private set; }

        internal PresentationArc(INode presentationArcNode) : base(presentationArcNode)
        {
            var orderAsString = presentationArcNode.GetAttributeValue("order");
            double orderParsedValue;
            if (double.TryParse(orderAsString, out orderParsedValue) == true)
                Order = orderParsedValue;
            var preferredLabelUriAsString = presentationArcNode.GetAttributeValue("preferredLabel");
            if (string.IsNullOrEmpty(preferredLabelUriAsString) == false)
                PreferredLabelUri = new Uri(preferredLabelUriAsString);
        }
    }
}
