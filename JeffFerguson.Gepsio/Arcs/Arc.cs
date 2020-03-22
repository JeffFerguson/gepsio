using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <param name="containingFragment">
        /// The XBRL fragment containing the arcs being compared.
        /// </param>
        /// <returns>
        /// True if the arcs are equivalent; false if the arcs are not equivalent.
        /// </returns>
        internal bool EquivalentTo(Arc otherArc, XbrlFragment containingFragment)
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
                if(AttributeValuesEquivalent(thisArcAttribute, matchingAttribute, containingFragment) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compare the values of two attributes using the type information found in its schema definition.
        /// </summary>
        /// <param name="attributeOne">
        /// The first attribute to compare.
        /// </param>
        /// <param name="attributeTwo">
        /// The second attribute to compare.
        /// </param>
        /// <param name="containingFragment">
        /// The fragment containing the attributes.
        /// </param>
        /// <returns>
        /// True if the attribute have the same value; false otherwise.
        /// </returns>
        private bool AttributeValuesEquivalent(IAttribute attributeOne, IAttribute attributeTwo, XbrlFragment containingFragment)
        {
            var attributeOneType = containingFragment.Schemas.GetAttributeType(attributeOne);
            var attributeTwoType = containingFragment.Schemas.GetAttributeType(attributeTwo);
            if ((attributeOneType == null) || (attributeTwoType == null))
            {
                return attributeOne.Value.Equals(attributeTwo.Value);
            }
            if(attributeOneType.GetType() != attributeTwoType.GetType())
            {
                return attributeOne.Value.Equals(attributeTwo.Value);
            }
            if(attributeOneType is Xsd.String)
            {
                return attributeOne.Value.Equals(attributeTwo.Value);
            }
            if(attributeOneType is Xsd.Decimal)
            {
                var attributeOneValueAsDecimal = Convert.ToDecimal(attributeOne.Value, CultureInfo.InvariantCulture);
                var attributeTwoValueAsDecimal = Convert.ToDecimal(attributeTwo.Value, CultureInfo.InvariantCulture);
                return attributeOneValueAsDecimal == attributeTwoValueAsDecimal;
            }
            if (attributeOneType is Xsd.Double)
            {
                var attributeOneValueAsDouble = Convert.ToDouble(attributeOne.Value, CultureInfo.InvariantCulture);
                var attributeTwoValueAsDouble = Convert.ToDouble(attributeTwo.Value, CultureInfo.InvariantCulture);
                return attributeOneValueAsDouble == attributeTwoValueAsDouble;
            }
            if (attributeOneType is Xsd.Boolean)
            {
                // The explicit checks for "1" and "0" are in place to satisfy conformance test
                // 331-equivalentRelationships-instance-13.xml. Convert.ToBoolean() does not convert these values
                //to Booleans.
                bool attributeOneValueAsBoolean;
                if (attributeOne.Value.Equals("1") == true)
                    attributeOneValueAsBoolean = true;
                else if (attributeOne.Value.Equals("0") == true)
                    attributeOneValueAsBoolean = false;
                else
                    attributeOneValueAsBoolean = Convert.ToBoolean(attributeOne.Value, CultureInfo.InvariantCulture);
                bool attributeTwoValueAsBoolean;
                if (attributeTwo.Value.Equals("1") == true)
                    attributeTwoValueAsBoolean = true;
                else if (attributeTwo.Value.Equals("0") == true)
                    attributeTwoValueAsBoolean = false;
                else
                    attributeTwoValueAsBoolean = Convert.ToBoolean(attributeTwo.Value, CultureInfo.InvariantCulture);
                return attributeOneValueAsBoolean == attributeTwoValueAsBoolean;
            }
            return false;
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
