# Design
* Gepsio now targets .NET 6.0.
* Gepsio now builds on Linux as well as Windows. Gepsio unit tests use the `Path.DirectorySeparatorChar` property when constructing relative paths so that the appropriate directory separator character is used when running unit tests on any given operating system.
* Per Microsoft's guidance, Gepsio's use of `HttpClient` is now instantiated once and re-used for all instances of `XbrlDocument`. Instantiating an `HttpClient` object for every request, as was done in previous versions of Gepsio, will exhaust the number of sockets available under heavy loads. This will result in `SocketException` errors.
* The `XbrlDocument` class now includes explicit support for loading documents found at the [SEC Web site](https://www.sec.gov/), with no additional work required by the caller. Gepsio automatically detects URIs specifying documents found in the SEC Web site and supplies the appropriate HTTP headers to the HTTP request during document retrieval. All of this extra work is transparent to the caller. See [this documentation](https://www.sec.gov/os/accessing-edgar-data) for more information.
* The `Create()` factory method in the `LinkbaseDocument` class will no longer throw an exception when the linkbase document's role cannot be determined. Instead, a `null` reference will be returned.
# Bug Fixes
* No changes from the previous release.
# Breaking Changes
 * No changes from the previous release.
# Conformance
* Improved notion of structure equality for units by allowing measure qualified names to be anywhere in the set of names, as long as they are present. Previous versions required the measure qualified names to be in the same order for a unit's structure equality.
* XML node values are now examined when determining a node's structure equality with another XML node.
* Gepsio can now internally check arcs for equivalency using the rules in [section 3.5.3.9.7.4 of the XBRL Specification](http://www.xbrl.org/Specification/XBRL-2.1/REC-2003-12-31/XBRL-2.1-REC-2003-12-31+corrected-errata-2013-02-20.html#_3.5.3.9.7.4).
* Inproved calculation linkbase validation, specifically:
  * Gepsio now honors explicit priority values specified in calculation arcs.
  * The calculation linkbase validation engine now honors calculation arcs marked with prohibited usage and manages the calculation link appropriately.
  * Gepsio now checks for structure equality between a summation concept item's context and all contributing concept item's contexts.
* Gepsio now has the notion of type-safe values for node and attribute values. For nodes and attributes defined in a schema as having a non-string value, such as an integer, double, or decimal value, then Gepsio will convert the original string-based value into a value of the correct type when necessary.
# Industry-Standard Schema Support
Gepsio automatically loads industry-standard schemas when referenced by their namespace, even when not explicitly referenced by a `schemaRef` element. This list describes the industry-standard schemas that Gepsio supports. 

Schemas marked ***NEW*** have been added to the support list in this release.

  * Document Information and Entity Information 2009
  * Document Information and Entity Information 2014
  * International Financial Reporting Standards (IFRS) 2016
  * International Financial Reporting Standards (IFRS) 2017
  * International Financial Reporting Standards (IFRS) 2018
  * US-GAAP 2009
  * US-GAAP 2017
  * US-GAAP 2017 Investment Management
  * US-GAAP 2018
  * US-GAAP 2018 Investment Management
  * US Mutual Fund Risk/Return Taxonomy 2012
  * US Mutual Fund Risk/Return Taxonomy 2018

The blog post "[Next Release to Support Automatic Loading of Industry Standard Schemas](https://gepsio.wordpress.com/2017/10/05/next-release-to-support-automatic-loading-of-industry-standard-schemas/)" provides more information about the need for, and design of, the support for industry-standard schemas.

# Conformance Tests In XBRL-CONF-2014-12-10 Passed by Gepsio
Tests marked ***NEW*** failed in previous releases but now pass due to Gepsio's increased conformance with the XBRL 2.1 Specification.
## Identifier Scope [Section 4.3  The Item Element]
* `301-01-IdScopeValid.xml` [301.01 Valid example of id attribute.]
* `301-03-IdScopePeriodDiff.xml` [301.03  Mismatch of periodType attribute and referenced context's period type.]
* `301-04-IdScopeContextRefToUnit.xml` [301.04  contextRef has IDREF to unit element.]
* `301-05-IdScopeUnitRefToContext.xml` [301.05 unitRef has IDREF to context element.]
* `301-06-FootnoteScopeValid.xml` [301.06  Valid example of footnote link href attribute.]
* `301-08-FootnoteToContext.xml` [301.08 href attribute is referencing to context element.]
* `301-09-FootnoteToUnit.xml` [301.09 href attribute is referencing to unit element.]
* `301-10-FootnoteFromOutOfScope.xml` [301.10 The instance contains two footnote links.  The second one contains an arc with a from value that does not have a corresponding loc in the same extended link.]
* `301-11-FootnoteToOutOfScope.xml` [301.11 The instance contains two footnote links.  The second one contains an arc with a to value that does not have a corresponding footnote resource in the same extended link.]
* `301-12-FootnoteLocOutOfScope.xml` [301.12 The instance contains a footnote link.  In the footnote link there is a loc element that has an href that points to a fact in another instance document.]
* `301-13-FootnoteLocInScope.xml` [301.13 The instance contains a footnote link.  In the footnote link there is a loc element that has an href that points to a fact in the instance document using the instance document file name with a fragment identifier.]
* `301-14-FootnoteFromResource.xml` [301.14 The instance contains a footnote link. The arc in the footnote link has a from value that matches a footnote resource. This is not valid for the fact-footnote arc role on the arc. The from must point to a loc which in turns points to a fact in the same instance document.]
* `301-15-FootnoteToLoc.xml` [301.15 The instance contains a footnote link. The arc in the footnote link has a from value that matches a footnote resource. This is not valid for the fact-footnote arc role on the arc. The from must point to a loc which in turns points to a fact in the same instance document.]
* `301-16-FootnoteWithoutLang.xml` [301.16 The xml:lang attribute is missing on the footnote resource.]
* `301-17-FootnoteCustomArcRole.xml` [301.17 The footnote custom arc role can relate two footnotes to each other instead of just fact to footnote.  (Only the standard footnote arc role is restricted to being from item or tuple locators.) Maybe this might be used to indicate how some footnote is "footnoting" another footnote.]
## Context Segments and Scenarios [Section 4.4  The Context Element]
* `302-01-SegmentValid.xml` [302.01 Valid segment in a context]
* `302-02-SegmentNamespaceInvalid.xml` [302.02 Invalid segment in a context; contains an element defined in xbrli namespace]
* `302-03-ScenarioValid.xml` [302.03  Valid scenario in a context]
* `302-04-ScenarioNamespaceInvalid.xml` [302.04 Invalid scenario in a context; contains an element defined in xbrli namespace]
* `302-05-SegmentSubstitutionInvalid.xml` [302.05 Invalid segment in a context; contains an element in substitution group of xbrli:item]
* `302-06-ScenarioSubstitutionInvalid.xml` [302.06  Invalid scenario in a context; contains an element in substitution group of xbrli:item]
* `302-07-SegmentEmptyContent.xml` [302.07 Segment in a context contains an element with empty content]
* `302-08-ScenarioEmptyContent.xml` [302.08 Scenario in a context contains an element with empty content]
* `302-09-PeriodDateTimeValid.xml` [302.09 Valid duration context with start date earlier than end date]
* `302-10-PeriodDateTimeInvalid.xml` [302.10 Invalid duration context with start date later than end date]
* `302-11-DecimalAttributeOnSegmentInconsistent.xbrl` [302.11 Two contexts are S-Equal even though a decimal-valued attribute in their segment elements have different lexical representations. The contexts are S-equal, so a calculation inconsistency MUST be signaled.]
* `302-12-DecimalAttributeOnScenarioInconsistent.xbrl` [302.12 Two contexts are S-Equal even though a decimal-valued attribute in their scenario elements have different lexical representations. The contexts are S-equal, so a calculation inconsistency MUST be signaled.]
## Period Type Consistency [Section 4.3  The Item Element]
* `303-01-PeriodInstantValid.xml` [303.01 instant context and item defined with PeriodType="instant"]
* `303-02-PeriodDurationValid.xml` [303.02 duration context and item defined with PeriodType="duration"]
* `303-03-PeriodInstantInvalid.xml` [303.03 duration context and item defined with PeriodType="instant"]
* `303-04-PeriodDurationInvalid.xml` [303.04 instant context and item defined with PeriodType="duration"]
* `303-05-ForeverElementewithInstancePeriodTypeReportedasForever.xbrl` [ForeverConcept with Instant Period Type is not allowed]
## Unit of Measure Consistency [Section 4.4 The Context Element]
* `304-01-monetaryItemTypeUnitsRestrictions.xml` [304.01 An element with a monetary item type has an ISO currency code for its units (using the standard ISO namespace prefix).]
* `304-02-monetaryItemTypeUnitsRestrictions.xml` [304.02 An element with a monetary item type has an ISO currency code for its units (using a non-standard ISO namespace prefix).]
* `304-03-monetaryItemTypeUnitsRestrictions.xml` [304.03 An element with a type derived by restriction from the monetary item type has an ISO currency code for its units.]
* `304-04-monetaryItemTypeUnitsRestrictions.xml` [304.04 An element with a type derived by restriction from monetary item type has an ISO currency code for its units (using a non-standard ISO namespace prefix).]
* `304-05-monetaryItemTypeUnitsRestrictions.xml` [304.05 An element with a non-monetary item type has an ISO currency code for its units (using the standard ISO namespace prefix).]
* `304-06-monetaryItemTypeUnitsRestrictions.xml` [304.06 An element with a monetary item type does not have an ISO currency code for its units - the namespace is wrong.]
* `304-07-monetaryItemTypeUnitsRestrictions.xml` [304.07 An element with a monetaryItemType does not have an ISO currency code for its units - the local name is wrong.]
* `304-08-monetaryItemTypeUnitsRestrictions.xml` [304.08 An element with a type derived by restriction from monetaryItemType does not have an ISO currency code for its units - the namespace is wrong.]
* `304-09-monetaryItemTypeUnitsRestrictions.xml` [304.09 An element with a type derived by restriction from monetaryItemType does not have an ISO currency code for its units - the local name is wrong.]
* `304-10-pureItemTypeUnitsRestrictions.xml` [304.10 An item with a pureItemType data type MUST have  a unit element and the local part of the measure MUST be "pure" with a namespace prefix that resolves to a namespace of "http://www.xbrl.org/2003/instance".]
* `304-11-pureItemTypeUnitsRestrictions.xml` [A measure element with a namespace prefix that resolves to the "http://www.xbrl.org/2003/instance" namespace MUST have a local part of either "pure" or "shares". The value 'impure' is not a valid measure in the XBRL instance namespace.]
* `304-12-pureItemTypeUnitsRestrictions.xml` [Unlike for monetaryItemType and sharesItemType, there is no constraint (in 4.8.2 or elsewhere) requiring an item with a pureItemType data type to have a particular kind of unit.]
* `304-12a-pureItemTypeUnitsRestrictions.xml` [Same as V-12, but the pure measure has no prefix and the default namespace is undefined.]
* `304-13-sharesItemTypeUnitsRestrictions.xml` [304.13 For facts that are of the sharesItemType, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance]
* `304-14-sharesItemTypeUnitsRestrictions.xml` [304.14 For facts that are DERIVED BY RESTRICTION from the sharesItemType, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance]
* `304-15-pureItemTypeUnitsRestrictions.xml` [304.15 For facts that are of shares item type, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance.  In this case the unit has two measure elements, both of which are pure.]
* `304-15a-sharesItemTypeUnitsRestrictions.xml` [Same as V-15 but in this case the unit has has shares but no prefix and the default namespace is undefined.]
* `304-16-unitsInSimplestForm.xml` [304.16 The units must not have numerator and denominator measures that cancel.]
* `304-17-sameOrderMeasuresValid.xml` [304.17 The units equality test which two units have same order measures.]
* `304-18-sameOrderDivisionMeasuresValid.xml` [304.18 The units equality test which two units have same order divisions.]
* `304-19-differentOrderMeasuresValid.xml` [304.19 The units equality test which two units have different order measures.]
* `304-20-differentOrderDivisionMeasuresValid.xml` [304.20 The units equality test which two units have division elements which their order of child measures are different.]
* `304-21-measuresInvalid.xml` [304.21 it tries to essence-alias equality of two elements with different units : where one is pure-feet and the second is pure-pounds. so the alias essence check is invalid and it should throw an error in xbrl validation]
* `304-22-divisionMeasuresInvalid.xml` [304.22 The test tried to essense-alias equality check of two elements with different units : where one is unit between "pure-inch / pound-feet" and other "pure-feet / pound-inch". The tests is invalid as it should throw an error during xbrl validation.]
* `304-23-Calculation-item-does-not-match-unit.xml` [Variation of 304-15 where the type of the fact value does not match that of the type of the reported element. Shares type versus Monetary unit]
* `304-24-valid-ISO-unit-of-measue.xml` [Valid ISO unit of measurement example]
* `304-25-measure-reported-with-prefix-undefined-instance.xbrl` [Measure reported with prefix undefined is considered XBRL invalid]
* `304-26-monetaryItemTypeUnitsRestrictions.xml` [Monetary item reported with unit having a denominator.]
## Decimal and Precision Mutual Exclusion and prohibition on nil items [Section 4.4  Items]
* `305-01-DecimalOnlySpecified.xml` [305.01 item has only Decimals specified]
* `305-02-PrecisionOnlySpecified.xml` [305.02 item has only Precision specified]
* `305-03-NoDecimalOrPrecisionSpecified.xml` [305.03 item has neither Decimals nor Precision specified]
* `305-04-BothDecimalAndPrecisionSpecified.xml` [305.04 item has both Decimals and Precision specified]
* `305-05-DecimalSpecifiedOnNilItem.xml` [305.05 nil item has Decimals specified]
* `305-06-PrecisionSpecifiedOnNilItem.xml` [305.06 nil item has Precision specified]
* `305_07_invalid_instance.xbrl` [305.07 a genuine inconsistency due to roll up of child values]
* `305-08-UnitsSpecifiedOnNilItem.xml` [305.08 nil items have no decimals or precision, with unitref, but the type specifies fixed values for decimals and precision.]
## Required Arc in Definition Linkbase [Section 5.5.6.4.1.5]
* `306-01-RequiredInstanceValid.xml` [306.01 The instance contains two elements in the same context. The presence of one element forces the presence of the other.]
* `306-02-RequiredInstanceTupleValid.xml` [306.02 The instance contains an item and a tuple. The presence of the tuple forces the presence of the item.]
* `306-03-RequiredInstanceInvalid.xml` [306.03 The instance contains an item and a tuple. The presence of the tuple forces the presence of the item.]
## Schema References [Section 5 Taxonomies]
* `307-01-SchemaRefExample.xml` [307.01 A schemaRef element MUST hold the URI of a schema.  In this case it does.]
* `307-02-SchemaRefCounterExample.xml` [307.01 A schemaRef element MUST hold the URI of a schema.  In this case it does not because the second reference to a schema actually points to an XML document that is a label linkbase. ]
* `307-03-SchemaRefXMLBase.xml` [307.03 schemaRef elements MUST hold the URI of Schemas.  In this case the requirement is not satisfied because the schema reference has to be resolved using the XML base attribute that ensures the schemaRef URI resolves to the XML document in the base directory.  This document, however, is a label linkbase, not a schema.  If the XML base attribute value is not used then the schema in the same directory as the instance is discovered and no issues are noticed.]
## Duplicate instance roleRef and duplicate arcroleRefs [3.5.2.4.5 and 3.5.2.5.5 duplicate instance roleRef and arcroleRef elements.]
* `308-01-instance.xml` [Instance contains two role references to the same URI, INVALID]
* `308-02-instance.xml` [Instance contains two arcrole references to the same URI, INVALID]
## LAX validation tests [Test that LAX validation is performed]
* `314-lax-validation-01.xml` [Segment has an element for which there is no definition, so it is allowed; item has an attribute with no definition, so it is allowed.  The definitions are imported to the discovered taxonomy.]
* `314-lax-validation-02.xml` [Segment has an element for which there is no definition, so it is allowed; item has an attribute with no definition, so it is allowed.  The definitions are found by schemaLocation from the instance document.]
* `314-lax-validation-03.xml` [Same as v-01 but segment has an element defined as integer with string contents]
* `314-lax-validation-04.xml` [Same as v-01 but item has an attribute defined as integer with string contents]
* `314-lax-validation-05.xml` [Same as v-02 but segment has an element defined as integer with string contents]
* `314-lax-validation-06.xml` [Same as v-02 but item has an attribute defined as integer with string contents]
## Identifier Scope [Relevant sections for calculation binding rules 5.2.5.2 and 4.6.6]
* `320-00-BindCalculationInferPrecision-instance.xbrl` [320.00 - Valid]
* `320-01-BindCalculationInferPrecision-instance.xbrl` [320.01 - Valid]
* `320-02-BindCalculationInferPrecision-instance.xbrl` [320.02 - InValid]
* `320-03-nestedtupleBindCalculationInferPrecision-instance.xbrl` [320.03 - Valid]
* `320-04-BindCalculationInferPrecision-instance.xbrl` [320.04 - Valid]
* `320-05-BindCalculationInferPrecision-instance.xbrl` [320.05 - Valid]
* `320-06-BindCalculationInferPrecision-instance.xbrl` [320.06 - inValid - inconsistent due to precision 0]
* `320-07-BindCalculationInferPrecision-instance.xbrl` [320.07 - inValid - inconsistent due to precision 0]
* `320-08-BindCalculationInferPrecision-instance.xbrl` [320.08 - inValid - inconsistent due to precision 0]
* `320-09-BindCalculationInferPrecision-instance.xbrl` [320.09 - Valid]
* `320-10-BindCalculationInferPrecision-instance.xbrl` [320.10 - Valid - precision stated as 15]
* `320-11-BindCalculationInferPrecision-instance.xbrl` [320.11 - inValid - inconsistent]
* `320-12-BindCalculationInferPrecision-instance.xbrl` [320.12 - Valid - Consistent - precision attribute stated as 15]
* `320-13-BindCalculationInferPrecision-instance.xbrl` [320.13 - Valid - Consistent]
* `320-14-BindCalculationInferPrecision-instance.xbrl` [320.14 - Valid - Consistent]
* `320-15-BindCalculationInferPrecision-instance.xbrl` [320.15 - Valid - COnsistent - Decimal attribute zero]
* `320-16-BindCalculationInferPrecision-instance.xbrl` [320.16 - InValid - Inconsistency contributing items 3200, summation value is 3201]
* `320-17-BindCalculationInferPrecision-instance.xbrl` [320.17 - InValid - Inconsistent roll up - weights stated as 1.0]
* `320-18-BindCalculationInferPrecision-instance.xbrl` [320.18 - InValid - 2.04 effective value generates inconsistent roll up - weight is defined as 1.01]
* `320-19-BindCalculationInferPrecision-instance.xbrl` [320.19 - InValid - effective value 1.02 generates inconsistent roll up - weight is defined as 1.01]
* `320-20-BindCalculationInferPrecision-instance.xbrl` [320.20 - Valid - Consistent; Weight is applied to the value after the decimal or precision is applied to the lexical value reported in the instance; Arc weights are defined with the value 1.01; Test ensures the sequence that we apply these rules of inferred precision and weight attribute]
* `320-21-BindCalculationInferPrecision-instance.xbrl` [320.21 - Duplicate Facts reported and thus calculation will not bind per 5.2.5.2; Test ensures that we do not bind the calculation due to a duplicated fact and do not infer precision.]
* `320-22-BindCalculationInferPrecision-instance.xbrl` [320.22 - Duplicate Facts reported and thus calculation will not bind per 5.2.5.2; Test ensures that we Do NOT bind the calculation and do not infer precision.  If it did bind then there would be an inconsistency.]
* `320-23-BindCalculationInferPrecision-instance.xbrl` [320-24 - Valid - Consistent; Inferred precision and Weight Attribute test]
* `320-24-BindCalculationInferPrecision-instance.xbrl` [320-24 - Valid - Consistent; Inferred precision and Weight Attribute test in correct sequence]
* `320-25-BindCalculationInferPrecision-instance.xbrl` [320-25 - Invalid; Test:   Verifies that IsNill facts do not bind to calculations per 5.2.5.2]
* `320-26-BindCalculationInferPrecision-instance.xbrl` [320-26 - Valid, but contains number likely to sneak in errors in floating point multiplication of the weight 0.45 x A113 (=1010103).  For non-decimal CPU arithmetic rounding may be required on each weight multiplication to avoid floating point weirdnesses.  (this irritation contributed by Herm Fischer)]
* `320-30-BindCalculationInferDecimals-instance.xbrl` [Tests that a decimals 0 value 0 is not treated as precision 0 (invalid) but as numeric zero.  In the prior approach where decimals 0 value 0 converted to precision 0 value 0, this would have been invalid.]
* `320-31-BindCalculationInferDecimals-instance.xbrl` [Edge case tests that decimal rounding with is performed. In decimals representations, summation 0d1 is compared to contributing items 1.234d2 and -1.223d2.  Contributing items round to 1.23 and -1.22, the contributing items rounded sum is .01.  Summation has decimals 1 which is applied to comparing 0 decimals 1 to contributing items rounded sum .01, but in decimals 1, which makes the contributing rounded sum .01 be rounded as .0, which is equal to summation zero decimals 1, in decimals.  If the calculation were performed the old way, treating summation 0d1 as 0p0 this alone would make the calculation roll up be invalid.  But many processors have anyway treated 0d1 as 0p1, in which case such a processor still would be comparing numeric 0 for the summation, to the rounded contributing items sum .01 in some non-zero precision, which would still be invalid.  So the point of this test is to be sure the processor is using the all-decimals calculation roll up.]
* `320-32-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even]
* `320-33-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even regardless]
* `320-34-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even regardless whether a processor uses float]
* `320-35-BindCalculationInferDecimals-instance.xbrl` [Same as V-34 but sum is 2.9 so if rounding were in float, the floating inprecision would make the 'bad' rounding look correct, whereas if rounding were in decimal, it would be invalid.]
## Internationalization of xlink:href content [Test the implementation of converting the xlink:href from the represented character string into the right URL for file names and the content of ID attributes]
* `321-01-internationalization-instance-invalid.xml` [321-01 the instance document refers to a taxonomy that contains item definitions using Spanish characters. The item definitions are references from the linkbases. The instance should be considered valid but inconsistent according to the relationships in the calculation linkbase]
* `321-01-internationalization-instance-valid.xml` [321-01 the instance document refers to a taxonomy that contains item definitions using Spanish characters. The item definitions are references from the linkbases. The instance should be considered valid and consistent according to the relationships in the calculation linkbase]
## XML-XBRL Interaction [Checks interaction between XML infoset and XBRL validation for decimals and precision, which are option in schema, but in this case are entered to infoset by default and fixed for calculation binding rules 5.2.5.2 and 4.6.6]
* `322-00-BindCalculationDefaultDecimals-instance.xbrl` [322.00 - Valid]
* `322-01-BindCalculationDefaultDecimals-instance.xbrl` [322.01 - Invalid]
* `322-02-BindCalculationDefaultPrecision-instance.xbrl` [322.02 - Valid]
* `322-03-BindCalculationDefaultPrecision-instance.xbrl` [322.03 - Invalid]
* `322-04-BindCalculationFixedDecimals-instance.xbrl` [322.04 - Valid]
* `322-05-BindCalculationFixedPrecision-instance.xbrl` [322.05 - Invalid]
## s-equal tests [Test equvalent relationships processing]
* `331-equivalentRelationships-instance-01.xml` [t:P1 is a summation of t:P2 and t:P3.  The contributing items have a calculation inconsistency.  Following tests will use relationship equivalence to prohibit (or not be successful at prohibiting) P3 so the sum is clean (or inconsistent if unsuccessful at prohibiting), and thus the test case determines if equivalency matches the spec (by prohibition successfulness in subsequent tests).]
* `331-equivalentRelationships-instance-02.xml` [Same as V-01 but t:P3 calculation arc is with an arc prohibited with nothing tricky, thus avoiding the calculation inconsistency.  The prohibiting arc has the same arcrole, from, to, order, weight, t: attributes and use=prohibited.]
* `331-equivalentRelationships-instance-03.xml` [Same as V-02 but prohibiting arc has different weight causing nonequivalency and thus the prohibit is ineffective and calculation is inconsistent.]
* `331-equivalentRelationships-instance-04.xml` [Same as V-02 but prohibiting arc has has an xmlns not on the original arc, which is exempt.  Also the xmlns provides different lexical prefixes for the home-made attributes on the arc.]
* `331-equivalentRelationships-instance-05.xml` [Same as V-02 but prohibiting arc has has an xlink:title not on the original arc, which is exempt.  ]
* `331-equivalentRelationships-instance-06.xml` [Same as V-02 but prohibiting arc has has an default attribute matching the default value (the original arc has the default valued attribute missing).  Because the arc allows this attribute with an any wildcard attribute definition, it is not considered an explicit attribute use in the schema declared attributes, and thus it the validator has no way of knowing this default attribute might be put into the PSVI by the validator, and is simply absent on the original arc in the PSVI.  Thus the equivalency test fails here.]
* `331-equivalentRelationships-instance-07.xml` [Same as V-02 but prohibiting arc has has an defaulted attribute whose value does not match the default value (of the original arc, where the default valued attribute was missing, and per V-06 didn't enter the post-schema-validation infoset of attributes of the original arc).]
* `331-equivalentRelationships-instance-08.xml` [Same as V-02 but prohibiting arc has an fixed attribute of the correct fixed value (the original arc has the fixed valued attribute missing). Per testing by several vendors, the fixed is determined to behave like default, and thus this attribute is not present on the original arc and causes nonequivalence.]
* `331-equivalentRelationships-instance-09.xml` [Same as V-02 but prohibiting arc has the stringAttr differently valued.]
* `331-equivalentRelationships-instance-10.xml` [Same as V-02 but prohibiting arc has the decimalAttr lexically different but same value.]
* `331-equivalentRelationships-instance-11.xml` [Same as V-02 but prohibiting arc has the doubleAttr lexically different and scaled differently but same value.]
* `331-equivalentRelationships-instance-12.xml` [Same as V-02 but prohibiting arc has the doubleAttr lexically different and scaled differently to produce a different value.]
* `331-equivalentRelationships-instance-13.xml` [Same as V-02 but prohibiting arc has the boolAttr lexically different but same value (e.g., 1 == true, 0 == false).]
## s-equal tests [Test s-equal processing]
* `330-s-equal-instance-01.xml` [t:P1 is a summation of t:P2 and t:P3.  The contributing items have identical contextRef and thus there is no calculation inconsistency.  The context has a scenario contrived to show nesting, attributes, and elements for s-equality testing purposes in subsequent variations.]
* `330-s-equal-instance-02.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context is missing the scenario of the summation item and other contributing item, thus causing calculation inconsistency.]
* `330-s-equal-instance-03.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has a different t:strVal within its context scenario, thus causing calculation inconsistency. (Prior versions of V-03 and following had different nested context element ID attributes and expected them to be ignored, per bug 378, but now deemed significant, so nested IDs have been removed to effectuate the desired testing of this and following variations.)]
* `330-s-equal-instance-04.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has a ordering of t:strVal and t:decVal within its context scenario, thus causing calculation inconsistency.]
* `330-s-equal-instance-05.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has a different parenting of t:strVal within its context scenario, thus causing calculation inconsistency.]
* `330-s-equal-instance-06.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has a different attribute a1 on nested element t:scenarioVal within its context scenario, thus causing calculation inconsistency.]
* `330-s-equal-instance-07.xml` [t:P1 is a summation of t:P2 and t:P3.  According to the 2006-12-18 spec, t:P2's context would be s-equal to that of t:P1 and t:P3 were it not for the id attributes in the scenario.  Current 2.1 spec treats these IDs as significant, see bug 378.  Thus this test is invalid.]
* `330-s-equal-instance-08.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has the same scenario, but attributes on element dv2-v02 have been re-ordered, but it is still s-equal to the context of t:P1 and t:P3, so the calculation is valid.  (Only the id attribute on the context differs, for the moment, id attributes have been removed from subelements.)]
* `330-s-equal-instance-12.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has the same scenario, but doubles and booleans test lexical representations by scaling, though values are the same.]
* `330-s-equal-instance-13.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has the same scenario, but numbers test lexical signing representations, + optional on positive, and +0 equal to -0.]
* `330-s-equal-instance-14.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has the same scenario, but doubles and test lexical non-number representations, INF.]
* `330-s-equal-instance-15.xml` [t:P1 is a summation of t:P2 and t:P3.  t:P2's context has the same scenario, but doubles and test lexical non-number representations, INF. Attribute a3 differs, to check that + infinity and - infinity are detected as unequal.]