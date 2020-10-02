using JeffFerguson.Gepsio.Xml.Interfaces;
using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A simple utilitiy class for processing data found in schemaLocation attributes. These attributes
    /// can be found in XBRL root nodes as well as linkbase document root nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Schema location attributes look something like this:
    /// </para>
    /// <para>
    /// xsi:schemaLocation="http://www.xbrl.org/2003/linkbase 
    /// http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd
    /// http://xbrl.org/conformance/example
    /// 331-equivalentRelationships-01.xsd"
    /// </para>
    /// <para>
    /// The attribute value is a whitespace-separated list of strings. The value of the list should be in
    /// sets of pairs; therefore, the number of strings in the list should be an even number, since eat set is
    /// made up of two strings. The first string in each set is the namespace to be applied to the referenced
    /// schema, and the second string in each set is the location of the referenced schema.
    /// </para>
    /// </remarks>
    internal static class SchemaLocationAttributeProcessor
    {
        /// <summary>
        /// Process a node by looking for a schemaLocation attribute and processing it if it is found.
        /// </summary>
        /// <param name="node">
        /// A node which may contain a schemaLocation attribute.
        /// </param>
        /// <param name="fragment">
        /// The XBRL fragment containing the node.
        /// </param>
        internal static void Process(INode node, XbrlFragment fragment)
        {
            foreach (IAttribute currentAttribute in node.Attributes)
            {
                if ((currentAttribute.NamespaceURI.Equals(XbrlDocument.XmlSchemaInstanceUri) == true) && (currentAttribute.LocalName.Equals("schemaLocation") == true))
                {
                    var attributeValue = currentAttribute.Value.Trim();
                    if (string.IsNullOrEmpty(attributeValue) == false)
                    {
                        ProcessSchemaLocationAttributeValue(attributeValue, fragment);
                    }
                }
            }
        }

        /// <summary>
        /// Process a value found in a schemaLocation attribute.
        /// </summary>
        /// <remarks>
        /// This string is formatted as a set of whitespace-delimited pairs. The first URI reference in each pair is a namespace name,
        /// and the second is the location of a schema that describes that namespace.
        /// </remarks>
        /// <param name="schemaLocationAttributeValue">
        /// The value of a schemaLocation attribute.
        /// </param>
        /// <param name="containingFragment">
        /// The XBRL fragment containing the attribute.
        /// </param>
        private static void ProcessSchemaLocationAttributeValue(string schemaLocationAttributeValue, XbrlFragment containingFragment)
        {
            var NamespacesAndLocations = schemaLocationAttributeValue.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < NamespacesAndLocations.Length; index += 2)
            {
                ProcessSchemaNamespaceAndLocation(NamespacesAndLocations[index], NamespacesAndLocations[index + 1], containingFragment);
            }
        }

        private static void ProcessSchemaNamespaceAndLocation(string schemaNamespace, string schemaLocation, XbrlFragment containingFragment)
        {
            foreach(var currentSchema in containingFragment.Schemas)
            {
                if(currentSchema.TargetNamespace.Equals(schemaNamespace) == true)
                {
                    return;
                }
                if (currentSchema.TargetNamespaceAlias.Equals(schemaNamespace) == true)
                {
                    return;
                }
            }
            var newSchema = new XbrlSchema(containingFragment, schemaLocation, string.Empty);
            if (newSchema.SchemaRootNode != null)
            {
                containingFragment.Schemas.Add(newSchema);
            }
        }
    }
}
