using JeffFerguson.Gepsio.Xsd;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio.Validators.Xbrl2Dot1
{
    /// <summary>
    /// Validates summation concepts found in an <see cref="XbrlFragment"/> instance.
    /// </summary>
    public class SummationConceptValidator
    {
        /// <summary>
        /// The XBRL fragment whose summation concepts have been validated by the validator.
        /// </summary>
        public XbrlFragment ValidatedFragment { get; private set; }

        internal SummationConceptValidator(XbrlFragment fragment)
        {
            this.ValidatedFragment = fragment;
        }

        /// <summary>
        /// Validates all of the summation concepts in this fragment.
        /// </summary>
        public void Validate()
        {
            foreach (var CurrentSchema in this.ValidatedFragment.Schemas.SchemaList)
                Validate(CurrentSchema);
        }

        /// <summary>
        /// Validates all summation concepts defined in the current schema.
        /// </summary>
        /// <param name="CurrentSchema">
        /// The schema whose containing summation concepts should be validated.
        /// </param>
        private void Validate(XbrlSchema CurrentSchema)
        {
            var calculationLinkbase = CurrentSchema.CalculationLinkbases;
            if (calculationLinkbase == null)
                return;
            foreach (CalculationLink CurrentCalculationLink in calculationLinkbase.SelectMany(x=>x.CalculationLinks))
                Validate(CurrentCalculationLink);
        }

        /// <summary>
        /// Validates all summation concepts found in the current calculation link.
        /// </summary>
        /// <param name="CurrentCalculationLink">
        /// The calculation link whose containing summation concepts should be validated.
        /// </param>
        private void Validate(CalculationLink CurrentCalculationLink)
        {
            foreach (SummationConcept CurrentSummationConcept in CurrentCalculationLink.SummationConcepts)
            {

                // Validate the main items in the fragment.

                ValidateSummationConcept(CurrentSummationConcept, this.ValidatedFragment.Facts);

                // Look for any tuples in the fragment and validate their items as well. This action
                // satisfies tests in the XBRL-CONF-CR3-2007-03-05 conformance suite such as 397.13.

                foreach (var CurrentFact in this.ValidatedFragment.Facts)
                {
                    if (CurrentFact is Tuple)
                    {
                        var CurrentTuple = CurrentFact as Tuple;
                        ValidateSummationConcept(CurrentSummationConcept, CurrentTuple.Facts);
                    }
                }
            }
        }

        /// <summary>
        /// Validates a given summation concept.
        /// </summary>
        /// <param name="CurrentSummationConcept">
        /// The summation concept to be validated.
        /// </param>
        /// <param name="FactList">
        /// The collection of items that should be searched when looking for summation or contributing items.
        /// </param>
        private void ValidateSummationConcept(SummationConcept CurrentSummationConcept, FactCollection FactList)
        {
            Element SummationConceptElement = LocateElement(CurrentSummationConcept.SummationConceptLocator);
            Item SummationConceptItem = LocateItem(SummationConceptElement, FactList);

            // If the summation concept item doesn't exist, then there is no calculation
            // to perform.

            if (SummationConceptItem == null)
                return;

            // If the summation concept item has a "nil" value, then there is no calculation
            // to perform.

            if (SummationConceptItem.NilSpecified == true)
            {
                return;
            }

            double SummationConceptRoundedValue = SummationConceptItem.RoundedValue;
            double ContributingConceptRoundedValueTotal = 0;
            var ContributingConceptItemsFound = false;
            var AtLeastOneItemWithZeroPrecision = false;
            foreach (Locator CurrentLocator in CurrentSummationConcept.ContributingConceptLocators)
            {

                // Some decisions need to be made before the code can actually add the value of the
                // contributing concept to the total that the code is keeping.

                var IncludeContributingConceptItemInCalculation = true;

                // Find the calculation arc for the given calculation link.

                var ContributingConceptCalculationArc = this.ValidatedFragment.GetCalculationArc(CurrentLocator);
                if (ContributingConceptCalculationArc == null)
                {
                    IncludeContributingConceptItemInCalculation = false;
                }
                if(ContributingConceptCalculationArc.Use == CalculationArc.ArcUse.Prohibited)
                {
                    IncludeContributingConceptItemInCalculation = false;
                }

                // Find the elemement for the given locator.

                Element ContributingConceptElement = LocateElement(CurrentLocator);
                if (ContributingConceptElement == null)
                {
                    IncludeContributingConceptItemInCalculation = false;
                }

                // Find all items for the given element. If there is more than one, and at least
                // one of them is not p-equals with at least one of the other ones, then
                // the entire calculation validation is forfeit, according to test 397.12 in 
                // the XBRL-CONF-CR3-2007-03-05 conformance suite.

                var AllMatchingItems = LocateItems(ContributingConceptElement, FactList);
                if (AllItemsNotPEquals(AllMatchingItems) == false)
                {
                    return;
                }

                // Find the item for the given element.

                if (AllMatchingItems.Count == 0)
                {
                    IncludeContributingConceptItemInCalculation = false;
                }
                else
                {
                    foreach (var ContributingConceptItem in AllMatchingItems)
                    {
                        if (IncludeContributingConceptItemInCalculation == true)
                        {
                            IncludeContributingConceptItemInCalculation = ContributingConceptItemEligibleForUseInCalculation(ContributingConceptItem, SummationConceptItem);
                        }
                        if(IncludeContributingConceptItemInCalculation == true)
                        {
                            IncludeContributingConceptItemInCalculation = SummationConceptItem.ContextRef.StructureEquals(ContributingConceptItem.ContextRef, ValidatedFragment);
                            if(IncludeContributingConceptItemInCalculation == false)
                            {
                            }
                        }
                        if (IncludeContributingConceptItemInCalculation == true)
                        {
                            ContributingConceptItemsFound = true;
                            if ((ContributingConceptItem.PrecisionSpecified == true) && (ContributingConceptItem.Precision == 0) && (ContributingConceptItem.InfinitePrecision == false))
                            {
                                AtLeastOneItemWithZeroPrecision = true;
                            }
                            double ContributingConceptRoundedValue = ContributingConceptItem.RoundedValue;
                            if (ContributingConceptCalculationArc.Weight != (decimal)(1.0))
                                ContributingConceptRoundedValue = ContributingConceptRoundedValue * (double)(ContributingConceptCalculationArc.Weight);
                            ContributingConceptRoundedValueTotal += ContributingConceptRoundedValue;
                        }
                    }
                }
            }
            if (ContributingConceptItemsFound == true)
            {
                if(AtLeastOneItemWithZeroPrecision == true)
                {
                    var MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("SummationConceptUsesContributingItemWithPrecisionZero");
                    MessageBuilder.AppendFormat(StringFormat, SummationConceptItem.Name);
                    ValidatedFragment.AddValidationError(new SummationConceptValidationError(CurrentSummationConcept, MessageBuilder.ToString()));
                    return;
                }
                ContributingConceptRoundedValueTotal = SummationConceptItem.Round(ContributingConceptRoundedValueTotal);
                if (SummationConceptRoundedValue != ContributingConceptRoundedValueTotal)
                {
                    var MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("SummationConceptError");
                    MessageBuilder.AppendFormat(StringFormat, SummationConceptItem.Name, SummationConceptRoundedValue, ContributingConceptRoundedValueTotal);
                    ValidatedFragment.AddValidationError(new SummationConceptValidationError(CurrentSummationConcept, MessageBuilder.ToString()));
                    return;
                }
            }
            else
            {                
                var MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NoValidContributingConceptsForSummationConcept");
                MessageBuilder.AppendFormat(StringFormat, SummationConceptItem.Name);
                ValidatedFragment.AddValidationError(new SummationConceptValidationError(CurrentSummationConcept, MessageBuilder.ToString()));
                return;
            }
        }

        /// <summary>
        /// Determines whether or not a contributing concept item is eligible for use in a calculation.
        /// </summary>
        /// <remarks>
        /// Contributing items must be context-equals, unit-equals, and have a non-nil value before it can
        /// be eligible for inclusion in a calculation.
        /// </remarks>
        /// <param name="ContributingConceptItem">
        /// The item to be considered for inclusion in a calculation.
        /// </param>
        /// <param name="SummationConceptItem">
        /// The summation item used in the calculation.
        /// </param>
        /// <returns>
        /// True if the contributing concept item is eligible to be used in a calculation; false otherwise.
        /// </returns>
        private bool ContributingConceptItemEligibleForUseInCalculation(Item ContributingConceptItem, Item SummationConceptItem)
        {
            if (SummationConceptItem.ContextEquals(ContributingConceptItem, ValidatedFragment) == false)
                return false;
            if (SummationConceptItem.UnitEquals(ContributingConceptItem) == false)
                return false;
            if (ContributingConceptItem.NilSpecified == true)
                return false;
            return true;
        }

        /// <summary>
        /// Locates an element given an element locator.
        /// </summary>
        /// <param name="ElementLocator">
        /// The locator specifying the element to find.
        /// </param>
        /// <returns>
        /// The element referenced by the locator; null if the element cannot be found.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Element"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private Element LocateElement(Locator ElementLocator)
        {
            return this.ValidatedFragment.Schemas.LocateElement(ElementLocator);
        }

        /// <summary>
        /// Locates an item in the fragment's list of facts.
        /// </summary>
        /// <param name="ItemElement">
        /// A schema element defining the item to be found.
        /// </param>
        /// <returns>
        /// A reference to the first item that matches the given element, or null if no matching item is found.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Item"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private Item LocateItem(Element ItemElement)
        {
            return LocateItem(ItemElement, this.ValidatedFragment.Facts);
        }

        /// <summary>
        /// Locates an item in a list of facts.
        /// </summary>
        /// <param name="ItemElement">
        /// A schema element defining the item to be found.
        /// </param>
        /// <param name="FactList">
        /// The collection of items that should be searched.
        /// </param>
        /// <returns>
        /// A reference to the first item that matches the given element, or null if no matching item is found.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Item"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private Item LocateItem(Element ItemElement, FactCollection FactList)
        {
            if (ItemElement == null)
                return null;
            foreach (Fact CurrentFact in FactList)
            {
                var CurrentItem = CurrentFact as Item;
                if (CurrentItem != null)
                {
                    if (CurrentItem.SchemaElement.Equals(ItemElement) == true)
                        return CurrentItem;
                }
            }
            return null;
        }

        /// <summary>
        /// Locates all items in the fragment that match the current element.
        /// </summary>
        /// <param name="ItemElement">
        /// The element describing the items to be found.
        /// </param>
        /// <returns>
        /// A collection of items that match the current element. If no items match,
        /// a non-null List will still be returned, but the list will be empty.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Item"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private List<Item> LocateItems(Element ItemElement)
        {
            return LocateItems(ItemElement, this.ValidatedFragment.Facts);
        }

        /// <summary>
        /// Locates all items in the given collection of facts that match the current element.
        /// </summary>
        /// <param name="ItemElement">
        /// The element describing the items to be found.
        /// </param>
        /// <param name="FactList">
        /// The collection of items that should be searched.
        /// </param>
        /// <returns>
        /// A collection of items that match the current element. If no items match,
        /// a non-null List will still be returned, but the list will be empty.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Item"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private List<Item> LocateItems(Element ItemElement, FactCollection FactList)
        {
            var ItemList = new List<Item>();
            if (ItemElement != null)
            {
                foreach (Fact CurrentFact in FactList)
                {
                    if ((CurrentFact is Item) == true)
                    {
                        var CurrentItem = CurrentFact as Item;
                        if (CurrentItem.SchemaElement.Equals(ItemElement) == true)
                            ItemList.Add(CurrentItem);
                    }
                    else if ((CurrentFact is Tuple) == true)
                    {
                        var currentTuple = CurrentFact as Tuple;
                        var tupleList = LocateItems(ItemElement, currentTuple.Facts);
                        ItemList.AddRange(tupleList);
                    }
                }
            }
            return ItemList;
        }

        /// <summary>
        /// Checks a list of <see cref="Item"/> objects to see if any of them are p-equals.
        /// </summary>
        /// <param name="ItemList">
        /// A collection of items to be checked.
        /// </param>
        /// <returns>
        /// True is returned if all of the items in the supplied collection are not p-equals.
        /// False is returned if any two items in the supplied collection are p-equals.
        /// </returns>
        /// <remarks>
        /// This method should most likely be moved into a class which wraps <see cref="Item"/>
        /// collections with a value-added wrapper class.
        /// </remarks>
        private bool AllItemsNotPEquals(List<Item> ItemList)
        {
            if (ItemList.Count < 2)
                return true;

            // At this point, we need to compare all items to all other items to check
            // for a p-equals condition. A naive implementation would be something like
            // this:
            //
            // for(var ItemIndex1 = 0; ItemIndex1 < ItemList.Count; ItemIndex1++)
            // {
            //     var Item1 = ItemList[ItemIndex1];
            //     for(var ItemIndex2 = 0; ItemIndex 2 < ItemList.Count; ItemIndex2++)
            //     {
            //         var Item2 = ItemList[ItemIndex2];
            //         // compare Item1 and Item2
            //     }
            // }
            //
            // However, that compares too many items two many times. Consider a collection
            // with three items. The loop above would perform the following comparisons:
            //
            // LOOP ITERATION    FIRST ITEM    SECOND ITEM    NOTES
            // ==============    ==========    ===========    =====
            // 1                 ItemList[0]   ItemList[0]    no need to compare to itself
            // 2                 ItemList[0]   ItemList[1]
            // 3                 ItemList[0]   ItemList[2]
            // 4                 ItemList[1]   ItemList[0]    same as loop iteration 2
            // 5                 ItemList[1]   ItemList[1]    no need to compare to itself
            // 6                 ItemList[1]   ItemList[2]
            // 7                 ItemList[2]   ItemList[0]    same as loop iteration 3
            // 8                 ItemList[2]   ItemList[1]    same as loop iteration 6
            // 9                 ItemList[2]   ItemList[2]    no need to compare to itself
            //
            // The inner for loop in the code below optimizes to get rid of some of these
            // inefficiencies.

            for (var ItemIndex1 = 0; ItemIndex1 < ItemList.Count; ItemIndex1++)
            {
                var item1 = ItemList[ItemIndex1];
                for (var ItemIndex2 = ItemIndex1 + 1; ItemIndex2 < ItemList.Count; ItemIndex2++)
                {
                    var item2 = ItemList[ItemIndex2];
                    if (item1.ParentEquals(item2) == true)
                        return false;
                }
            }
            return true;
        }
    }
}
