﻿using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// An implementation of IDocument that uses the System.Xml.Linq classes from .NET Standard 2.1.
    /// </summary>
    internal class Document : IDocument
    {
        private XDocument doc;

        public void Load(string path)
        {
            doc = XDocument.Load(path, LoadOptions.SetBaseUri);
        }

        public void Load(Stream stream)
        {
            doc = XDocument.Load(stream, LoadOptions.SetBaseUri);
        }

        public async Task LoadAsync(string path)
        {
            if(File.Exists(path))
            {
                await LoadFileAsync(path);
            }
            else
            {
                await LoadUriAsync(path);
            }
        }

        private async Task LoadUriAsync(string path)
        {
            var req = WebRequest.Create(path);
            using (Stream stream = req.GetResponse().GetResponseStream())
            {
                await LoadAsync(stream);
            }
        }

        private async Task LoadFileAsync(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                await LoadAsync(reader.BaseStream);
            }
        }

        public async Task LoadAsync(Stream stream)
        {
            var cancelToken = new CancellationToken();
            doc = await XDocument.LoadAsync(stream, LoadOptions.SetBaseUri, cancelToken);
        }

        public INodeList SelectNodes(string xpath, INamespaceManager namespaceManager)
        {
            var selectedElements = doc.XPathSelectElements(xpath, (namespaceManager as NamespaceManager).XmlNamespaceManager);
            var nodeList = new NodeList();
            foreach(var currentElement in selectedElements)
            {
                var newNode = new Node(currentElement);
                nodeList.Add(newNode);
            }
            return nodeList;
        }

        public INode SelectSingleNode(string xPath, INamespaceManager namespaceManager)
        {
            var selectedElement = doc.XPathSelectElement(xPath, (namespaceManager as NamespaceManager).XmlNamespaceManager);
            if(selectedElement != null)
            {
                var newNode = new Node(selectedElement);
                return newNode;
            }
            return null;
        }
    }
}
