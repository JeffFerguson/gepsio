using System.Globalization;
using System.Collections.Generic;
using System.Text;
using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A definition of a unit of measure used by numeric or fractional facts within the
    /// XBRL document. XBRL allows more complex units to be defined if necessary. Facts
    /// of a monetary nature must use a unit from the ISO 4217 namespace.
    /// </summary>
    public class Unit
    {
        private INode thisUnitNode;
        private List<QualifiedName> thisRatioNumeratorQualifiedNames;
        private List<QualifiedName> thisRatioDenominatorQualifiedNames;

        /// <summary>
        /// The ID of this unit.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// A collection of <see cref="QualifiedName"/> objects representing the set of measure qualified names for this unit.
        /// </summary>
        public List<QualifiedName> MeasureQualifiedNames { get; private set; }

        /// <summary>
        /// True if the unit's ISO 4217 code is valid; false otherwise.
        /// </summary>
        public bool IsIso4217CodeValid { get; private set; }

        /// <summary>
        /// Describes whether or not this unit represents a ratio. Returns true if this unit represents a ratio.
        /// Returns false if this unit does not represent a ratio.
        /// </summary>
        public bool Ratio { get; private set; }

        /// <summary>
        /// The <see cref="XbrlFragment"/> which contains the unit.
        /// </summary>
        public XbrlFragment Fragment { get; private set; }

        private INamespaceManager namespaceManager;

        internal Unit(XbrlFragment fragment, INode UnitNode, INamespaceManager namespaceManager)
        {
            this.Fragment = fragment;
            this.IsIso4217CodeValid = false;
            thisUnitNode = UnitNode;
            this.Id = thisUnitNode.Attributes["id"].Value;
            this.MeasureQualifiedNames = new List<QualifiedName>();
            this.Ratio = false;
            thisRatioNumeratorQualifiedNames = new List<QualifiedName>();
            thisRatioDenominatorQualifiedNames = new List<QualifiedName>();
            this.namespaceManager = namespaceManager;
            foreach (INode CurrentChild in UnitNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    this.MeasureQualifiedNames.Add(new QualifiedName(CurrentChild, namespaceManager));
                else if (CurrentChild.LocalName.Equals("divide") == true)
                {
                    ProcessDivideChildElement(CurrentChild);
                    CheckForMeasuresCommonToNumeratorsAndDenominators();
                    this.Ratio = true;
                }
            }
        }

        private void CheckForMeasuresCommonToNumeratorsAndDenominators()
        {
            foreach (QualifiedName CurrentNumeratorMeasure in thisRatioNumeratorQualifiedNames)
            {
                if (thisRatioDenominatorQualifiedNames.Contains(CurrentNumeratorMeasure) == true)
                {
                    string MessageFormat = AssemblyResources.GetName("UnitRatioUsesSameMeasureInNumeratorAndDenominator");
                    StringBuilder MessageFormatBuilder = new StringBuilder();
                    MessageFormatBuilder.AppendFormat(MessageFormat, this.Id, CurrentNumeratorMeasure.ToString());
                    this.Fragment.AddValidationError(new UnitValidationError(this, MessageFormatBuilder.ToString()));
                    return;
                }
            }
        }

        private void ProcessDivideChildElement(INode UnitDivideNode)
        {
            foreach (INode CurrentChild in UnitDivideNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("unitNumerator") == true)
                    ProcessUnitNumerators(CurrentChild);
                else if (CurrentChild.LocalName.Equals("unitDenominator") == true)
                    ProcessUnitDenominators(CurrentChild);
            }
        }

        private void ProcessUnitDenominators(INode UnitDivideDenominatorNode)
        {
            foreach (INode CurrentChild in UnitDivideDenominatorNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisRatioDenominatorQualifiedNames.Add(new QualifiedName(CurrentChild, namespaceManager));
            }
        }

        private void ProcessUnitNumerators(INode UnitDivideNumeratorNode)
        {
            foreach (INode CurrentChild in UnitDivideNumeratorNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("measure") == true)
                    thisRatioNumeratorQualifiedNames.Add(new QualifiedName(CurrentChild, namespaceManager));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal void ValidateISO4217Code(string Iso4217Code)
        {
            string[] validIso4217Codes =
            {
                "ADF", "ADP", "AED", "AFA", "AFN", "ALL", "AMD", "ANG", "AOA", "AON", "AOR",
                "ARA", "ARL", "ARP", "ARS", "ATS", "AUD", "AWG", "AZM", "AZN", "BAD", "BAM",
                "BBD", "BDT", "BEF", "BGL", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOP",
                "BOV", "BRB", "BRC", "BRE", "BRL", "BRN", "BRR", "BRZ", "BSD", "BTN", "BWP",
                "BYB", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLE", "CLF", "CLP",
                "CNY", "COP", "COU", "CRC", "CSD", "CSK", "CUC", "CUP", "CVE", "CYP", "CZK",
                "DDM", "DEM", "DJF", "DKK", "DOP", "DZD", "ECS", "ECV", "EEK", "EGP", "ERN",
                "ESA", "ESB", "ESP", "ETB", "EUR", "FIM", "FJD", "FKP", "FRF", "GBP", "GEL",
                "GHC", "GHS", "GIP", "GMD", "GNE", "GNF", "GQE", "GRD", "GTQ", "GWP", "GYD",
                "HKD", "HNL", "HRD", "HRK", "HTG", "HUF", "IDR", "IEP", "ILP", "ILR", "ILS",
                "INR", "IQD", "IRR", "ISJ", "ISK", "ITL", "JMD", "JOD", "JPY", "KES", "KGS",
                "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAJ", "LAK", "LBP", "LKR",
                "LRD", "LSL", "LTL", "LUF", "LVL", "LYD", "MAD", "MAF", "MCF", "MDL", "MGA",
                "MGF", "MKD", "MKN", "MLF", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR", "MVQ",
                "MVR", "MWK", "MXN", "MXP", "MXV", "MYR", "MZM", "MZN", "NAD", "NFD", "NGN",
                "NIO", "NLG", "NOK", "NPR", "NZD", "OMR", "PAB", "PEH", "PEI", "PEN", "PGK",
                "PHP", "PKR", "PLN", "PLZ", "PTE", "PTP", "PYG", "QAR", "ROL", "RON", "RSD",
                "RUB", "RUR", "RWF", "SAR", "SBD", "SCR", "SDD", "SDG", "SDP", "SEK", "SGD",
                "SHP", "SIT", "SKK", "SLL", "SML", "SOS", "SRD", "SRG", "SSP", "STD", "SUR",
                "SVC", "SYP", "SZL", "THB", "TJR", "TJS", "TMM", "TMT", "TND", "TNF", "TOP",
                "TPE", "TRL", "TRY", "TTD", "TWD", "TZS", "UAH", "UAK", "UGS", "UGX", "USD",
                "USN", "USS", "UYI", "UYN", "UYU", "UZS", "VAL", "VEB", "VEF", "VND", "VUV",
                "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XEU",
                "XFO", "XFU", "XOF", "XPD", "XPF", "XPT", "XSU", "XTS", "XUA", "XXX", "YDD",
                "YER", "YUD", "YUG", "YUM", "YUN", "YUO", "YUR", "ZAL", "ZAR", "ZMK", "ZMW",
                "ZRN", "ZRZ", "ZWC", "ZWD", "ZWL", "ZWN", "ZWR"
            };

            foreach(var currentCode in validIso4217Codes)
            {
                if(currentCode.Equals(Iso4217Code) == true)
                {
                    this.IsIso4217CodeValid = true;
                    return;
                }
            }
            this.IsIso4217CodeValid = false;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal bool StructureEquals(Unit OtherUnit)
        {
            if (thisUnitNode == null)
                return false;
            if (OtherUnit.thisUnitNode == null)
                return false;
            if (thisUnitNode.StructureEquals(OtherUnit.thisUnitNode) == false)
                return false;
            if (this.Ratio == false)
                return NonRatioStructureEquals(OtherUnit);
            else
                return RatioStructureEquals(OtherUnit);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool RatioStructureEquals(Unit OtherUnit)
        {
            if (QualifiedNameListsStructureEquals(thisRatioNumeratorQualifiedNames, OtherUnit.thisRatioNumeratorQualifiedNames) == false)
                return false;
            if (QualifiedNameListsStructureEquals(thisRatioDenominatorQualifiedNames, OtherUnit.thisRatioDenominatorQualifiedNames) == false)
                return false;
            return true;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool NonRatioStructureEquals(Unit OtherUnit)
        {
            return QualifiedNameListsStructureEquals(MeasureQualifiedNames, OtherUnit.MeasureQualifiedNames);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private bool QualifiedNameListsStructureEquals(List<QualifiedName> List1, List<QualifiedName> List2)
        {
            if (List1.Count != List2.Count)
                return false;
            foreach (QualifiedName CurrentQualifiedName in List1)
            {
                if (List2.Contains(CurrentQualifiedName) == false)
                    return false;
            }
            return true;
        }
    }
}
