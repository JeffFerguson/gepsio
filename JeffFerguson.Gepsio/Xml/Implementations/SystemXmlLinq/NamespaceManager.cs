using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    internal class NamespaceManager : INamespaceManager
    {
        private XmlNamespaceManager thisNamespaceManager;
        private IDocument thisDocument;

        public IDocument Document
        {
            get => thisDocument;
            set => thisDocument = value;
        }

        internal XmlNamespaceManager XmlNamespaceManager => thisNamespaceManager;

        public NamespaceManager() => thisNamespaceManager = new XmlNamespaceManager(new NameTable());

        public void AddNamespace(string prefix, string uri)
        {
            if (prefix.Equals("xmlns") == false)
            {
                thisNamespaceManager.AddNamespace(prefix, uri);
            }
        }

        public string LookupPrefix(string uri) => thisNamespaceManager.LookupPrefix(uri);

        public string LookupNamespace(string prefix) => thisNamespaceManager.LookupNamespace(prefix);
    }
}
