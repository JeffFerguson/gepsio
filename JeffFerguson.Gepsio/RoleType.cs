using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a role type. Role types are used to define custom role values in XBRL
    /// extended links. Role types are defined by roleType elements in XBRL schemas.
    /// </summary>
    public class RoleType
    {
        /// <summary>
        /// The schema that references this role type.
        /// </summary>
        public XbrlSchema Schema { get; private set; }

        /// <summary>
        /// The URI for this role.
        /// </summary>
        public Uri RoleUri { get; private set; }

        /// <summary>
        /// The ID of this role type.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The definition of this role type.
        /// </summary>
        public string Definition { get; private set; }

        /// <summary>
        /// A collection of "UsedOn" references for this role type. Used to identify what
        /// elements may use a taxonomy defined role or arc role value.
        /// </summary>
        public List<string> UsedOnReferences { get; private set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal RoleType(XbrlSchema ContainingXbrlSchema, INode roleTypeNode)
        {
            this.Schema = ContainingXbrlSchema;
            this.UsedOnReferences = new List<string>();
            this.RoleUri = new Uri(roleTypeNode.GetAttributeValue("roleURI"));
            this.Id = roleTypeNode.GetAttributeValue("id");
            foreach (INode currentChild in roleTypeNode.ChildNodes)
            {
                if (currentChild.LocalName.Equals("definition") == true)
                {
                    this.Definition = currentChild.InnerText;
                }
                if (currentChild.LocalName.Equals("usedOn") == true)
                {
                    this.UsedOnReferences.Add(currentChild.InnerText);
                }
            }
        }
    }
}
