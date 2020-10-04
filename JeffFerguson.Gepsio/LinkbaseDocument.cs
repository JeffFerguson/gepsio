using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.IO;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a linkbase document. A linkbase document is an XML document with a root
    /// element called linkbase. Linkbase documents are referenced in linkbaseRef elements in
    /// XBRL schemas.
    /// </summary>
    public class LinkbaseDocument
    {
        private readonly IDocument thisXmlDocument;
        private readonly string thisLinkbasePath;
        private readonly INamespaceManager thisNamespaceManager;
        internal INode thisLinkbaseNode;

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal LinkbaseDocument(string ContainingDocumentUri, string DocumentPath)
        {
            thisLinkbasePath = GetFullLinkbasePath(ContainingDocumentUri, DocumentPath);
            thisXmlDocument = Container.Resolve<IDocument>();
            thisXmlDocument.Load(thisLinkbasePath);
            thisNamespaceManager = Container.Resolve<INamespaceManager>();
            thisNamespaceManager.Document = thisXmlDocument;
            thisNamespaceManager.AddNamespace("default", XbrlDocument.XbrlLinkbaseNamespaceUri);
            thisLinkbaseNode = thisXmlDocument.SelectSingleNode("//default:linkbase", thisNamespaceManager);
        }

        //------------------------------------------------------------------------------------
        // Used for when a linkbaseRef element has no "role" attribute and the type of linkbase
        // document must be discovered. The 331-equivalentRelationships-instance-02.xml
        // document in the XBRL-CONF-2014-12-10 conformance suite is an example of this need.
        //------------------------------------------------------------------------------------
        internal static LinkbaseDocument Create(string containingDocumentUri, string href)
        {
            var newLinkbaseDocument = new LinkbaseDocument(containingDocumentUri, href);
            var firstChild = newLinkbaseDocument.thisLinkbaseNode.FirstChild;
            if(firstChild == null)
            {
                throw new NotSupportedException($"Linkbase node has no child nodes in document {href} at URI {containingDocumentUri}.");
            }

            string firstChildLocalName = firstChild.LocalName;
            if(firstChildLocalName.Equals("calculationLink"))
            {
                return new CalculationLinkbaseDocument(containingDocumentUri, href);
            }
            if (firstChildLocalName.Equals("definitionLink"))
            {
                return new DefinitionLinkbaseDocument(containingDocumentUri, href);
            }
            if (firstChildLocalName.Equals("labelLink"))
            {
                return new LabelLinkbaseDocument(containingDocumentUri, href);
            }
            if (firstChildLocalName.Equals("presentationLink"))
            {
                return new PresentationLinkbaseDocument(containingDocumentUri, href);
            }
            if (firstChildLocalName.Equals("referenceLink"))
            {
                return new ReferenceLinkbaseDocument(containingDocumentUri, href);
            }
            throw new NotSupportedException($"Linkbase node has unsupported child node with local name {firstChildLocalName} in document {href} at URI {containingDocumentUri}.");
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private string GetFullLinkbasePath(string ContainingDocumentUri, string LinkbaseDocFilename)
        {
            if (string.IsNullOrEmpty(ContainingDocumentUri))
                throw new ArgumentNullException(nameof(ContainingDocumentUri));
            if (string.IsNullOrEmpty(LinkbaseDocFilename))
                throw new ArgumentNullException(nameof(LinkbaseDocFilename));

            var lowerCaseLinkbaseDocFilename = LinkbaseDocFilename.ToLower();
            if (lowerCaseLinkbaseDocFilename.StartsWith("http://") ||
                lowerCaseLinkbaseDocFilename.StartsWith("https://") ||
                lowerCaseLinkbaseDocFilename.StartsWith("file://"))
            {
                return LinkbaseDocFilename;
            }

            string DocumentUri = ContainingDocumentUri;
            int LastPathSeparator = DocumentUri.LastIndexOf(Path.DirectorySeparatorChar);
            if (LastPathSeparator == -1 && Path.DirectorySeparatorChar != '/')
                LastPathSeparator = DocumentUri.LastIndexOf('/');

            // Check for remote linkbases when using local files
            string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);
            if (DocumentPath.StartsWith("file:///") &&
                (LinkbaseDocFilename.StartsWith("http://") || LinkbaseDocFilename.StartsWith("https://")))
            {
                return LinkbaseDocFilename;
            }

            return Path.Combine(DocumentPath, LinkbaseDocFilename);
        }

        //------------------------------------------------------------------------------------
        // Called during validation passes. Derived classes can override this to provide
        // linkbase-specific validation.
        //------------------------------------------------------------------------------------
        virtual internal void Validate()
        {
        }
    }
}
