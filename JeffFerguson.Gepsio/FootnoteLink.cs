using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "footnoteLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class FootnoteLink
    {
        private INode thisFootnoteLinkNode;

        /// <summary>
        /// A collection of <see cref="FootnoteArc"/> objects that apply to this footnote link.
        /// </summary>
        public List<FootnoteArc> FootnoteArcs { get; private set; }

        /// <summary>
        /// A collection of <see cref="FootnoteLocator"/> objects that apply to this footnote link.
        /// </summary>
        public List<FootnoteLocator> FootnoteLocators { get; private set; }

        /// <summary>
        /// A collection of <see cref="Footnote"/> objects that apply to this footnote link.
        /// </summary>
        public List<Footnote> Footnotes { get; private set; }

        /// <summary>
        /// The <see cref="XbrlFragment"/> which contains the unit.
        /// </summary>
        public XbrlFragment Fragment { get; private set; }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal FootnoteLink(XbrlFragment ParentFragment, INode FootnoteLinkNode)
        {
            thisFootnoteLinkNode = FootnoteLinkNode;
            this.Fragment = ParentFragment;
            this.Footnotes = new List<Footnote>();
            this.FootnoteLocators = new List<FootnoteLocator>();
            this.FootnoteArcs = new List<FootnoteArc>();
            foreach (INode ChildNode in thisFootnoteLinkNode.ChildNodes)
            {
                if (ChildNode.LocalName.Equals("loc") == true)
                    this.FootnoteLocators.Add(new FootnoteLocator(this, ChildNode));
                else if (ChildNode.LocalName.Equals("footnote") == true)
                    this.Footnotes.Add(new Footnote(this, ChildNode));
                else if (ChildNode.LocalName.Equals("footnoteArc") == true)
                    this.FootnoteArcs.Add(new FootnoteArc(this, ChildNode));
            }
        }

        /// <summary>
        /// Gets the footnote locator whose label matches the supplied label.
        /// </summary>
        /// <param name="Label">
        /// The label of the footnote locator to be returned.
        /// </param>
        /// <returns>
        /// The footnote locator whose label matches the supplied label. Null is returned if no
        /// matching footnote locator can be found.
        /// </returns>
        public FootnoteLocator GetLocator(string Label)
        {
            foreach (FootnoteLocator CurrentLocator in this.FootnoteLocators)
            {
                if (CurrentLocator.Label.Equals(Label) == true)
                    return CurrentLocator;
            }
            return null;
        }

        /// <summary>
        /// Gets the footnote whose label matches the supplied label.
        /// </summary>
        /// <param name="Label">
        /// The label of the footnote to be returned.
        /// </param>
        /// <returns>
        /// The footnote whose label matches the supplied label. Null is returned if no
        /// matching footnote can be found.
        /// </returns>
        public Footnote GetFootnote(string Label)
        {
            foreach (Footnote CurrentFootnote in this.Footnotes)
            {
                if (CurrentFootnote.Label.Equals(Label) == true)
                    return CurrentFootnote;
            }
            return null;
        }
    }
}
