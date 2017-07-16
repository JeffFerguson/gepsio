
namespace JeffFerguson.Gepsio.Xsd
{
    internal class WhiteSpaceFacetDefinition : FacetDefinition
    {
        internal WhiteSpaceFacetDefinition() : base("whiteSpace")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(String)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("fixed", typeof(Boolean), "false"));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
