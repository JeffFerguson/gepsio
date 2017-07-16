using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "qualifiedName" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class QualifiedName : AnySimpleType
    {
        private string thisToStringValue;

        /// <summary>
        /// The local name portion of the qualified name.
        /// </summary>
        public string LocalName
        {
            get;
            private set;
        }

        /// <summary>
        /// The namespace of the qualified name.
        /// </summary>
        public string Namespace
        {
            get;
            private set;
        }

        /// <summary>
        /// The URI of the namespace of the qualified name.
        /// </summary>
        public string NamespaceUri
        {
            get;
            private set;
        }

        internal QualifiedName(INode QnameNode, INamespaceManager namespaceManager)
        {
            InitializeLocalNameAndNamespace(QnameNode);
            if (this.Namespace.Length > 0)
                InitializeNamespaceUri(QnameNode, namespaceManager);
        }

        private void InitializeNamespaceUri(INode QnameNode, INamespaceManager namespaceManager)
        {
            if (this.Namespace.Length == 0)
            {
                this.NamespaceUri = string.Empty;
                return;
            }
            string AttributeName = "xmlns:" + this.Namespace;
            var currentNode = QnameNode;
            while ((currentNode != null) && (string.IsNullOrEmpty(this.NamespaceUri) == true))
            {
                this.NamespaceUri = currentNode.GetAttributeValue(AttributeName);
                currentNode = currentNode.ParentNode;
            }
            if (string.IsNullOrEmpty(this.NamespaceUri) == true)
                this.NamespaceUri = namespaceManager.LookupNamespace(this.Namespace);
        }

        private void InitializeLocalNameAndNamespace(INode QnameNode)
        {
            thisToStringValue = QnameNode.InnerText;
            string[] InnerTextSplit = QnameNode.InnerText.Split(':');
            if (InnerTextSplit.Length == 1)
            {
                this.Namespace = string.Empty;
                this.LocalName = InnerTextSplit[0];
            }
            else
            {
                this.Namespace = InnerTextSplit[0];
                this.LocalName = InnerTextSplit[1];
            }
        }

        /// <summary>
        /// Compares the current qualified name object with the supplied qualified name object.
        /// </summary>
        /// <param name="obj">
        /// The object which should be compared to the current qualified name object.
        /// </param>
        /// <returns>
        /// True if the supplied object is equal to the current qualified name object. False
        /// if the supplied object is not equal to the current qualified name object.
        /// </returns>
        public override bool Equals(object obj)
        {
            if ((obj is QualifiedName) == false)
                return false;
            QualifiedName OtherObj = obj as QualifiedName;
            if(this.LocalName.Equals(OtherObj.LocalName) == false)
                return false;
            if(this.Namespace.Equals(OtherObj.Namespace) == false)
                return false;

            // At this point, the equality of the namespace URIs need to be checked.
            // Start by checking the various null conditions.

            if ((this.NamespaceUri == null) && (OtherObj.NamespaceUri == null))
                return true;
            if ((this.NamespaceUri != null) && (OtherObj.NamespaceUri == null))
                return false;
            if ((this.NamespaceUri == null) && (OtherObj.NamespaceUri != null))
                return false;

            // At this point, we know that both namespace URIs are non-null. Check
            // their values for equality.

            if(this.NamespaceUri.Equals(OtherObj.NamespaceUri) == false)
                return false;
            return true;
        }

        /// <summary>
        /// Calculates a hash code for the current object.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.LocalName.GetHashCode();
        }

        /// <summary>
        /// Represents the qualified name object as a string.
        /// </summary>
        /// <returns>
        /// The string representation of the qualified name object.
        /// </returns>
        public override string ToString()
        {
            return thisToStringValue;
        }
    }
}
