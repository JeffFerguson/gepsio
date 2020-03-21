using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A base class for all arcs.
    /// </summary>
    public class Arc : XlinkNode
    {
        private INode node;

        internal Arc(INode arcNode) : base(arcNode)
        {
            node = arcNode;
        }

        /// <summary>
        /// Checks to see if the current arc is equivalent to a supplied arc.
        /// </summary>
        /// <remarks>
        /// Equivalency is determined using the rules in section 3.5.3.9.7.4 of the XBRL Specification.
        /// </remarks>
        /// <param name="otherArc">
        /// The other arc to compare to this arc.
        /// </param>
        /// <returns>
        /// True if the arcs are equivalent; false if the arcs are not equivalent.
        /// </returns>
        internal bool EquivalentTo(Arc otherArc)
        {
            var nonExemptAttributesForThisArc = GetNonExemptAttributes(this);
            var nonExemptAttributesForOtherArc = GetNonExemptAttributes(otherArc);
            if(nonExemptAttributesForThisArc.Count != nonExemptAttributesForOtherArc.Count)
            {
                return false;
            }
            foreach(var thisArcAttribute in nonExemptAttributesForThisArc)
            {
                var matchingAttribute = nonExemptAttributesForOtherArc.FirstOrDefault(a => a.LocalName.Equals(thisArcAttribute.LocalName));
                if(matchingAttribute == null)
                {
                    return false;
                }
                if(thisArcAttribute.NamespaceURI.Equals(matchingAttribute.NamespaceURI) == false)
                {
                    return false;
                }
                if(thisArcAttribute.Value.Equals(matchingAttribute.Value) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get a list of non-exempt attributes for an arc.
        /// </summary>
        /// <param name="arc">
        /// The arc whose non-exempt attributes should be returned.
        /// </param>
        /// <returns>
        /// A list of non-exempt attributes.
        /// </returns>
        private List<IAttribute> GetNonExemptAttributes(Arc arc)
        {
            var uriXmlNamespaceUri2000 = new Uri(XbrlDocument.XmlNamespaceUri2000);
            var urixlinkNamespace = new Uri(XlinkNode.xlinkNamespace);

            var listToReturn = new List<IAttribute>();
            foreach(IAttribute currentAttribute in arc.node.Attributes)
            {
                var currentAttributeIsExempt = false;

                if (string.IsNullOrEmpty(currentAttribute.NamespaceURI) == false)
                {
                    var currentAttributeNamespaceUri = new Uri(currentAttribute.NamespaceURI);
                    if (currentAttributeNamespaceUri == uriXmlNamespaceUri2000)
                    {
                        currentAttributeIsExempt = true;
                    }
                    if (currentAttributeNamespaceUri == urixlinkNamespace)
                    {
                        currentAttributeIsExempt = true;
                    }
                }
                if(currentAttribute.LocalName.Equals("use") == true)
                {
                    currentAttributeIsExempt = true;
                }
                if (currentAttribute.LocalName.Equals("priority") == true)
                {
                    currentAttributeIsExempt = true;
                }
                if (currentAttributeIsExempt == false)
                {
                    listToReturn.Add(currentAttribute);
                }
            }
            return listToReturn;
        }
    }
}
