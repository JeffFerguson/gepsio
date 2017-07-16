using System.Text;

namespace JeffFerguson.Gepsio.Xsd
{
    internal class PureItemType : ComplexType
    {
        internal PureItemType()
            : base("pureItemType", new Pure(null), new NumericItemAttributes())
        {
        }

        internal override void ValidateFact(Item FactToValidate)
        {
            base.ValidateFact(FactToValidate);
            string UnitMeasureLocalName = string.Empty;
            Unit UnitReference = FactToValidate.UnitRef;
            bool PureMeasureFound = true;
            if (UnitReference.MeasureQualifiedNames.Count != 1)
                PureMeasureFound = false;
            if (PureMeasureFound == true)
            {
                UnitMeasureLocalName = UnitReference.MeasureQualifiedNames[0].LocalName;
                PureMeasureFound = UnitMeasureLocalName.Equals("pure");
            }
            if (PureMeasureFound == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("PureItemTypeUnitLocalNameNotPure");
                MessageBuilder.AppendFormat(StringFormat, FactToValidate.Name, UnitReference.Id, UnitMeasureLocalName);
                //throw new XbrlException(MessageBuilder.ToString());
            }
        }
    }
}
