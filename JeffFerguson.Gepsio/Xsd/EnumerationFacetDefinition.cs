
namespace JeffFerguson.Gepsio.Xsd
{
    internal class EnumerationFacetDefinition : FacetDefinition
    {
        internal EnumerationFacetDefinition() : base("enumeration")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(String)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
