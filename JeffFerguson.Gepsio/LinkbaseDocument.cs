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
        //------------------------------------------------------------------------------------
        private string GetFullLinkbasePath(string ContainingDocumentUri, string LinkbaseDocFilename)
        {
            string FullPath;
            int FirstPathSeparator = LinkbaseDocFilename.IndexOf(System.IO.Path.DirectorySeparatorChar);
            if (FirstPathSeparator == -1)
            {
                string DocumentUri = ContainingDocumentUri;
                int LastPathSeparator = DocumentUri.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                if (LastPathSeparator == -1)
                    LastPathSeparator = DocumentUri.LastIndexOf('/');
                string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);

                // Check for remote linkbases when using local files

                if ((DocumentPath.StartsWith("file:///") == true) && (LinkbaseDocFilename.StartsWith("http://") == true))
                    return LinkbaseDocFilename;

                FullPath = DocumentPath + LinkbaseDocFilename;
            }
            else
            {
                throw new NotImplementedException("XbrlSchema.GetFullSchemaPath() code path not implemented.");
            }
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
