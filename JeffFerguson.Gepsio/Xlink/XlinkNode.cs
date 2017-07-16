using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xlink
{
    /// <summary>
    /// An implementation for an XML node that supports XLink.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Xlink is dependent upon XML, and, for Gepsio, XLink will use the XML interfaces.
    /// The XLinkNode class itself does not need to be interface based, because its implementation
    /// will not need to change between platforms. It will simply leverage the lower-level
    /// XML service layer, which is interface based.
    /// </para>
    /// <para>
    /// This class implements the specification available at http://www.w3.org/TR/xlink11/.
    /// </para>
    /// </remarks>
    public class XlinkNode
    {
        internal static string xlinkNamespace = "http://www.w3.org/1999/xlink";

        /// <summary>
        /// An enumeration of possible types for an Xlink node.
        /// </summary>
        public enum XlinkType
        {
            /// <summary>
            /// An unknown link type.
            /// </summary>
            Unknown,
            /// <summary>
            /// The "simple" link type.
            /// </summary>
            Simple,
            /// <summary>
            /// The "extended" link type.
            /// </summary>
            Extended,
            /// <summary>
            /// The "locator" link type.
            /// </summary>
            Locator,
            /// <summary>
            /// The "arc" link type.
            /// </summary>
            Arc,
            /// <summary>
            /// The "resource" link type.
            /// </summary>
            Resource,
            /// <summary>
            /// The "title" link type.
            /// </summary>
            Title
        }

        /// <summary>
        /// The type of Xlink node.
        /// </summary>
        public XlinkType Type { get; private set; }

        /// <summary>
        /// The value of the node's "type" attribute.
        /// </summary>
        public string TypeAttributeValue { get; private set; }

        /// <summary>
        /// The value of the node's "href" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Href { get; private set; }

        /// <summary>
        /// The value of the node's "role" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Role { get; private set; }

        /// <summary>
        /// The value of the node's "arcrole" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string ArcRole { get; private set; }

        /// <summary>
        /// The value of the node's "title" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The value of the node's "show" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Show { get; private set; }

        /// <summary>
        /// The value of the node's "actuate" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Actuate { get; private set; }

        /// <summary>
        /// The value of the node's "label" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// The value of the node's "from" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string From { get; private set; }

        /// <summary>
        /// The value of the node's "to" attribute. This value is the empty string
        /// if the attribute was not found on the node.
        /// </summary>
        public string To { get; private set; }

        internal static bool IsXlinkNode(INode xlinkNodeCandidate)
        {
            var typeAttributeValue = xlinkNodeCandidate.GetAttributeValue(xlinkNamespace, "type");
            var hrefAttributeValue = xlinkNodeCandidate.GetAttributeValue(xlinkNamespace, "href");
            if ((string.IsNullOrEmpty(typeAttributeValue) == true) && (string.IsNullOrEmpty(hrefAttributeValue) == true))
                return false;
            return true;
        }

        internal XlinkNode(INode xmlNode)
        {
            this.TypeAttributeValue = xmlNode.GetAttributeValue(xlinkNamespace, "type");
            SetType();
            this.Href = xmlNode.GetAttributeValue(xlinkNamespace, "href");
            this.Role = xmlNode.GetAttributeValue(xlinkNamespace, "role");
            this.ArcRole = xmlNode.GetAttributeValue(xlinkNamespace, "arcrole");
            this.Title = xmlNode.GetAttributeValue(xlinkNamespace, "title");
            this.Show = xmlNode.GetAttributeValue(xlinkNamespace, "show");
            this.Actuate = xmlNode.GetAttributeValue(xlinkNamespace, "actuate");
            this.Label = xmlNode.GetAttributeValue(xlinkNamespace, "label");
            this.From = xmlNode.GetAttributeValue(xlinkNamespace, "from");
            this.To = xmlNode.GetAttributeValue(xlinkNamespace, "to");
        }

        internal bool IsInRole(string roleUri)
        {
            return this.Role.Equals(roleUri);
        }

        private void SetType()
        {
            this.Type = XlinkType.Unknown;


            // According to section 4 of the XLink specification, if an element has an xlink:href attribute but does
            // not have an xlink:type attribute, then it is treated exactly as if it had an xlink:type attribute with
            // the value "simple".

            if ((string.IsNullOrEmpty(this.TypeAttributeValue) == true) && (string.IsNullOrEmpty(this.Href) == false))
            {
                this.Type = XlinkType.Simple;
                return;
            }

            // At this point, we know that the node has a "type" attribute.

            if (this.TypeAttributeValue.Equals("simple") == true)
            {
                this.Type = XlinkType.Simple;
                return;
            }
            if (this.TypeAttributeValue.Equals("extended") == true)
            {
                this.Type = XlinkType.Extended;
                return;
            }
            if (this.TypeAttributeValue.Equals("locator") == true)
            {
                this.Type = XlinkType.Locator;
                return;
            }
            if (this.TypeAttributeValue.Equals("arc") == true)
            {
                this.Type = XlinkType.Arc;
                return;
            }
            if (this.TypeAttributeValue.Equals("resource") == true)
            {
                this.Type = XlinkType.Resource;
                return;
            }
            if (this.TypeAttributeValue.Equals("title") == true)
            {
                this.Type = XlinkType.Title;
                return;
            }
        }
    }
}
