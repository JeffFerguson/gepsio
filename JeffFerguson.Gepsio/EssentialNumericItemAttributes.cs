using JeffFerguson.Gepsio.Xsd;

namespace JeffFerguson.Gepsio
{
    internal class EssentialNumericItemAttributes : ItemAttributes
    {
        internal EssentialNumericItemAttributes() : base()
        {
            AddAttribute(new Attribute("unitRef", typeof(IDREF), true));
        }
    }
}
