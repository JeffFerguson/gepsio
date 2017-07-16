using System.Text;

namespace JeffFerguson.Gepsio.Xsd
{
    internal class SharesItemType : ComplexType
    {
        internal SharesItemType()
            : base("sharesItemType", new Shares(null), new NumericItemAttributes())
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
            bool SharesMeasureFound = true;
            string UnitMeasureLocalName = string.Empty;
            Unit UnitReference = FactToValidate.UnitRef;
            if (UnitReference.MeasureQualifiedNames.Count != 1)
                SharesMeasureFound = false;
            if (SharesMeasureFound == true)
            {
                UnitMeasureLocalName = UnitReference.MeasureQualifiedNames[0].LocalName;
                SharesMeasureFound = UnitMeasureLocalName.Equals("shares");
            }
            if (SharesMeasureFound == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SharesItemTypeUnitLocalNameNotShares");
                MessageBuilder.AppendFormat(StringFormat, FactToValidate.Name, UnitReference.Id, UnitMeasureLocalName);
                //throw new XbrlException(MessageBuilder.ToString());
            }
        }
    }
}
