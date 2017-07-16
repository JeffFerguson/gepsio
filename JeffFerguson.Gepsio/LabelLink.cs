using System.Collections.Generic;
using System.Linq;
using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xlink;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "labelLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class LabelLink : XlinkNode
    {
        /// <summary>
        /// A collection of <see cref="Locator"/> objects for the label link.
        /// </summary>
        public List<Locator> Locators { get; private set; }

        /// <summary>
        /// A collection of <see cref="LabelArc"/> objects for the label link.
        /// </summary>
        public List<LabelArc> LabelArcs { get; private set; }

        /// <summary>
        /// A collection of <see cref="Label"/> objects for the label link.
        /// </summary>
        public List<Label> Labels { get; private set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal LabelLink(INode LabelLinkNode) : base(LabelLinkNode)
        {
            this.Locators = new List<Locator>();
            this.LabelArcs = new List<LabelArc>();
            this.Labels = new List<Label>();
            ReadChildLocators(LabelLinkNode);
            ReadChildLabelArcs(LabelLinkNode);
            ReadChildLabels(LabelLinkNode);
            ResolveLocators();
        }

        /// <summary>
        /// Gets the label locator which references the given href.
        /// </summary>
        /// <param name="href">
        /// The href of the locator to be returned.
        /// </param>
        /// <returns>
        /// The label locator with the given href. Returns null if the given href does not have a label locator.
        /// </returns>
        public Locator GetLocator(string href)
        {
            foreach(var currentLocator in Locators)
            {
                if (currentLocator.Href.Equals(href) == true)
                    return currentLocator;
            }
            return null;
        }

        /// <summary>
        /// Gets the label arc with the specified "from" attribute value.
        /// </summary>
        /// <param name="fromAttributeValue">
        /// The value of the "from" attribute whose arc should be returned. 
        /// </param>
        /// <returns>
        /// The label arc having the supplied "from" attribute value, or null if the arc cannot be found.
        /// </returns>
        public LabelArc GetLabelArc(string fromAttributeValue)
        {
            foreach(var currentLabelArc in LabelArcs)
            {
                if (currentLabelArc.FromId.Equals(fromAttributeValue) == true)
                    return currentLabelArc;
            }
            return null;
        }

        /// <summary>
        /// Gets the label with the specified "label" attribute value.
        /// </summary>
        /// <param name="labelAttributeValue">
        /// The value of the "label" attribute whose label should be returned.
        /// </param>
        /// <returns>
        /// The label having the supplied "label" attribute value, or null if the label cannot be found.
        /// </returns>
        public Label GetLabel(string labelAttributeValue)
        {
            foreach(var currentLabel in Labels)
            {
                if (currentLabel.Label.Equals(labelAttributeValue) == true)
                    return currentLabel;
            }
            return null;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators()
        {
            IDictionary<string, Locator> dictLocators = this.Locators.ToDictionary(loc => loc.Label);
            foreach (LabelArc CurrentLabelArc in this.LabelArcs)
            {
                try
                {
                    CurrentLabelArc.FromLocator = dictLocators[CurrentLabelArc.FromId];
                }
                catch(KeyNotFoundException)
                {
                    CurrentLabelArc.FromLocator = null;
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLabelArcs(INode LabelLinkNode)
        {
            foreach (INode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("labelArc") == true)
                    this.LabelArcs.Add(new LabelArc(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLocators(INode LabelLinkNode)
        {
            foreach (INode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("loc") == true)
                    this.Locators.Add(new Locator(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLabels(INode LabelLinkNode)
        {
            foreach (INode CurrentChildNode in LabelLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("label") == true)
                    this.Labels.Add(new Label(CurrentChildNode));
            }
        }
    }
}
