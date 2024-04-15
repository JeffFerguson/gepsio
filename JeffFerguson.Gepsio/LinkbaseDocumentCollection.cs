using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A collection of linkbase documents.
    /// </summary>
    /// <remarks>
    /// Linkbase documents are referenced from XML nodes with a local name of "linkbaseRef".
    /// Linkbase references can be found within schemas or within XBRL fragments.
    /// </remarks>
    public class LinkbaseDocumentCollection
    {
        private readonly List<LinkbaseDocument> thisLinkbaseDocuments;

        /// <summary>
        /// A reference to the schema's calculation linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public CalculationLinkbaseDocument CalculationLinkbase
        {
            get
            {
                if (thisLinkbaseDocuments != null)
                {
                    foreach (var currentLinkbaseDocument in thisLinkbaseDocuments)
                    {
                        if (currentLinkbaseDocument is CalculationLinkbaseDocument)
                            return currentLinkbaseDocument as CalculationLinkbaseDocument;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// A reference to the schema's definition linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public DefinitionLinkbaseDocument DefinitionLinkbase
        {
            get
            {
                if (thisLinkbaseDocuments != null)
                {
                    foreach (var currentLinkbaseDocument in thisLinkbaseDocuments)
                    {
                        if (currentLinkbaseDocument is DefinitionLinkbaseDocument)
                            return currentLinkbaseDocument as DefinitionLinkbaseDocument;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// A reference to the schema's label linkbase. Null is returned if no such linkbase is available.
        /// </summary>
		public IEnumerable< LabelLinkbaseDocument > LabelLinkbase
        {
            get
            {
                if (thisLinkbaseDocuments != null)
                {
                    foreach (var currentLinkbaseDocument in thisLinkbaseDocuments)
                    {
                        if (currentLinkbaseDocument is LabelLinkbaseDocument)
                            yield return currentLinkbaseDocument as LabelLinkbaseDocument;
                    }
                }
            }
        }

        /// <summary>
        /// A reference to the schema's presentation linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public IEnumerable<PresentationLinkbaseDocument> PresentationLinkbase
        {
            get
            {
                if (thisLinkbaseDocuments != null)
                {
                    foreach (var currentLinkbaseDocument in thisLinkbaseDocuments)
                    {
                        if (currentLinkbaseDocument is PresentationLinkbaseDocument)
                            yield return currentLinkbaseDocument as PresentationLinkbaseDocument;
                    }
                }
			}
        }

        /// <summary>
        /// A reference to the schema's reference linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public ReferenceLinkbaseDocument ReferenceLinkbase
        {
            get
            {
                if (thisLinkbaseDocuments != null)
                {
                    foreach (var currentLinkbaseDocument in thisLinkbaseDocuments)
                    {
                        if (currentLinkbaseDocument is ReferenceLinkbaseDocument)
                            return currentLinkbaseDocument as ReferenceLinkbaseDocument;
                    }
                }
                return null;
            }
        }

        internal LinkbaseDocumentCollection()
        {
            thisLinkbaseDocuments = new List<LinkbaseDocument>();
        }

        /// <summary>
        /// Read linkbase references found in a schema.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the linkbase reference.
        /// </param>
        /// <param name="parentNode">
        /// The XML node whose child nodes will be searched for linkbase references.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase reference.</param>
        internal void ReadLinkbaseReferences(string ContainingDocumentUri, INode parentNode, XbrlFragment containingFragment)
        {
            foreach (INode CurrentChild in parentNode.ChildNodes)
            {
                if ((CurrentChild.NamespaceURI.Equals(XbrlDocument.XbrlLinkbaseNamespaceUri) == true) && (CurrentChild.LocalName.Equals("linkbaseRef") == true))
                    ReadLinkbaseReference(ContainingDocumentUri, CurrentChild, containingFragment);
            }
        }

        private void ReadLinkbaseReference(string ContainingDocumentUri, INode LinkbaseReferenceNode, XbrlFragment containingFragment)
        {
            var xlinkNode = new XlinkNode(LinkbaseReferenceNode);
            if (xlinkNode.IsInRole(XbrlDocument.XbrlCalculationLinkbaseReferenceRoleNamespaceUri) == true)
            {
                this.thisLinkbaseDocuments.Add(new CalculationLinkbaseDocument(ContainingDocumentUri, xlinkNode.Href, containingFragment));
            }
            else if (xlinkNode.IsInRole(XbrlDocument.XbrlDefinitionLinkbaseReferenceRoleNamespaceUri) == true)
            {
                this.thisLinkbaseDocuments.Add(new DefinitionLinkbaseDocument(ContainingDocumentUri, xlinkNode.Href, containingFragment));
            }
            else if (xlinkNode.IsInRole(XbrlDocument.XbrlLabelLinkbaseReferenceRoleNamespaceUri) == true)
            {
                this.thisLinkbaseDocuments.Add(new LabelLinkbaseDocument(ContainingDocumentUri, xlinkNode.Href, containingFragment));
            }
            else if (xlinkNode.IsInRole(XbrlDocument.XbrlPresentationLinkbaseReferenceRoleNamespaceUri) == true)
            {
                this.thisLinkbaseDocuments.Add(new PresentationLinkbaseDocument(ContainingDocumentUri, xlinkNode.Href, containingFragment));
            }
            else if (xlinkNode.IsInRole(XbrlDocument.XbrlReferenceLinkbaseReferenceRoleNamespaceUri) == true)
            {
                this.thisLinkbaseDocuments.Add(new ReferenceLinkbaseDocument(ContainingDocumentUri, xlinkNode.Href, containingFragment));
            }
            else
            {

                // At this point, the role is either not available or not in the list of supported roles.
                // Attempt to use a factory method to look at the linkbase document and attempt to discover
                // the correct document type.

                var createdDocument = LinkbaseDocument.Create(ContainingDocumentUri, xlinkNode.Href, containingFragment);
                if (createdDocument != null)
                {
                    this.thisLinkbaseDocuments.Add(createdDocument);
                }
            }
        }
    }
}
