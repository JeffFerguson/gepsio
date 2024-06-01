using JeffFerguson.Gepsio.Xml.Interfaces;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    /// <summary>
    /// An implementation of IDocument that uses the System.Xml.Linq classes from .NET 8.
    /// </summary>
    internal class Document : IDocument
    {
        private XDocument doc;
        private static readonly HttpClient httpClient;

        // Per Microsoft's guidance, HttpClient is intended to be instantiated once and re-used
        // throughout the life of an application. Instantiating an HttpClient class for every request
        // will exhaust the number of sockets available under heavy loads. This will result in
        // SocketException errors.
        static Document()
        {
            httpClient = new HttpClient();
        }

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
            if (File.Exists(path))
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
            var clientResponse = await httpClient.GetAsync(path);
            using (var clientResponseAsStream = await clientResponse.Content.ReadAsStreamAsync())
            {
                await LoadAsync(clientResponseAsStream);
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
            foreach (var currentElement in selectedElements)
            {
                var newNode = new Node(currentElement);
                nodeList.Add(newNode);
            }
            return nodeList;
        }

        public INode SelectSingleNode(string xPath, INamespaceManager namespaceManager)
        {
            var selectedElement = doc.XPathSelectElement(xPath, (namespaceManager as NamespaceManager).XmlNamespaceManager);
            if (selectedElement != null)
            {
                var newNode = new Node(selectedElement);
                return newNode;
            }
            return null;
        }
    }
}
