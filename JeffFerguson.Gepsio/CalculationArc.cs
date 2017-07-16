using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "calculationArc" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// The CalculationArc manages information stored in a calculation arc. Calculation arcs are found in
    /// calculation linkbase documents. Calculation arcs take the following form:
    /// </para>
    /// <para>
    /// <code>
    /// &lt;calculationArc
    /// xlink:type="arc"
    /// xlink:arcrole="http://www.xbrl.org/2003/arcrole/summation-item"
    /// xlink:from="FromLocatorLabel"
    /// xlink:to="ToLocatorLabel"
    /// order="[OrderValue]"
    /// weight="[WeightValue]"
    /// use="[UseValue]"
    /// /&gt;
    /// </code>
    /// </para>
    /// <para>
    /// It is important to note that the "to" label in a calculation arc may reference more than one locator. The 397.00 test in the XBRL-CONF-CR3-2007-03-05
    /// conformance suite uses the following calculation link in 397-ABC-calculation.xml:
    /// </para>
    /// <para>
    /// <code>
    /// &lt;calculationLink xlink:type="extended" xlink:role="http://www.xbrl.org/2003/role/link"&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#A" xlink:label="summationItem" /&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#B" xlink:label="contributingItem" /&gt;
    ///     &lt;loc xlink:type="locator" xlink:href="397-ABC.xsd#C" xlink:label="contributingItem" /&gt;
    ///     &lt;!-- A = B + C --&gt;
    ///     &lt;calculationArc xlink:type="arc" xlink:arcrole="http://www.xbrl.org/2003/arcrole/summation-item" xlink:from="summationItem" xlink:to="contributingItem" weight="1"/&gt;
    /// &lt;/calculationLink&gt;
    /// </code>
    /// </para>
    /// <para>
    /// Note that the calculation arc goes from a label of "summationItem" to a label of "contributingItem". Note also that there are two locators that match
    /// the "contributingItem" label: the locator with an href of "397-ABC.xsd#B" and a label with an href of "397-ABC.xsd#C". Both locators must be used for
    /// any calculations performed in satisfcation of the calculation arc.
    /// </para>
    /// </remarks>
    public class CalculationArc
    {
        /// <summary>
        /// The ID of the "from" label referenced in the calculation arc.
        /// </summary>
        public string FromId { get; private set; }

        /// <summary>
        /// The ID of the "to" label referenced in the calculation arc.
        /// </summary>
        public string ToId { get; private set; }

        /// <summary>
        /// The locator referenced by the "from" label referenced in the calculation arc.
        /// </summary>
        public Locator FromLocator { get; set; }

        /// <summary>
        /// A collection of locators referenced by the "to" label referenced in the calculation arc.
        /// </summary>
        public List<Locator> ToLocators { get; private set; }

        /// <summary>
        /// The value of the "order" attribute used in the calculation arc.
        /// </summary>
        public decimal Order { get; private set; }

        /// <summary>
        /// The value of the "weight" attribute used in the calculation arc.
        /// </summary>
        public decimal Weight { get; private set; }

        /// <summary>
        /// The constructor for the CalculationArc class.
        /// </summary>
        /// <param name="CalculationArcNode">
        /// The XML node for the calculation arc.
        /// </param>
        internal CalculationArc(INode CalculationArcNode)
        {
            this.ToLocators = new List<Locator>();
            this.FromId = CalculationArcNode.GetAttributeValue(XlinkNode.xlinkNamespace, "from");
            this.ToId = CalculationArcNode.GetAttributeValue(XlinkNode.xlinkNamespace, "to");
            string OrderString = CalculationArcNode.GetAttributeValue("order");
            if(string.IsNullOrEmpty(OrderString) == false)
                this.Order = Convert.ToDecimal(OrderString);
            string WeightString = CalculationArcNode.GetAttributeValue("weight");
            if (string.IsNullOrEmpty(WeightString) == false)
                this.Weight = Convert.ToDecimal(WeightString);
            else
                this.Weight = (decimal)(1.0);
        }

        /// <summary>
        /// Adds a new locator to the arc's collection of "To" locators.
        /// </summary>
        /// <param name="ToLocator">
        /// The locator to be added to the "to" locator collection.
        /// </param>
        internal void AddToLocator(Locator ToLocator)
        {
            this.ToLocators.Add(ToLocator);
        }
    }
}
