using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "definitionArc" as defined in the http://www.xbrl.org/2003/linkbase namespace.
    /// </summary>
    /// <remarks>
    /// A definition arc is a concrete arc for use in definition extended links.
    /// </remarks>
    public class DefinitionArc : Arc
    {
        /// <summary>
        /// An enumeration describing the role of this definition arc.
        /// </summary>
        public enum RoleEnum
        {
            /// <summary>
            /// An unknown role type.
            /// </summary>
            Unknown,
            /// <summary>
            /// An essence alias role type. Used when the definition arc's role has a value of http://www.xbrl.org/2003/arcrole/essence-alias.
            /// </summary>
            EssenceAlias,
            /// <summary>
            /// A general special role type. Used when the definition arc's role has a value of http://www.xbrl.org/2003/arcrole/general-special.
            /// </summary>
            GeneralSpecial,
            /// <summary>
            /// A similar tuples role type. Used when the definition arc's role has a value of http://www.xbrl.org/2003/arcrole/similar-tuples.
            /// </summary>
            SimilarTuples,
            /// <summary>
            /// A requires element role type. Used when the definition arc's role has a value of http://www.xbrl.org/2003/arcrole/requires-element.
            /// </summary>
            RequiresElement
        }

        /// <summary>
        /// The role of this definition arc.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A definition arc role is defined in XBRL markup through a "definitionArc" element attribute named
        /// "arcrole". The value of the "arcrole" attribute, if it exists, should reference a namespace that
        /// identifies the role.
        /// </para>
        /// <para>
        /// If the "arcrole" attribute is not found in the "definitionArc" element, or if the value of the
        /// "arcrole" attribute is not one of the defined values, then the Role will be set to <see cref="RoleEnum.Unknown"/>.
        /// </para>
        /// <para>
        /// This property is named "DefinitionArcRole" to distinguish it from the "Role" and "ArcRole" properties
        /// found in the XLinkNode base class.
        /// </para>
        /// </remarks>
        public RoleEnum DefinitionArcRole { get; private set; }

        /// <summary>
        /// The identifier of the "from" portion of the definition arc.
        /// </summary>
        public string FromId { get; private set; }

        /// <summary>
        /// The identifier of the "to" portion of the definition arc.
        /// </summary>
        public string ToId { get; private set; }

        /// <summary>
        /// The locator of the "from" portion of the definition arc.
        /// </summary>
        public Locator FromLocator { get; internal set; }

        /// <summary>
        /// The locator of the "to" portion of the definition arc.
        /// </summary>
        public Locator ToLocator { get; internal set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal DefinitionArc(INode DefinitionArcNode) : base(DefinitionArcNode)
        {
            foreach (IAttribute CurrentAttribute in DefinitionArcNode.Attributes)
            {
                if (CurrentAttribute.NamespaceURI.Equals(XlinkNode.xlinkNamespace) == false)
                    continue;
                if (CurrentAttribute.LocalName.Equals("arcrole") == true)
                    SetRole(CurrentAttribute.Value);
                else if (CurrentAttribute.LocalName.Equals("from") == true)
                    this.FromId = CurrentAttribute.Value;
                else if (CurrentAttribute.LocalName.Equals("to") == true)
                    this.ToId = CurrentAttribute.Value;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetRole(string ArcRoleValue)
        {
            this.DefinitionArcRole = RoleEnum.Unknown;
            if (ArcRoleValue.Equals(XbrlDocument.XbrlEssenceAliasArcroleNamespaceUri) == true)
                this.DefinitionArcRole = RoleEnum.EssenceAlias;
            else if (ArcRoleValue.Equals(XbrlDocument.XbrlGeneralSpecialArcroleNamespaceUri) == true)
                this.DefinitionArcRole = RoleEnum.GeneralSpecial;
            else if (ArcRoleValue.Equals(XbrlDocument.XbrlSimilarTuplesArcroleNamespaceUri) == true)
                this.DefinitionArcRole = RoleEnum.SimilarTuples;
            else if (ArcRoleValue.Equals(XbrlDocument.XbrlRequiresElementArcroleNamespaceUri) == true)
                this.DefinitionArcRole = RoleEnum.RequiresElement;
        }
    }
}
