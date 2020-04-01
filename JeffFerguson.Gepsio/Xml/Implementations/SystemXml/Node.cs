using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class Node : INode
    {
        internal XmlNode thisNode;
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
                    foreach (XmlAttribute xmlAttribute in thisNode.Attributes)
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
                    foreach (XmlNode xmlChildNode in thisNode.ChildNodes)
                    {
                        var newNode = new Node(xmlChildNode);
                        newList.Add(newNode);
                    }
                    thisChildNodeList = newList;
                }
                return thisChildNodeList;
            }
        }

        public INode ParentNode
        {
            get
            {
                if (thisParentNode == null)
                    thisParentNode = new Node(thisNode.ParentNode);
                return thisParentNode;
            }
        }

        public string NamespaceURI => thisNode.NamespaceURI;

        public string BaseURI => thisNode.BaseURI;

        public string InnerText => thisNode.InnerText;

        public string LocalName => thisNode.LocalName;

        public string Name => thisNode.Name;

        public bool IsComment
        {
            get
            {
                if (thisNode.NodeType == XmlNodeType.Comment)
                    return true;
                return false;
            }
        }

        public string Prefix => thisNode.Prefix;

        public INode FirstChild
        {
            get
            {
                if (thisFirstChild == null)
                {
                    var first = thisNode.FirstChild;
                    if (first != null)
                        thisFirstChild = new Node(first);
                }
                return thisFirstChild;
            }
        }

        public string Value => thisNode.Value;

        internal Node(XmlNode node)
        {
            thisNode = node;
            thisAttributeList = null;
            thisChildNodeList = null;
            thisParentNode = null;
            thisFirstChild = null;
        }

        public INodeList SelectNodes(string xpath, INamespaceManager namespaceManager)
        {
            var xmlNamespaceManager = (namespaceManager as NamespaceManager).XmlNamespaceManager;
            var xmlNodeList = thisNode.SelectNodes(xpath, xmlNamespaceManager);
            var listToReturn = new NodeList();
            foreach (XmlNode foundNode in xmlNodeList)
            {
                var newNode = new Node(foundNode);
                listToReturn.Add(newNode);
            }
            return listToReturn;
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

        public bool StructureEquals(INode OtherNode, XbrlFragment containingFragment)
        {
            if (OtherNode == null)
                return false;
            if (this.NamespaceURI.Equals(OtherNode.NamespaceURI) == false)
                return false;
            if (this.LocalName.Equals(OtherNode.LocalName) == false)
                return false;
            if (this.Value.Equals(OtherNode.Value) == false)
                return false;
            return this.ChildNodes.StructureEquals(OtherNode.ChildNodes, containingFragment);
        }

        public bool ParentEquals(INode OtherNode)
        {
            if (OtherNode == null)
                return false;
            return object.ReferenceEquals((this.ParentNode as Node).thisNode, (OtherNode.ParentNode as Node).thisNode);
        }

        /// <summary>
        /// The value of the node typed to the data type specified in
        /// the schema definition for the node. If no data type is available,
        /// then a string representation is returned, in which case TypedValue
        /// returns the same string as what the InnerText property returns.
        /// </summary>
        /// <param name="containingFragment">
        /// The fragment containing the attributes.
        /// </param>
        /// <returns>
        /// The value of the node typed to the data type specified in
        /// the schema definition for the node. If no data type is available,
        /// then a string representation is returned.
        /// </returns>
        public object GetTypedValue(XbrlFragment containingFragment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares the typed value of this node with the typed value of another node.
        /// </summary>
        /// <param name="otherNode">
        /// The other node whose typed value is to be compared with this node's typed value.
        /// </param>
        /// <param name="containingFragment">
        /// The fragment containing the attributes.
        /// </param>
        /// <returns>
        /// True if the nodes have the same typed value; false otherwise.
        /// </returns>
        public bool TypedValueEquals(INode otherNode, XbrlFragment containingFragment)
        {
            throw new NotImplementedException();
        }
    }
}
