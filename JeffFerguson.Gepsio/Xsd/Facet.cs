using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio.Xsd
{
	/// <summary>
	/// The base class for the encapsulation of all XML schema facets defined in the http://www.w3.org/2001/XMLSchema namespace. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
	/// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
	/// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
	/// Gepsio's XML schema type system obsolete.
	/// </para>
	/// </remarks>
    public class Facet
    {
        private FacetDefinition thisDefinition;

		internal List<FacetProperty> Properties
		{
			get;
			private set;
		}

        internal static Facet CreateFacet(FacetDefinition Definition)
        {
            if (Definition is PatternFacetDefinition)
                return new PatternFacet(Definition);
            if (Definition is LengthFacetDefinition)
                return new LengthFacet(Definition);
            if (Definition is MaxLengthFacetDefinition)
                return new MaxLengthFacet(Definition);
            if (Definition is WhiteSpaceFacetDefinition)
                return new WhiteSpaceFacet(Definition);
            if (Definition is MinLengthFacetDefinition)
                return new WhiteSpaceFacet(Definition);
            if (Definition is EnumerationFacetDefinition)
                return new EnumerationFacet(Definition);
            string MessageFormat = AssemblyResources.GetName("FacetDefinitionNotSupportedForFacetCreation");
            StringBuilder MessageBuilder = new StringBuilder();
            MessageBuilder.AppendFormat(MessageFormat, Definition.GetType().ToString());
            //throw new XbrlException(MessageBuilder.ToString());
            return null;
        }

		/// <summary>
		/// Constructor for the <see cref="Facet"/> class.
		/// </summary>
		/// <param name="Definition">
		/// The definition of the facet to be used during the construction of the facet.
		/// </param>
        protected Facet(FacetDefinition Definition)
        {
            thisDefinition = Definition;
            this.Properties = new List<FacetProperty>();
        }

        internal void AddFacetProperty(FacetPropertyDefinition Definition, string Value)
        {
            this.Properties.Add(new FacetProperty(Definition, Value));
        }

        internal FacetProperty GetFacetProperty(string PropertyName)
        {
            foreach (FacetProperty CurrentFacetProperty in this.Properties)
            {
                if (CurrentFacetProperty.Definition.Name.Equals(PropertyName) == true)
                    return CurrentFacetProperty;
            }
            return null;
        }
    }
}
