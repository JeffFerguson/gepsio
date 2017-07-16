using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Text;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "decimal" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class Decimal : AnySimpleType
    {
        internal Decimal(INode StringRootNode) : base(StringRootNode)
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);

            if (FactToValidate.NilSpecified == true)
                ValidateNilFact(FactToValidate);
            else
                ValidateNonNilFact(FactToValidate);
        }

        private void ValidateNilFact(Item FactToValidate)
        {
            if ((FactToValidate.PrecisionSpecified == true) || (FactToValidate.DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NilNumericFactWithSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                //throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }

        private void ValidateNonNilFact(Item FactToValidate)
        {
            if ((FactToValidate.PrecisionSpecified == false) && (FactToValidate.DecimalsSpecified == false))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithoutSpecifiedPrecisionOrDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                //throw new XbrlException(MessageFormatBuilder.ToString());
            }
            if ((FactToValidate.PrecisionSpecified == true) && (FactToValidate.DecimalsSpecified == true))
            {
                string MessageFormat = AssemblyResources.GetName("NumericFactWithSpecifiedPrecisionAndDecimals");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, FactToValidate.Name, FactToValidate.Id);
                //throw new XbrlException(MessageFormatBuilder.ToString());
            }
        }
    }
}
