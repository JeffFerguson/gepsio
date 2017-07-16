
namespace JeffFerguson.Gepsio.Xsd
{
    internal class MaxLengthFacetDefinition : FacetDefinition
    {
        internal MaxLengthFacetDefinition() : base("minLength")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(NonNegativeInteger)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("fixed", typeof(Boolean), "false"));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
