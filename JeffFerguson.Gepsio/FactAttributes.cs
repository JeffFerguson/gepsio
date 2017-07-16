using JeffFerguson.Gepsio.Xsd;
namespace JeffFerguson.Gepsio
{
    internal class FactAttributes : AttributeGroup
    {
        internal FactAttributes() : base()
        {
            AddAttribute(new Attribute("id", typeof(ID), false));
        }
    }
}
