using JeffFerguson.Gepsio.Xsd;

namespace JeffFerguson.Gepsio
{
    internal class ItemAttributes : FactAttributes
    {
        internal ItemAttributes() : base()
        {
            AddAttribute(new Attribute("contextRef", typeof(IDREF), true));
        }
    }
}
