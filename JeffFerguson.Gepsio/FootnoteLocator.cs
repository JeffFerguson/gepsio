using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A locator used for a footnote.
    /// </summary>
    public class FootnoteLocator
    {
        private INode thisLocNode;

        /// <summary>
        /// The link for this footnote locator.
        /// </summary>
        public FootnoteLink Link { get; private set; }

        /// <summary>
        /// The hyperlink reference for this footnote locator.
        /// </summary>
        public HyperlinkReference Href { get; private set; }

        /// <summary>
        /// The label of this footnote locator.
        /// </summary>
        public string Label { get; private set; }

        internal FootnoteLocator(FootnoteLink ParentLink, INode LocNode)
        {
            thisLocNode = LocNode;
            this.Link = ParentLink;
            foreach (IAttribute CurrentAttribute in LocNode.Attributes)
            {
                if (CurrentAttribute.LocalName.Equals("href") == true)
                {
                    string AttributeValue;

                    AttributeValue = CurrentAttribute.Value;
                    this.Href = new HyperlinkReference(AttributeValue);
                }
                else if (CurrentAttribute.LocalName.Equals("label") == true)
                {
                    this.Label = CurrentAttribute.Value;
                }
            }
        }
    }
}
