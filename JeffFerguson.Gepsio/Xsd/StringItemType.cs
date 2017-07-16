
namespace JeffFerguson.Gepsio.Xsd
{
	/// <summary>
	/// An encapsulation of the XBRL complex type "stringItemType" as defined in the http://www.xbrl.org/2003/instance namespace.
	/// </summary>
    public class StringItemType : ComplexType
    {
        internal StringItemType()
            : base("stringItemType", new String(), new NonNumericItemAttributes())
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
        }
    }
}
