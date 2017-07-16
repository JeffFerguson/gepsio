using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class NamespaceManager : INamespaceManager
    {
        private XmlNamespaceManager thisNamespaceManager;
        private IDocument thisDocument;

        public IDocument Document
        {
            get
            {
                return thisDocument;
            }
            set
            {
                thisDocument = value;
                var xmlDocument = (thisDocument as Document).XmlDocument;
                thisNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            }
        }

        internal XmlNamespaceManager XmlNamespaceManager
        {
            get
            {
                return thisNamespaceManager;
            }
        }

        public NamespaceManager()
        {
            thisNamespaceManager = new XmlNamespaceManager(new NameTable());
        }

        public void AddNamespace(string prefix, string uri)
        {
            thisNamespaceManager.AddNamespace(prefix, uri);
        }

        public string LookupPrefix(string uri)
        {
            return thisNamespaceManager.LookupPrefix(uri);
        }

        public string LookupNamespace(string prefix)
        {
            return thisNamespaceManager.LookupNamespace(prefix);
        }
    }
}
