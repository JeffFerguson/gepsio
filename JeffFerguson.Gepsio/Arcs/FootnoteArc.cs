using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "footnoteArc" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class FootnoteArc : Arc
    {
        private Item fromItem;
        private Footnote fromFootnote;

        /// <summary>
        /// Describes the possible sources of the "from" portion of a footnote arc.
        /// </summary>
        public enum FromSources
        {
            /// <summary>
            /// This footnote arc has no "from" portion set.
            /// </summary>
            None,
            /// <summary>
            /// This footnote arc has an item as the "from" source.
            /// </summary>
            Item,
            /// <summary>
            /// This footnote arc has an footnote as the "from" source.
            /// </summary>
            Footnote
        }

        /// <summary>
        /// The link definition for the footnote.
        /// </summary>
        public FootnoteLink Link { get; private set; }

        /// <summary>
        /// The item referenced by the "from" portion of the footnote arc.
        /// </summary>
        /// <remarks>
        /// This value will be valid only if the <see cref="FromSource"/> property
        /// has a value of <see cref="FromSources.Item"/>. If the property has any
        /// other value, then the value of this property will be null.
        /// </remarks>
        public Item FromItem
        {
            get
            {
                return fromItem;
            }
            internal set
            {
                fromItem = value;
                if(value != null)
                    this.FromSource = FromSources.Item;
            }
        }

        /// <summary>
        /// The footnote referenced by the "from" portion of the footnote arc.
        /// </summary>
        /// <remarks>
        /// This value will be valid only if the <see cref="FromSource"/> property
        /// has a value of <see cref="FromSources.Footnote"/>. If the property has any
        /// other value, then the value of this property will be null.
        /// </remarks>
        public Footnote FromFootnote
        {
            get
            {
                return fromFootnote;
            }
            internal set
            {
                fromFootnote = value;
                if(value != null)
                    this.FromSource = FromSources.Footnote;
            }
        }

        /// <summary>
        /// The footnote referenced by the "to" portion of the footnote arc.
        /// </summary>
        public Footnote ToFootnote { get; internal set; }

        /// <summary>
        /// Describes whether or not the footnote arc is in the standard footnote arc role,
        /// according to section 4.11.1.3.1 of the XBRL spec.
        /// </summary>
        public bool StandardArcRole
        {
            get
            {
                return ArcRole.Equals(XbrlDocument.XbrlFactFootnoteArcroleNamespaceUri);
            }
        }

        /// <summary>
        /// The source of the "from" portion of the arc.
        /// </summary>
        public FromSources FromSource { get; private set; }

        internal FootnoteArc(FootnoteLink ParentLink, INode FootnoteArcNode) : base(FootnoteArcNode)
        {
            this.Link = ParentLink;
            this.FromSource = FromSources.None;
        }
    }
}
