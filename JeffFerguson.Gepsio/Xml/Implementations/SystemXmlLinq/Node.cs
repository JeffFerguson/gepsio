using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    internal class Node : INode
    {
        private XElement thisElement;
        private IAttributeList thisAttributeList;
        private INodeList thisChildNodeList;
        private INode thisParentNode;
        private INode thisFirstChild;

        public IAttributeList Attributes
        {
            get
            {
                if (thisAttributeList == null)
                {
                    var newList = new AttributeList();
                    foreach (XAttribute xmlAttribute in thisElement.Attributes())
                    {
                        var newAttribute = new Attribute(xmlAttribute);
                        newList.Add(newAttribute);
                    }
                    thisAttributeList = newList;
                }
                return thisAttributeList;
            }
        }

        public INodeList ChildNodes
        {
            get
            {
                if (thisChildNodeList == null)
                {
                    var newList = new NodeList();
                    foreach (XElement xmlChildNode in thisElement.Elements())
                    {
                        var newNode = new Node(xmlChildNode);
                        newList.Add(newNode);
                    }
                    thisChildNodeList = newList;
                }
                return thisChildNodeList;
            }
        }

        public string NamespaceURI => thisElement.Name.NamespaceName;

        public string BaseURI => thisElement.BaseUri;

        public string InnerText => thisElement.Value;

        public string Name => thisElement.Name.ToString();

        public string LocalName => thisElement.Name.LocalName;

        public bool IsComment => thisElement.NodeType == System.Xml.XmlNodeType.Comment;

        public INode ParentNode
        {
            get
            {
                if (thisParentNode == null)
                {
                    if(thisElement.Parent == null)
                    {
                        return null;
                    }
                    thisParentNode = new Node(thisElement.Parent);
                }
                return thisParentNode;
            }
        }

        public string Prefix
        {
            get
            {
                var prefix = thisElement.GetPrefixOfNamespace(thisElement.Name.Namespace);
                if(prefix == null)
                {
                    prefix = string.Empty;
                }
                return prefix;
            }
        }

        /// <summary>
        /// Returns the first child node of the current node.
        /// </summary>
        /// <remarks>
        /// In the System.Xml implementation, if an element node has no child element nodes,
        /// FirstChild natively returns a new text node with the element's text in the node.
        /// This implementation mimics that behavior.
        /// </remarks>
        public INode FirstChild
        {
            get
            {
                if(thisFirstChild == null)
                {
                    var firstChildElement = thisElement.Elements().FirstOrDefault();
                    if(firstChildElement != null)
                    {
                        thisFirstChild = new Node(firstChildElement);
                    }
                    else
                    {
                        var textElement = new XElement(thisElement.Name, thisElement.Value);
                        thisFirstChild = new Node(textElement);
                    }
                }
                return thisFirstChild;
            }
        }

        public string Value => thisElement.Value;

        internal Node(XElement element)
        {
            thisElement = element;
        }

        public string GetAttributeValue(string AttributeName)
        {
            bool NameIncludesNamespaceId;

            if (AttributeName.IndexOf(':') == -1)
                NameIncludesNamespaceId = false;
            else
                NameIncludesNamespaceId = true;
            foreach (IAttribute CurrentAttribute in Attributes)
            {
                if (NameIncludesNamespaceId == false)
                {
                    if (CurrentAttribute.LocalName.Equals(AttributeName) == true)
                        return CurrentAttribute.Value;
                }
                else
                {
                    if (CurrentAttribute.Name.Equals(AttributeName) == true)
                        return CurrentAttribute.Value;
                }
            }
            return string.Empty;
        }

        public string GetAttributeValue(string AttributeNamespaceUri, string AttributeLocalName)
        {
            foreach (IAttribute CurrentAttribute in Attributes)
            {
                if (CurrentAttribute.NamespaceURI.Equals(AttributeNamespaceUri) == true)
                {
                    if (CurrentAttribute.LocalName.Equals(AttributeLocalName) == true)
                        return CurrentAttribute.Value;
                }
            }
            return string.Empty;
        }

        public bool ParentEquals(INode OtherNode)
        {
            if (OtherNode == null)
                return false;
            return object.ReferenceEquals((this.ParentNode as Node).thisElement, (OtherNode.ParentNode as Node).thisElement);
        }

        public INodeList SelectNodes(string xpath, INamespaceManager namespaceManager)
        {
            var selectedElements = thisElement.XPathSelectElements(xpath, (namespaceManager as NamespaceManager).XmlNamespaceManager);
            var nodeList = new NodeList();
            foreach (var currentElement in selectedElements)
            {
                var newNode = new Node(currentElement);
                nodeList.Add(newNode);
            }
            return nodeList;
        }

        public bool StructureEquals(INode OtherNode, XbrlFragment containingFragment)
        {
            if (OtherNode == null)
            {
                return false;
            }
            if (this.NamespaceURI.Equals(OtherNode.NamespaceURI) == false)
            {
                return false;
            }
            if (this.LocalName.Equals(OtherNode.LocalName) == false)
            {
                return false;
            }
            if (this.Value.Equals(OtherNode.Value) == false)
            {
                return false;
            }
            if(this.Attributes.StructureEquals(OtherNode.Attributes, containingFragment) == false)
            {
                return false;
            }
            return this.ChildNodes.StructureEquals(OtherNode.ChildNodes, containingFragment);
        }
    }
}
