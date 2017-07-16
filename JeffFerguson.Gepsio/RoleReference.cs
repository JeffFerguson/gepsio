using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a role reference.
    /// </summary>
    /// <remarks>
    /// Role references are denoted in XBRL instances with the roleRef element.
    /// </remarks>
    public class RoleReference : XlinkNode
    {
        /// <summary>
        /// The URI for this role reference.
        /// </summary>
        public Uri Uri { get; private set; }

        internal RoleReference(INode roleRefNode) : base(roleRefNode)
        {
            this.Uri = new Uri(roleRefNode.GetAttributeValue("roleURI"));
        }
    }
}
