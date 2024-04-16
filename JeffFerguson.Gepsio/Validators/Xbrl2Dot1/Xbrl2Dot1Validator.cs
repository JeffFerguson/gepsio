using JeffFerguson.Gepsio.Xsd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio.Validators.Xbrl2Dot1
{
    /// <summary>
    /// Validates an XBRL fragment in accordance with the XBRL 2.1 specification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Most validations are performed at the fragment level because the validations
    /// need information available at the fragment level, across multiple classes. For
    /// example, one of the many validations to be performed is to check the context
    /// references in each of the facts to ensure that the references are valid. To do
    /// that, validation code needs both a list of facts, which contain the context
    /// references to be validated, as well as the list of loaded contexts, which contain
    /// the valid references. These two pieces of information are not exclusively available
    /// in the Fact class, nor are they exclusively available in the Context class. Those
    /// two pieces of information are together only at the XBRL fragment level.
    /// </para>
    /// <para>
    /// This class, therefore, performs fragment level validations. Other classes may
    /// are available to perform validations specific to a class.
    /// </para>
    /// <para>
    /// This class is the top-level, main, "start here" entry point for all XBRL 2.1
    /// validations. This class delegates to other validators as necessary.
    /// </para>
    /// </remarks>
    internal class Xbrl2Dot1Validator
    {
        private XbrlFragment validatingFragment;

        internal void Validate(XbrlFragment fragment)
        {
            validatingFragment = fragment;
            ValidateRoleReferences();
            ValidateArcroleReferences();
            ValidateContexts();
            ValidateContextRefs();
            ValidateUnitRefs();
            ValidateContextTimeSpansAgainstPeriodTypes();
            ValidateFootnoteLocations();
            ValidateFootnoteArcs();
            ValidateItems();
        }

        //-------------------------------------------------------------------------------
        // Validate role references.
        //
        // According to test 308.01 of the CR5 conformance suite, each role reference must
        // reference a unique URI.
        //-------------------------------------------------------------------------------
        private void ValidateRoleReferences()
        {
            var uniqueUris = new Dictionary<string, RoleReference>();

            foreach (var currentRoleReference in validatingFragment.RoleReferences)
            {
                var currentRoleReferenceUriAsString = currentRoleReference.Uri.ToString();
                if (uniqueUris.ContainsKey(currentRoleReferenceUriAsString) == true)
                {
                    string MessageFormat = AssemblyResources.GetName("DuplicateRoleReferenceUri");
                    StringBuilder MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, currentRoleReferenceUriAsString);
                    validatingFragment.AddValidationError(new RoleReferenceValidationError(currentRoleReference, MessageBuilder.ToString()));
                    return;
                }
                uniqueUris.Add(currentRoleReferenceUriAsString, currentRoleReference);
            }
        }


        //-------------------------------------------------------------------------------
        // Validate arcrole references.
        //
        // According to test 308.02 of the CR5 conformance suite, each arcrole reference
        // must reference a unique URI.
        //-------------------------------------------------------------------------------
        private void ValidateArcroleReferences()
        {
            var uniqueUris = new Dictionary<string, ArcroleReference>();

            foreach (var currentArcroleReference in validatingFragment.ArcroleReferences)
            {
                var currentArcroleReferenceUriAsString = currentArcroleReference.Uri.ToString();
                if (uniqueUris.ContainsKey(currentArcroleReferenceUriAsString) == true)
                {
                    string MessageFormat = AssemblyResources.GetName("DuplicateArcroleReferenceUri");
                    StringBuilder MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, currentArcroleReferenceUriAsString);
                    validatingFragment.AddValidationError(new ArcroleReferenceValidationError(currentArcroleReference, MessageBuilder.ToString()));
                    return;
                }
                uniqueUris.Add(currentArcroleReferenceUriAsString, currentArcroleReference);
            }
        }

        /// <summary>
        /// Validate context references for all facts in the fragment.
        /// </summary>
        private void ValidateContextRefs()
        {
            ValidateContextRefs(validatingFragment.Facts);
        }


        /// <summary>
        /// Validate context references for all facts in the given fact collection.
        /// </summary>
        /// <param name="FactList">
        /// A collection of facts whose contexts should be validated.
        /// </param>
        private void ValidateContextRefs(FactCollection FactList)
        {
            foreach (Fact CurrentFact in FactList)
            {
                if (CurrentFact is Item)
                    ValidateContextRef(CurrentFact as Item);
                else if (CurrentFact is Tuple)
                {
                    var CurrentTuple = CurrentFact as Tuple;
                    ValidateContextRefs(CurrentTuple.Facts);
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validates the context reference for the given fact. Ensures that the context
        // ref can be tied to a defined context.
        //-------------------------------------------------------------------------------
        private void ValidateContextRef(Item ItemToValidate)
        {
            string ContextRefValue = ItemToValidate.ContextRefName;
            if (ContextRefValue.Length == 0)
                return;

            try
            {
                Context MatchingContext = validatingFragment.ContextDictionary[ContextRefValue];
                ItemToValidate.ContextRef = MatchingContext;
            }
            catch (KeyNotFoundException)
            {
                string MessageFormat = AssemblyResources.GetName("CannotFindContextForContextRef");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, ContextRefValue);
                validatingFragment.AddValidationError(new ItemValidationError(ItemToValidate, MessageBuilder.ToString()));
            }
        }


        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateUnitRefs()
        {
            foreach (Fact CurrentFact in validatingFragment.Facts)
            {
                if (CurrentFact is Item)
                    ValidateUnitRef(CurrentFact as Item);
            }
        }

        //-------------------------------------------------------------------------------
        // Validates the unit reference for the given fact. Ensures that the unit ref
        // can be tied to a defined unit.
        //-------------------------------------------------------------------------------
        private void ValidateUnitRef(Item ItemToValidate)
        {
            string UnitRefValue = ItemToValidate.UnitRefName;
            //-----------------------------------------------------------------------
            // According to section 4.6.2, non-numeric items must not have a unit
            // reference. So, if the fact's unit reference is blank, and this is a
            // non-numeric item, then there is nothing to validate.
            //-----------------------------------------------------------------------
            if (UnitRefValue.Length == 0)
            {
                if (ItemToValidate.SchemaElement == null)
                    return;
                if (ItemToValidate.Type == null)
                    return;
                if (ItemToValidate.Type.IsNumeric == false)
                    return;
            }
            //-----------------------------------------------------------------------
            // At this point, we have a unit ref should be matched to a unit.
            //-----------------------------------------------------------------------
            bool UnitFound = false;
            Unit MatchingUnit = null;
            foreach (Unit CurrentUnit in validatingFragment.Units)
            {
                if (CurrentUnit.Id == UnitRefValue)
                {
                    UnitFound = true;
                    MatchingUnit = CurrentUnit;
                    ItemToValidate.UnitRef = MatchingUnit;
                }
            }
            //-----------------------------------------------------------------------
            // Check to see if a unit is found.
            //-----------------------------------------------------------------------
            if (UnitFound == false)
            {
                string MessageFormat = AssemblyResources.GetName("CannotFindUnitForUnitRef");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, UnitRefValue);
                validatingFragment.AddValidationError(new ItemValidationError(ItemToValidate, MessageBuilder.ToString()));
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateContextTimeSpansAgainstPeriodTypes()
        {
            foreach (Fact CurrentFact in validatingFragment.Facts)
            {
                if (CurrentFact is Item)
                {
                    var CurrentItem = CurrentFact as Item;
                    switch (CurrentItem.SchemaElement.PeriodType)
                    {
                        case Element.ElementPeriodType.Duration:
                            if (CurrentItem.ContextRef != null)
                            {
                                if (CurrentItem.ContextRef.DurationPeriod == false)
                                {
                                    StringBuilder MessageBuilder = new StringBuilder();
                                    string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesDurationButUsedWithNonDurationContext");
                                    MessageBuilder.AppendFormat(StringFormat, CurrentItem.SchemaElement.Schema.SchemaReferencePath, CurrentItem.Name, CurrentItem.ContextRef.Id);
                                    validatingFragment.AddValidationError(new ItemValidationError(CurrentItem, MessageBuilder.ToString()));
                                }
                            }
                            break;
                        case Element.ElementPeriodType.Instant:
                            if (CurrentItem.ContextRef != null)
                            {
                                if (CurrentItem.ContextRef.InstantPeriod == false)
                                {
                                    StringBuilder MessageBuilder = new StringBuilder();
                                    string StringFormat = AssemblyResources.GetName("ElementSchemaDefinesInstantButUsedWithNonInstantContext");
                                    MessageBuilder.AppendFormat(StringFormat, CurrentItem.SchemaElement.Schema.SchemaReferencePath, CurrentItem.Name, CurrentItem.ContextRef.Id);
                                    validatingFragment.AddValidationError(new ItemValidationError(CurrentItem, MessageBuilder.ToString()));
                                }
                            }
                            break;
                    }
                }
            }
        }


        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteLocations()
        {
            foreach (FootnoteLink CurrentFootnoteLink in validatingFragment.FootnoteLinks)
            {
                foreach (FootnoteLocator CurrentLocation in CurrentFootnoteLink.FootnoteLocators)
                    ValidateFootnoteLocation(CurrentLocation.Href.ElementId);   // TODO
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteLocation(string FootnoteLocationReference)
        {
            HyperlinkReference Reference = new HyperlinkReference(FootnoteLocationReference);
            if (Reference.UrlSpecified == true)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("FootnoteReferencesFactInExternalDoc");
                MessageBuilder.AppendFormat(StringFormat, Reference.ElementId, Reference.Url);
                validatingFragment.AddValidationError(new HyperlinkReferenceValidationError(Reference, MessageBuilder.ToString()));
                return;
            }
            if (validatingFragment.GetFact(Reference.ElementId) == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NoFactForFootnoteReference");
                MessageBuilder.AppendFormat(StringFormat, FootnoteLocationReference);
                validatingFragment.AddValidationError(new HyperlinkReferenceValidationError(Reference, MessageBuilder.ToString()));
                return;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteArcs()
        {
            foreach (FootnoteLink CurrentFootnoteLink in validatingFragment.FootnoteLinks)
            {
                foreach (FootnoteArc CurrentArc in CurrentFootnoteLink.FootnoteArcs)
                    ValidateFootnoteArc(CurrentArc);
            }
        }

        //-------------------------------------------------------------------------------
        // Validates a footnote arc.
        //
        // Validation is handled differently, depending on the arc's role. Note that
        // the XBRL specification discusses this:
        //
        // 4.11.1.3.1 @xlink:arcrole attributes on <footnoteArc> elements
        // The value of the @xlink:arcrole attribute MUST be a URI that indicates the
        // meaning of the arc. One standard arc role value has been defined for arc role
        // values on <footnoteArc> elements. Its value is:
        //
        // http://www.xbrl.org/2003/arcrole/fact-footnote
        //
        // This arc role value is for use on a <footnoteArc> from item or tuple Locators
        // to footnote resources and it indicates that the <footnote> conveys human-readable
        // information about the fact or facts.
        //
        // For more information, see the blog post at http://gepsio.wordpress.com/2014/07/09/better-validation-coming-for-footnote-arcs-and-arc-roles/.
        //-------------------------------------------------------------------------------
        private void ValidateFootnoteArc(FootnoteArc CurrentArc)
        {
            FootnoteLocator Locator = CurrentArc.Link.GetLocator(CurrentArc.From);
            if (Locator == null)
            {
                if (CurrentArc.StandardArcRole == true)
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("CannotFindFootnoteLocator");
                    MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, CurrentArc.From);
                    validatingFragment.AddValidationError(new FootnoteArcValidationError(CurrentArc, MessageBuilder.ToString()));
                    return;
                }
                var fromFootnote = CurrentArc.Link.GetFootnote(CurrentArc.From);
                if (fromFootnote == null)
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("CannotFindFootnoteLocatorOrFootnote");
                    MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, CurrentArc.From);
                    validatingFragment.AddValidationError(new FootnoteArcValidationError(CurrentArc, MessageBuilder.ToString()));
                    return;
                }
                CurrentArc.FromFootnote = fromFootnote;
            }
            else
            {
                if ((Locator.Href.UrlSpecified == true) && (validatingFragment.UrlReferencesFragmentDocument(Locator.Href) == false))
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("FootnoteReferencesFactInExternalDoc");
                    MessageBuilder.AppendFormat(StringFormat, Locator.Href.ElementId, Locator.Href.Url);
                    validatingFragment.AddValidationError(new FootnoteArcValidationError(CurrentArc, MessageBuilder.ToString()));
                    return;
                }
                CurrentArc.FromItem = validatingFragment.GetFact(Locator.Href.ElementId);
                if (CurrentArc.FromItem == null)
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("CannotFindFactForFootnoteArc");
                    MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, Locator.Href);
                    validatingFragment.AddValidationError(new FootnoteArcValidationError(CurrentArc, MessageBuilder.ToString()));
                    return;
                }
            }
            CurrentArc.ToFootnote = CurrentArc.Link.GetFootnote(CurrentArc.To);
            if (CurrentArc.ToFootnote == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("CannotFindFootnoteForFootnoteArc");
                MessageBuilder.AppendFormat(StringFormat, CurrentArc.Title, CurrentArc.To);
                validatingFragment.AddValidationError(new FootnoteArcValidationError(CurrentArc, MessageBuilder.ToString()));
                return;
            }
        }

        //-------------------------------------------------------------------------------
        // Validate all of the facts found in the fragment. Multiple activities happen
        // here:
        //
        // * each fact is validated against its data type described in its definition
        //   via an <element> tag in a taxonomy schema
        // * any facts that participate in an arc role are checked
        //-------------------------------------------------------------------------------
        private void ValidateItems()
        {
            var validator = new FactValidator();
            foreach (Fact CurrentFact in validatingFragment.Facts)
            {
                validator.Validate(validatingFragment, CurrentFact);
            }
            ValidateItemsReferencedInDefinitionArcRoles();
            ValidateSummationConcepts();
        }

        //-------------------------------------------------------------------------------
        // Searches the associated XBRL schemas, looking for facts that are referenced
        // in arc roles.
        //-------------------------------------------------------------------------------
        private void ValidateItemsReferencedInDefinitionArcRoles()
        {
            foreach (var currentSchema in validatingFragment.Schemas.SchemaList)
            {
                var currentDefinitionLinkbaseDocument = currentSchema.DefinitionLinkbases;
                if (currentDefinitionLinkbaseDocument != null)
                {
					foreach( DefinitionLink CurrentDefinitionLink in currentDefinitionLinkbaseDocument.SelectMany( x => x.DefinitionLinks ) )
                        ValidateFactsReferencedInDefinitionArcRoles(CurrentDefinitionLink);
                }
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ValidateFactsReferencedInDefinitionArcRoles(DefinitionLink CurrentDefinitionLink)
        {
            foreach (DefinitionArc CurrentDefinitionArc in CurrentDefinitionLink.DefinitionArcs)
            {
                switch (CurrentDefinitionArc.DefinitionArcRole)
                {
                    case DefinitionArc.RoleEnum.EssenceAlias:
                        ValidateEssenceAliasedFacts(CurrentDefinitionArc);
                        break;
                    case DefinitionArc.RoleEnum.RequiresElement:
                        ValidateRequiresElementFacts(CurrentDefinitionArc);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Validate the essence alias between two facts referenced in a definition arc using
        /// the set of all facts in the fragment. 
        /// </summary>
        /// <param name="EssenceAliasDefinitionArc">
        /// The definition arc defining the essence alias.
        /// </param>
        private void ValidateEssenceAliasedFacts(DefinitionArc EssenceAliasDefinitionArc)
        {
            ValidateEssenceAliasedFacts(EssenceAliasDefinitionArc, validatingFragment.Facts);
        }

        /// <summary>
        /// Validate the essence alias between two facts referenced in a definition arc using
        /// the set of all facts in the fragment. 
        /// </summary>
        /// <param name="EssenceAliasDefinitionArc">
        /// The definition arc defining the essence alias.
        /// </param>
        /// <param name="FactList">
        /// A collection of <see cref="Fact"/> objects defined in the fragment.
        /// </param>
        private void ValidateEssenceAliasedFacts(DefinitionArc EssenceAliasDefinitionArc, FactCollection FactList)
        {
            Locator CurrentFromLocator = EssenceAliasDefinitionArc.FromLocator;
            if (CurrentFromLocator == null)
                throw new NullReferenceException("FromLocator is NULL in ValidateEssenceAliasedFacts()");
            Locator CurrentToLocator = EssenceAliasDefinitionArc.ToLocator;

            foreach (Fact CurrentFact in FactList)
            {
                if (CurrentFact is Item)
                {
                    var CurrentItem = CurrentFact as Item;
                    if (CurrentItem.Name.Equals(CurrentFromLocator.HrefResourceId) == true)
                        ValidateEssenceAliasedFacts(CurrentItem, FactList, CurrentToLocator.HrefResourceId);
                }
                else if (CurrentFact is Tuple)
                {
                    var CurrentTuple = CurrentFact as Tuple;
                    ValidateEssenceAliasedFacts(EssenceAliasDefinitionArc, CurrentTuple.Facts);
                }
            }
        }

        /// <summary>
        /// Validate the essence alias between a given fact and all other facts with the given fact name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An essence alias is a relationship between a "from" item and a "to" item. The "from" item and
        /// the "to" item must be identical. This method is given the "from" item and must search for the
        /// corresponding "to" item.
        /// </para>
        /// <para>
        /// The scoping of the search for the corresponding "to" item is important. In the simple case, an
        /// XBRL fragment has only items, and the search for the corresponding "to" item can be conducted
        /// in the list all of all items in the fragment.
        /// </para>
        /// <para>
        /// However, if the "from" item is found in a tuple, then the list of items from which the "to" item
        /// is to be found must be restricted to the other items in the tuple and not simply the set of all
        /// items in the fragment.
        /// </para>
        /// </remarks>
        /// <param name="FromItem">
        /// The item that represents the "from" end of the essence alias relationship.
        /// </param>
        /// <param name="FactList">
        /// The list of facts that should be searched to find the item that represents the "to" end of the essence alias relationship.
        /// </param>
        /// <param name="ToItemName">
        /// The name of the item that represents the "to" end of the essence alias relationship.
        /// </param>
        private void ValidateEssenceAliasedFacts(Item FromItem, FactCollection FactList, string ToItemName)
        {
            foreach (Fact CurrentFact in FactList)
            {
                var CurrentItem = CurrentFact as Item;
                if (CurrentItem != null)
                {
                    if (CurrentFact.Name.Equals(ToItemName) == true)
                        ValidateEssenceAliasedFacts(FromItem, CurrentItem);
                }
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the essence alias between two given facts.
        //-------------------------------------------------------------------------------
        private void ValidateEssenceAliasedFacts(Item FromItem, Item ToItem)
        {

            // Essence alias checks for c-equals items are a bit tricky, according to the
            // XBRL-CONF-CR3-2007-03-05 conformance suite. Test 392.11 says that it is valid
            // to have two items with contexts having the same structure but different
            // period values is valid; however, test 392.13 says that it is invalid two have
            // two items with contexts having a different structure.

            if (FromItem.ContextEquals(ToItem, validatingFragment) == false)
            {
                if ((FromItem.ContextRef != null) && (ToItem.ContextRef != null))
                {
                    if (FromItem.ContextRef.PeriodTypeEquals(ToItem.ContextRef) == false)
                    {
                        StringBuilder MessageBuilder = new StringBuilder();
                        string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotContextEquals");
                        MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                        var validationError = new ItemsValidationError(MessageBuilder.ToString());
                        validationError.AddItem(FromItem);
                        validationError.AddItem(ToItem);
                        validatingFragment.AddValidationError(validationError);
                        return;
                    }
                }
                return;
            }
            if (FromItem.ParentEquals(ToItem) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotParentEquals");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                var validationError = new ItemsValidationError(MessageBuilder.ToString());
                validationError.AddItem(FromItem);
                validationError.AddItem(ToItem);
                validatingFragment.AddValidationError(validationError);
                return;
            }
            if (FromItem.UnitEquals(ToItem) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsNotUnitEquals");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, ToItem.Id);
                var validationError = new ItemsValidationError(MessageBuilder.ToString());
                validationError.AddItem(FromItem);
                validationError.AddItem(ToItem);
                validatingFragment.AddValidationError(validationError);
                return;
            }

            // At this point, the valies of the items need to be compared. Check the item's type
            // to ensure that the correct value is being compared.

            var ItemValuesMatch = true;
            if (FromItem.SchemaElement.TypeName.Name.Equals("stringItemType") == true)
            {
                ItemValuesMatch = FromItem.Value.Equals(ToItem.Value);
            }
            else
            {
                if (FromItem.RoundedValue != ToItem.RoundedValue)
                    ItemValuesMatch = false;
            }
            if (ItemValuesMatch == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("EssenceAliasFactsHaveDifferentRoundedValues");
                MessageBuilder.AppendFormat(StringFormat, FromItem.Name, ToItem.Name, FromItem.Id, FromItem.RoundedValue.ToString(), ToItem.Id, ToItem.RoundedValue.ToString());
                var validationError = new ItemsValidationError(MessageBuilder.ToString());
                validationError.AddItem(FromItem);
                validationError.AddItem(ToItem);
                validatingFragment.AddValidationError(validationError);
                return;
            }
        }

        //-------------------------------------------------------------------------------
        // Validate the "requires element" connection between two facts referenced in a
        // definition arc.
        //-------------------------------------------------------------------------------
        private void ValidateRequiresElementFacts(DefinitionArc RequiresElementDefinitionArc)
        {
            Locator CurrentFromLocator = RequiresElementDefinitionArc.FromLocator;
            Locator CurrentToLocator = RequiresElementDefinitionArc.ToLocator;
            int FromFactCount = CountFactInstances(CurrentFromLocator.HrefResourceId);
            int ToFactCount = CountFactInstances(CurrentToLocator.HrefResourceId);
            if (FromFactCount > ToFactCount)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("NotEnoughToFactsInRequiresElementRelationship");
                MessageBuilder.AppendFormat(StringFormat, CurrentFromLocator.HrefResourceId, CurrentToLocator.HrefResourceId);
                validatingFragment.AddValidationError(new DefinitionArcValidationError(RequiresElementDefinitionArc, MessageBuilder.ToString()));
            }
        }

        //-------------------------------------------------------------------------------
        // Returns a count of the number of facts with the given name.
        //-------------------------------------------------------------------------------
        private int CountFactInstances(string FactName)
        {
            int Count = 0;

            foreach (Fact CurrentFact in validatingFragment.Facts)
            {
                if (CurrentFact.Name.Equals(FactName) == true)
                    Count++;
            }
            return Count;
        }

        /// <summary>
        /// Validates all of the summation concepts in this fragment.
        /// </summary>
        private void ValidateSummationConcepts()
        {
            var validator = new SummationConceptValidator(validatingFragment);
            validator.Validate();
        }

        private void ValidateContexts()
        {
            var validator = new ContextValidator();
            foreach (var currentContext in validatingFragment.Contexts)
                validator.Validate(validatingFragment, currentContext);
        }
    }
}
