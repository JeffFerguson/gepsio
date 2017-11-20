using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An XBRL fact with a single value.
    /// </summary>
    public class Item : Fact
    {
        private string thisPrecisionAttributeValue;
        private string thisDecimalsAttributeValue;
        private XbrlSchema thisSchema;
        private bool thisRoundedValueCalculated;
        private double thisRoundedValue;

        /// <summary>
        /// The context associated with this item.
        /// </summary>
        public Context ContextRef { get; internal set; }

        /// <summary>
        /// The name of the context reference associated with this item.
        /// </summary>
        public string ContextRefName { get; private set; }

        /// <summary>
        /// The unit associated with this item.
        /// </summary>
        public Unit UnitRef { get; internal set; }

        /// <summary>
        /// The name of the unit reference associated with this item.
        /// </summary>
        public string UnitRefName { get; private set; }

        /// <summary>
        /// The precision of this item.
        /// </summary>
        public int Precision { get; private set; }

        /// <summary>
        /// True if this item has infinite precision. False if this item does not have infinite precision.
        /// </summary>
        public bool InfinitePrecision { get; private set; }

        /// <summary>
        /// True if this item has inferred precision. False if this item does not have inferred precision.
        /// </summary>
        public bool PrecisionInferred { get; private set; }

        /// <summary>
        /// The decimals value of this item.
        /// </summary>
        public int Decimals { get; private set; }

        /// <summary>
        /// True if this item has infinite decimals. False if this item does not have infinite decimals.
        /// </summary>
        public bool InfiniteDecimals { get; private set; }

        /// <summary>
        /// True if this item has a specified precision. False if this fact does not have a specified precision.
        /// </summary>
        public bool PrecisionSpecified { get; private set; }

        /// <summary>
        /// True if this item has a specified decimals value. False if this fact does not have a specified decimals value.
        /// </summary>
        public bool DecimalsSpecified { get; private set; }

        /// <summary>
        /// The value of this fact.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The namespace for this item.
        /// </summary>
        public string Namespace
        {
            get
            {
                return thisFactNode.NamespaceURI;
            }
        }

        /// <summary>
        /// The schema element that defines this item.
        /// </summary>
        public Element SchemaElement { get; private set; }

        /// <summary>
        /// True if this fact has a nil value. False if this fact does not have a nil value.
        /// </summary>
        public bool NilSpecified { get; private set; }

        /// <summary>
        /// The type of this item.
        /// </summary>
        internal ISchemaType Type { get; private set; }

        /// <summary>
        /// Returns the rounded value of the fact's actual value. The rounded value is calculated from the precision
        /// (or imferred precision) of the fact's actual value.
        /// </summary>
        public double RoundedValue
        {
            get
            {
                if (thisRoundedValueCalculated == false)
                {
                    thisRoundedValue = GetRoundedValue();
                    thisRoundedValueCalculated = true;
                }
                return thisRoundedValue;
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal Item(XbrlFragment ParentFragment, INode ItemNode) : base(ParentFragment, ItemNode)
        {
            this.ContextRefName = thisFactNode.GetAttributeValue("contextRef");
            this.UnitRefName = thisFactNode.GetAttributeValue("unitRef");
            thisRoundedValueCalculated = false;
            this.NilSpecified = false;
            string NilValue = thisFactNode.GetAttributeValue(XbrlSchema.XmlSchemaInstanceNamespaceUri, "nil");
            if (NilValue.Equals("true", StringComparison.CurrentCultureIgnoreCase) == true)
                this.NilSpecified = true;
            GetSchemaElementFromSchema();
            this.Value = thisFactNode.InnerText;
            if (SchemaElement.SubstitutionGroup == Element.ElementSubstitutionGroup.Item)
                SetItemType(SchemaElement.TypeName);
            SetDecimals();
            SetPrecision();
            if (string.IsNullOrEmpty(thisPrecisionAttributeValue) == true)
                InferPrecision();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetPrecision()
        {
            PrecisionSpecified = false;
            thisPrecisionAttributeValue = thisFactNode.GetAttributeValue("precision");
            if (string.IsNullOrEmpty(thisPrecisionAttributeValue) == true)
            {

                // There is no "precision" attribute on the item itself, but one may be specified
                // in the schema definition for the type. Check there if the type is a complex
                // type.

                if(this.Type?.IsComplex == true)
                {
                    var precisionAttribute = this.Type.GetAttribute("precision");
                    if (precisionAttribute != null)
                        thisPrecisionAttributeValue = precisionAttribute.FixedValue;
                }
            }
            if (string.IsNullOrEmpty(thisPrecisionAttributeValue) == false)
            {
                PrecisionSpecified = true;
                this.PrecisionInferred = false;
                if (thisPrecisionAttributeValue.Equals("INF") == true)
                {
                    this.InfinitePrecision = true;
                    this.Precision = 0;
                }
                else
                {
                    this.InfinitePrecision = false;
                    this.Precision = Convert.ToInt32(thisPrecisionAttributeValue);
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetDecimals()
        {
            DecimalsSpecified = false;
            thisDecimalsAttributeValue = thisFactNode.GetAttributeValue("decimals");
            if (string.IsNullOrEmpty(thisDecimalsAttributeValue) == true)
            {

                // There is no "decimals" attribute on the item itself, but one may be specified
                // in the schema definition for the type. Check there if the type is a complex
                // type.

                if (this.Type?.IsComplex == true)
                {
                    var decimalsAttribute = this.Type.GetAttribute("decimals");
                    if (decimalsAttribute != null)
                        thisDecimalsAttributeValue = decimalsAttribute.FixedValue;
                }
            }
            if (string.IsNullOrEmpty(thisDecimalsAttributeValue) == false)
            {
                DecimalsSpecified = true;
                if (thisDecimalsAttributeValue.Equals("INF") == true)
                {
                    this.InfiniteDecimals = true;
                    this.Decimals = 0;
                }
                else
                {
                    this.InfiniteDecimals = false;
                    this.Decimals = Convert.ToInt32(thisDecimalsAttributeValue);
                }
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void InferPrecision()
        {
            this.PrecisionInferred = true;
            int CalculationPart1Value = GetNumberOfDigitsToLeftOfDecimalPoint();
            if (CalculationPart1Value == 0)
                CalculationPart1Value = GetNegativeNumberOfLeadingZerosToRightOfDecimalPoint();
            int CalculationPart2Value = GetExponentValue();
            int CalculationPart3Value = this.Decimals;
            this.Precision = CalculationPart1Value + CalculationPart2Value + CalculationPart3Value;
            if (this.Precision < 0)
                this.Precision = 0;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void GetSchemaElementFromSchema()
        {
            thisSchema = thisParentFragment.Schemas.GetSchemaFromTargetNamespace(this.Namespace, thisParentFragment);
            if (thisSchema == null)
            {
                if (thisParentFragment.Schemas.Count == 0)
                {
                    string MessageFormat = AssemblyResources.GetName("NoSchemasForFragment");
                    StringBuilder MessageFormatBuilder = new StringBuilder();
                    MessageFormatBuilder.AppendFormat(MessageFormat);
                    thisParentFragment.AddValidationError(new ItemValidationError(this, MessageFormatBuilder.ToString()));
                }
                else
                {
                    string MessageFormat = AssemblyResources.GetName("NoSchemasForNamespace");
                    StringBuilder MessageFormatBuilder = new StringBuilder();
                    MessageFormatBuilder.AppendFormat(MessageFormat, this.Name, this.Namespace);
                    thisParentFragment.AddValidationError(new ItemValidationError(this, MessageFormatBuilder.ToString()));
                }
            }
            this.SchemaElement = thisParentFragment.Schemas.GetElement(this.Name);
            if (this.SchemaElement == null)
            {
                string MessageFormat = AssemblyResources.GetName("CannotFindFactElementInSchema");
                StringBuilder MessageFormatBuilder = new StringBuilder();
                MessageFormatBuilder.AppendFormat(MessageFormat, this.Name, thisSchema.Path);
                thisParentFragment.AddValidationError(new ItemValidationError(this, MessageFormatBuilder.ToString()));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void SetItemType(IQualifiedName ItemTypeValue)
        {
            this.Type = null;
            if(thisSchema != null)
                this.Type = thisSchema.GetXmlSchemaType(ItemTypeValue);
            if (this.Type == null)
            {
                var MessageFormatBuilder = new StringBuilder();
                if (thisSchema == null)
                {
                    string MessageFormat = AssemblyResources.GetName("NoSchemaForElementItemType");
                    MessageFormatBuilder.AppendFormat(MessageFormat, ItemTypeValue.Name, this.Name);
                }
                else
                {
                    string MessageFormat = AssemblyResources.GetName("InvalidElementItemType");
                    MessageFormatBuilder.AppendFormat(MessageFormat, thisSchema.Path, ItemTypeValue.Name, this.Name);
                }
                thisParentFragment.AddValidationError(new ItemValidationError(this, MessageFormatBuilder.ToString()));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private string GetLocalName(string FullName)
        {
            if (FullName == null)
                throw new NotSupportedException("null full names not supported in Fact.GetLocalName()");
            int ColonIndex = FullName.IndexOf(':');
            if (ColonIndex == -1)
                return FullName;
            return FullName.Substring(ColonIndex + 1);
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a monetary type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a monetary type and false otherwise.
        /// </returns>
        internal bool IsMonetary()
        {
            return TypeNameContains("monetary");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a pure type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a pure type and false otherwise.
        /// </returns>
        internal bool IsPure()
        {
            return TypeNameContains("pure");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a shares type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a shares type and false otherwise.
        /// </returns>
        internal bool IsShares()
        {
            return TypeNameContains("shares");
        }

        /// <summary>
        /// Determines whether or not the item type for this fact is a decimal type.
        /// </summary>
        /// <returns>
        /// True if the type for this fact is a decimal type and false otherwise.
        /// </returns>
        internal bool IsDecimal()
        {
            return TypeNameContains("decimal");
        }

        /// <summary>
        /// Determines whether or not the fact's item type is of the given type.
        /// </summary>
        /// <returns>
        /// True if the type is of the given type and false otherwise.
        /// </returns>
        private bool TypeNameContains(string TypeName)
        {
            try
            {
                return TypeNameContains(TypeName, this.Type);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether or not the supplied item type is of the given type.
        /// </summary>
        /// <returns>
        /// True if the type is of the given type and false otherwise.
        /// </returns>
        private bool TypeNameContains(string TypeName, ISchemaType CurrentType)
        {
            if (CurrentType == null)
                return false;
            if (CurrentType.Name.Contains(TypeName) == true)
                return true;
            if (CurrentType.IsComplex == true)
            {
                if (CurrentType.DerivedByRestriction == true)
                {
                    return TypeNameContains(TypeName, CurrentType.BaseSchemaType);
                }
            }
            return false;
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Context Equal (c-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ContextEquals(Item OtherFact)
        {
            if (Object.ReferenceEquals(this.ContextRef, OtherFact.ContextRef) == true)
                return true;
            return this.ContextRef.StructureEquals(OtherFact.ContextRef);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Parent Equal (p-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool ParentEquals(Item OtherFact)
        {
            if (thisFactNode == null)
                return false;
            return thisFactNode.ParentEquals(OtherFact.thisFactNode);
        }

        //------------------------------------------------------------------------------------
        // Returns true if this Fact is Unit Equal (u-equal) to a supplied fact, and false
        // otherwise. See section 4.10 of the XBRL 2.1 spec for more information.
        //------------------------------------------------------------------------------------
        internal bool UnitEquals(Item OtherFact)
        {
            if (OtherFact == null)
                return false;
            if (this.UnitRef == null)
                return true;
            if (OtherFact.UnitRef == null)
                return true;
            return this.UnitRef.StructureEquals(OtherFact.UnitRef);
        }

        /// <summary>
        /// Calculates the number of digits to the left of the decimal point. Leading zeros are not counted.
        /// </summary>
        /// <returns>
        /// The number of digits to the left of the decimal point. Leading zeros are not counted.
        /// </returns>
        private int GetNumberOfDigitsToLeftOfDecimalPoint()
        {
            if (this.Value == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            string WithoutLeadingZeros = ParsedValue[0].TrimStart(new char[] { '0' });
            return WithoutLeadingZeros.Length;
        }

        /// <summary>
        /// Calculates the negative number of leading zeros to the right of the decimal point.
        /// </summary>
        /// <returns>
        /// The negative number of leading zeros to the right of the decimal point.
        /// </returns>
        private int GetNegativeNumberOfLeadingZerosToRightOfDecimalPoint()
        {
            if (this.Value == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            string ValueToTheRightOfTheDecimal = ParsedValue[1];
            if (string.IsNullOrEmpty(ValueToTheRightOfTheDecimal))
                return 0;
            int NumberOfLeadingZeros = 0;
            int Index = 0;
            while (Index < ValueToTheRightOfTheDecimal.Length)
            {
                if (ValueToTheRightOfTheDecimal[Index] == '0')
                {
                    NumberOfLeadingZeros++;
                    Index++;
                }
                else
                {
                    Index = ValueToTheRightOfTheDecimal.Length;
                    break;
                }
            }
            return -NumberOfLeadingZeros;
        }

        /// <summary>
        /// Calculates the value of the exponent in the lexical representation of the fact value.
        /// </summary>
        /// <returns>
        /// The value of the exponent in the lexical representation of the fact value.
        /// </returns>
        private int GetExponentValue()
        {
            if (this.Value == null)
                return 0;
            string[] ParsedValue = ParseValueIntoComponentParts();
            if (string.IsNullOrEmpty(ParsedValue[2]))
                return 0;
            int ExponentValue;
            bool Success = int.TryParse(ParsedValue[2], out ExponentValue);
            if (Success == true)
                return ExponentValue;
            return 0;
        }

        /// <summary>
        /// <para>
        /// Parses the fact value into three main parts:
        /// </para>
        /// <list>
        /// <item>
        /// Values to the left of the decimal point
        /// </item>
        /// <item>
        /// Values to the right of the decimal point
        /// </item>
        /// <item>
        /// Exponent value
        /// </item>
        /// </list>
        /// </summary>
        /// <para>
        /// Some of these values may be empty if the original value did not carry all of these components.
        /// </para>
        /// <returns>A string array of length 3. Item 0 contains the value before the decimal point,
        /// item 1 contains the value after the decimal point, and item 2 contains the exponent value. If any
        /// of these elements are not available in the original value, then their individual value within the
        /// array will be empty.</returns>
        private string[] ParseValueIntoComponentParts()
        {
            return ParseValueIntoComponentParts(this.Value);
        }

        /// <summary>
        /// <para>
        /// Parses a string representation of a fact value into three main parts:
        /// </para>
        /// <list>
        /// <item>
        /// Values to the left of the decimal point
        /// </item>
        /// <item>
        /// Values to the right of the decimal point
        /// </item>
        /// <item>
        /// Exponent value
        /// </item>
        /// </list>
        /// </summary>
        /// <para>
        /// Some of these values may be empty if the original value did not carry all of these components.
        /// </para>
        /// <returns>A string array of length 3. Item 0 contains the value before the decimal point,
        /// item 1 contains the value after the decimal point, and item 2 contains the exponent value. If any
        /// of these elements are not available in the original value, then their individual value within the
        /// array will be empty.</returns>
        private string[] ParseValueIntoComponentParts(string OriginalValue)
        {
            string[] ArrayToReturn = new string[3];

            string[] StringsAfterExponentSplit = OriginalValue.Split(new char[] { 'e', 'E' });
            if (StringsAfterExponentSplit.Length == 2)
                ArrayToReturn[2] = StringsAfterExponentSplit[1];
            string NumericValue = StringsAfterExponentSplit[0];
            string[] StringsAfterDecimalSplit = NumericValue.Split(new char[] { '.' });
            ArrayToReturn[0] = StringsAfterDecimalSplit[0];
            if (StringsAfterDecimalSplit.Length == 2)
                ArrayToReturn[1] = StringsAfterDecimalSplit[1];
            return ArrayToReturn;
        }

        private double GetRoundedValue()
        {
            double RoundedValue = Convert.ToDouble(this.Value);
            return Round(RoundedValue);
        }

        /// <summary>
        ///  Round a given value using the Precision already available in the fact.
        /// </summary>
        /// <param name="OriginalValue"></param>
        /// <returns></returns>
        public double Round(double OriginalValue)
        {

            // If the item has specified either infinite precision or infinite decimals, then no rounding will take place
            // and the original value will be returned.

            if ((InfinitePrecision == true) || (InfiniteDecimals == true))
                return OriginalValue;

            // Break the original value into three parts: (1) values to the left of the decimal, (2) values to the right of the decimal,
            // and (3) the exponent value. Remember that one or more of these, particularly parts (2) and (3), may be empty or null.

            double RoundedValue = OriginalValue;

            string OriginalValueAsString = OriginalValue.ToString();
            string[] ComponentParts = ParseValueIntoComponentParts(OriginalValueAsString);
            ComponentParts[0] = ComponentParts[0].TrimStart(new char[] { '0' });
            if (string.IsNullOrEmpty(ComponentParts[1]) == false)
                ComponentParts[1] = ComponentParts[1].TrimEnd(new char[] { '0' });
            if (Precision > ComponentParts[0].Length)
            {

                // In this case, the Precision value is greater than the length of the portion of the value to the left of the decimal.
                // An example of this may be a precision of 5 and a value of "123.456". The length of the portion of the value to the left
                // of the decimal ("123") is 3 and the precision is 5. In this situation, we will need to round to a number of places
                // to the right of the decimal. Since the precision is 5, and since three of those five will be used for the left of the decimal,
                // then we are left with two places to round to the right of the decimal.

                RoundedValue = Math.Round(RoundedValue, Precision - ComponentParts[0].Length);
            }
            else if (Precision == ComponentParts[0].Length)
            {
                // In this case, the Precision value is equal to the length of the portion of the value to the left of the decimal. In this case,
                // we'll simply round to the nearest integer.

                RoundedValue = Math.Round(RoundedValue);
            }
            else
            {

                // In this case, the Precision value is less than the length of the portion of the value to the left of the decimal. We need, therefore,
                // to round a whole number -- that part of the number stored as the first component part.

                double PowerOfTen = Math.Pow(10.0, (double)(ComponentParts[0].Length - Precision));
                RoundedValue = RoundedValue / PowerOfTen;
                RoundedValue = Math.Round(RoundedValue);
                RoundedValue = RoundedValue * PowerOfTen;
            }
            return RoundedValue;
        }
    }
}
