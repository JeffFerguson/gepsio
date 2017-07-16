using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "string" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class String : AnySimpleType
    {
        /// <summary>
        /// The constructor for the class.
        /// </summary>
        public String() : base()
        {
        }

        internal String(INode StringRootNode) : base(StringRootNode)
        {
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
    }
}
