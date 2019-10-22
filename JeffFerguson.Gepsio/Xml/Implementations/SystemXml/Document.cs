using System;

using JeffFerguson.Gepsio.Xml.Interfaces;
using System.IO;
using System.Xml;

using JeffFerguson.Gepsio.IoC;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    /// <summary>
    /// An implementation of IDocument that uses the System.Xml.Xml* classes from .NET 3.5.
    /// </summary>
    internal class Document : IDocument
    {
        private XmlDocument thisDocument;

        internal XmlDocument XmlDocument
        {
            get
            {
                return thisDocument;
            }
        }

        public Document()
        {
            thisDocument = new XmlDocument{XmlResolver = Container.Resolve<XmlResolver>(  )};
        }

        public void Load(string path)
        {
            thisDocument.Load(path);
        }

        public void Load(Stream stream)
        {
            thisDocument.Load(stream);
        }

        public INodeList SelectNodes(string xpath, INamespaceManager namespaceManager)
        {            
            var xmlNamespaceManager = (namespaceManager as NamespaceManager).XmlNamespaceManager;
            var xmlNodeList = thisDocument.SelectNodes(xpath, xmlNamespaceManager);
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
            var xmlNode = thisDocument.SelectSingleNode(xPath, xmlNamespaceManager);
            return new Node(xmlNode);
        }
    }
}
