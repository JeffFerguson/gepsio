
namespace JeffFerguson.Gepsio.Xsd
{
    internal class TokenItemType : ComplexType
    {
        internal TokenItemType()
            : base("tokenItemType", new Token(null), new NonNumericItemAttributes())
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
        }
    }
}
