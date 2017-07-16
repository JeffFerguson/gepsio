using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "simpleType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class Token : NormalizedString
    {
        private List<string> thisEnumerationValues;

        /// <summary>
        /// A collection of string-based enumeration values.
        /// </summary>
        public List<string> EnumerationValues
        {
            get
            {
                CollectEnumerationValues();
                return thisEnumerationValues;
            }
        }

        internal Token(INode TokenRootNode) : base(TokenRootNode)
        {
            thisEnumerationValues = null;
        }

        /// <summary>
        /// Adds constraining facet definitions for this schema type.
        /// </summary>
        protected override void AddConstrainingFacetDefinitions()
        {
            AddConstrainingFacetDefinition(new LengthFacetDefinition());
            AddConstrainingFacetDefinition(new MinLengthFacetDefinition());
            AddConstrainingFacetDefinition(new MaxLengthFacetDefinition());
            AddConstrainingFacetDefinition(new PatternFacetDefinition());
            AddConstrainingFacetDefinition(new EnumerationFacetDefinition());
            AddConstrainingFacetDefinition(new WhiteSpaceFacetDefinition());
        }

        private void CollectEnumerationValues()
        {
            if (thisEnumerationValues != null)
                return;
            thisEnumerationValues = new List<string>();
            List<Facet> EnumerationFacets = GetFacets(typeof(EnumerationFacet));
            foreach (Facet EnumerationFacet in EnumerationFacets)
            {
                FacetProperty ValueProperty = EnumerationFacet.GetFacetProperty("value");
                if (ValueProperty != null)
                {
                    String StringValueProperty = ValueProperty.Value as String;
                    thisEnumerationValues.Add(StringValueProperty.ValueAsString);
                }
            }
        }
    }
}
