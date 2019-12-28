using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    /// <summary>
    /// An implementation of IDocument that uses the System.Xml.Xml* classes from .NET 3.5.
    /// </summary>
    internal class Document : IDocument
    {
        internal XmlDocument XmlDocument { get; private set; }

        public Document()
        {
            this.XmlDocument = new XmlDocument();
        }

        public void Load(string path) => this.XmlDocument.Load(path);

        public void Load(Stream stream) => this.XmlDocument.Load(stream);

        public Task LoadAsync(string path) => throw new NotImplementedException("SystemXml implementation does not support async loading.");

        public Task LoadAsync(Stream stream) => throw new NotImplementedException("SystemXml implementation does not support async loading.");

        public INodeList SelectNodes(string xpath, INamespaceManager namespaceManager)
        {            
            var xmlNamespaceManager = (namespaceManager as NamespaceManager).XmlNamespaceManager;
            var xmlNodeList = this.XmlDocument.SelectNodes(xpath, xmlNamespaceManager);
            var listToReturn = new NodeList();
            foreach(XmlNode foundNode in xmlNodeList)
            {
                var newNode = new Node(foundNode);
                listToReturn.Add(newNode);
            }
            return listToReturn;
        }

        public INode SelectSingleNode(string xPath, INamespaceManager namespaceManager)
        {
            var xmlNamespaceManager = (namespaceManager as NamespaceManager).XmlNamespaceManager;
            var xmlNode = this.XmlDocument.SelectSingleNode(xPath, xmlNamespaceManager);
            return new Node(xmlNode);
        }
    }
}
