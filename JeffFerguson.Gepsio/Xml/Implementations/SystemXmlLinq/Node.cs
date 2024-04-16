using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Xml;
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
        private object thisTypedValue;
        private bool thisTypedValueInitialized;

        public IAttributeList Attributes
        {
            get
            {
                if (thisAttributeList == null)
                {
                    var newList = new AttributeList();
                    foreach (XAttribute xmlAttribute in thisElement.Attributes())
                    {
                        var newAttribute = new Attribute(xmlAttribute, this);
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

        /// <summary>
        /// Returns only the text of this node without including text from child nodes.
        /// </summary>
        public string InnerText
        {
            get
            {
                var textNode = thisElement.Nodes().OfType<XText>().FirstOrDefault();
                if (textNode == null)
                {
                    return string.Empty;
                }
                return textNode.ToString();
            }
        }

        public string Name => thisElement.Name.ToString();

        public string LocalName => thisElement.Name.LocalName;

        public bool IsComment => thisElement.NodeType == XmlNodeType.Comment;

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

        /// <summary>
        /// Returns the text of this node concatenated with text from child nodes.
        /// </summary>
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
            if(this.TypedValueEquals(OtherNode, containingFragment) == false)
            {
                return false;
            }
            if (this.Attributes.StructureEquals(OtherNode.Attributes, containingFragment) == false)
            {
                return false;
            }
            return this.ChildNodes.StructureEquals(OtherNode.ChildNodes, containingFragment);
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
            if (thisTypedValueInitialized == false)
            {
                InitializeTypedValue(containingFragment);
                thisTypedValueInitialized = true;
            }
            return thisTypedValue;
        }

        /// <summary>
        /// Initialize the attribute's typed value.
        /// </summary>
        /// <param name="containingFragment">
        /// The fragment containing the attribute.
        /// </param>
        private void InitializeTypedValue(XbrlFragment containingFragment)
        {
            var nodeType = containingFragment.Schemas.GetNodeType(this);
            if (nodeType == null)
            {
                thisTypedValue = this.InnerText;
                return;
            }
            if (nodeType is Xsd.String)
            {
                thisTypedValue = this.InnerText;
                return;
            }
            if (nodeType is Xsd.Decimal)
            {
                thisTypedValue = Convert.ToDecimal(this.InnerText, CultureInfo.InvariantCulture);
                return;
            }
            if (nodeType is Xsd.Double)
            {
                // Handle "INF" and "-INF" separately, since those values are defined in the XBRL Specification
                // but not supported by Convert.ToDouble().
                if (Value.Equals("INF") == true)
                {
                    thisTypedValue = Double.PositiveInfinity;
                }
                else if (Value.Equals("-INF") == true)
                {
                    thisTypedValue = Double.NegativeInfinity;
                }
                else
                {
                    thisTypedValue = Convert.ToDouble(this.InnerText, CultureInfo.InvariantCulture);
                }
                return;
            }
            if (nodeType is Xsd.Boolean)
            {
                // The explicit checks for "1" and "0" are in place to satisfy conformance test
                // 331-equivalentRelationships-instance-13.xml. Convert.ToBoolean() does not convert these values
                //to Booleans.
                if (this.InnerText.Equals("1") == true)
                    thisTypedValue = true;
                else if (this.InnerText.Equals("0") == true)
                    thisTypedValue = false;
                else
                    thisTypedValue = Convert.ToBoolean(this.InnerText, CultureInfo.InvariantCulture);
                return;
            }
            thisTypedValue = this.InnerText;
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
            var thisTypedValue = this.GetTypedValue(containingFragment);
            var otherTypedValue = otherNode.GetTypedValue(containingFragment);
            if ((thisTypedValue == null) && (otherTypedValue == null))
            {
                return this.InnerText.Equals(otherNode.Value);
            }
            if ((thisTypedValue == null) || (otherTypedValue == null))
            {
                return false;
            }
            if (thisTypedValue.GetType() == otherTypedValue.GetType())
            {
                return thisTypedValue.Equals(otherTypedValue);
            }
            return false;
        }
    }
}
