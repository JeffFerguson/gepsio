using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Represents a linkbase document. A linkbase document is an XML document with a root
    /// element called linkbase. Linkbase documents are referenced in linkbaseRef elements in
    /// XBRL schemas.
    /// </summary>
    public class LinkbaseDocument
    {
        private IDocument thisXmlDocument;
        private string thisLinkbasePath;
        private INamespaceManager thisNamespaceManager;
        internal INode thisLinkbaseNode;

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal LinkbaseDocument()
        {
            thisXmlDocument = null;
            thisLinkbasePath = string.Empty;
            thisNamespaceManager = null;
            thisLinkbaseNode = null;
        }

        /// <summary>
        /// Construct a linkbase document object.
        /// </summary>
        /// <param name="ContainingDocumentUri">
        /// The URI of the document containing the linkbase.
        /// </param>
        /// <param name="DocumentPath">
        /// The path to the document containing the linkbase.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the linkbase reference.
        /// </param>
        internal LinkbaseDocument(string ContainingDocumentUri, string DocumentPath, XbrlFragment containingFragment)
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
        // A NULL reference will be returned if the correct type cannot be identified.
        //------------------------------------------------------------------------------------
        internal static LinkbaseDocument Create(string containingDocumentUri, string href, XbrlFragment containingFragment)
        {
            var newLinkbaseDocument = new LinkbaseDocument(containingDocumentUri, href, containingFragment);
            var firstChild = newLinkbaseDocument.thisLinkbaseNode.FirstChild;
            if (firstChild == null)
            {
                throw new NotSupportedException($"Linkbase node has no child nodes in document {href} at URI {containingDocumentUri}.");
            }
            string firstChildLocalName = firstChild.LocalName;
            firstChild = null;
            newLinkbaseDocument = null;
            if (firstChildLocalName.Equals("calculationLink"))
            {
                return new CalculationLinkbaseDocument(containingDocumentUri, href, containingFragment);
            }
            if (firstChildLocalName.Equals("definitionLink"))
            {
                return new DefinitionLinkbaseDocument(containingDocumentUri, href, containingFragment);
            }
            if (firstChildLocalName.Equals("labelLink"))
            {
                return new LabelLinkbaseDocument(containingDocumentUri, href, containingFragment);
            }
            if (firstChildLocalName.Equals("presentationLink"))
            {
                return new PresentationLinkbaseDocument(containingDocumentUri, href, containingFragment);
            }
            if (firstChildLocalName.Equals("referenceLink"))
            {
                return new ReferenceLinkbaseDocument(containingDocumentUri, href, containingFragment);
            }
            return null;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private string GetFullLinkbasePath(string ContainingDocumentUri, string LinkbaseDocFilename)
        {
            string FullPath;
            if (LinkbaseDocFilename.StartsWith("http://") == true)
            {
                return LinkbaseDocFilename;
            }
            var DocumentUri = ContainingDocumentUri;
            int LastPathSeparator = DocumentUri.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
            if (LastPathSeparator == -1)
                LastPathSeparator = DocumentUri.LastIndexOf('/');
            string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);

            // Check for remote linkbases when using local files.

            if ((DocumentPath.StartsWith("file:///") == true) && (LinkbaseDocFilename.StartsWith("http://") == true))
                return LinkbaseDocFilename;
            FullPath = DocumentPath + LinkbaseDocFilename;
            return FullPath;
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
