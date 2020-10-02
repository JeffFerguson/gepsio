using System.Xml;

namespace JeffFerguson.Gepsio
{
    internal static class XmlUtilities
    {
        //------------------------------------------------------------------------------------
        // Finds an attribute with the given name in the given XML node and returns the
        // attribute of the value. Returns an empty string if no such attribute exists. Works
        // for local names as well as names qualified with a namespace identifier.
        //------------------------------------------------------------------------------------
        internal static string GetAttributeValue(XmlNode Node, string AttributeName)
        {
            bool NameIncludesNamespaceId;

            if (AttributeName.IndexOf(':') == -1)
                NameIncludesNamespaceId = false;
            else
                NameIncludesNamespaceId = true;
            if (Node == null)
                return string.Empty;
            if (Node.Attributes == null)
                return string.Empty;
            foreach (XmlAttribute CurrentAttribute in Node.Attributes)
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

        //------------------------------------------------------------------------------------
        // Finds an attribute with the given namespace URI and the given local name in the
        // given XML node and returns the attribute of the value. Returns an empty string if
        // no such attribute exists.
        //------------------------------------------------------------------------------------
        internal static string GetAttributeValue(XmlNode Node, string AttributeNamespaceUri, string AttributeLocalName)
        {
            if (Node == null)
                return string.Empty;
            if (Node.Attributes == null)
                return string.Empty; 
            foreach (XmlAttribute CurrentAttribute in Node.Attributes)
            {
                if (CurrentAttribute.NamespaceURI.Equals(AttributeNamespaceUri) == true)
                {
                    if (CurrentAttribute.LocalName.Equals(AttributeLocalName) == true)
                        return CurrentAttribute.Value;
                }
            }
            return string.Empty;
        }
    }
}
