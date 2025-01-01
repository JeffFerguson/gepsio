# Welcome

Welcome to Gepsio **2.1.0.19**. Gepsio is a document object model for [XBRL](https://www.xbrl.org/) documents. The object model is built using .NET 8 and will work with any software development platform that can consume .NET 8 assemblies.

Load your XBRL document with the `XbrlDocument` class and work with your XBRL document exposed as a set of .NET 8 classes with a variety of properties and methods. Loaded XBRL documents are automatically validated against the information against the XBRL specification, and exceptions are thrown when invalid XBRL documents are loaded. The Gepsio code base is unit tested using the [XBRL Conformance Suite](https://specifications.xbrl.org/release-history-base-spec-conformance-suite.html) designed by the XBRL organization.

The [Wiki](https://github.com/JeffFerguson/gepsio/wiki) area of [the Github repository for Gepsio](https://github.com/JeffFerguson/gepsio/) includes a section called "Working with Gepsio" that describes how to use Gepsio to work with XBRL document instances.

# Design

- Gepsio now supports the direct loading of schemas and linkbase documents hosted on `sec.gov`. Previous versions of Gepsio supported the loading of XBRL document instances hosted on `sec.gov`, but did not support the loading of any ancillary documents also hosted on `sec.gov`.

# Bug Fixes

- Fixed a bug which caused XBRL document instances hosted on `sec.gov` to report schema load errors and report that no facts are available in the instance. See the Design note for more information. This fixes [Issue 56](https://github.com/JeffFerguson/gepsio/issues/56).
- Fixed a bug which caused calls to `GetHashCode()` on schema `Element` objects without explicitly defined `ID` attributes to throw a `NullReferenceException`. This fixes [Issue 57](https://github.com/JeffFerguson/gepsio/issues/57).

# Breaking Changes

- No changes from the previous release.

# Conformance

- No changes from the previous release.

# Industry-Standard Schema Support

- No changes from the previous release. The Wiki page "[Support for Automatic Loading of Industry Standard Schemas](https://github.com/JeffFerguson/gepsio/wiki/Support-for-Automatic-Loading-of-Industry-Standard-Schemas)" provides more information about the need for, and design of, the support for industry-standard schemas.

# Conformance Tests In XBRL-CONF-2014-12-10 Passed by Gepsio

Tests marked **_NEW_** failed in previous releases but now pass due to Gepsio's increased conformance with the XBRL 2.1 Specification.

## Identifier Scope [Section 4.3 The Item Element]

- `301-01-IdScopeValid.xml` [301.01 Valid example of id attribute.]
- `301-03-IdScopePeriodDiff.xml` [301.03 Mismatch of periodType attribute and referenced context's period type.]
- `301-04-IdScopeContextRefToUnit.xml` [301.04 contextRef has IDREF to unit element.]
- `301-05-IdScopeUnitRefToContext.xml` [301.05 unitRef has IDREF to context element.]
- `301-06-FootnoteScopeValid.xml` [301.06 Valid example of footnote link href attribute.]
- `301-08-FootnoteToContext.xml` [301.08 href attribute is referencing to context element.]
- `301-09-FootnoteToUnit.xml` [301.09 href attribute is referencing to unit element.]
- `301-10-FootnoteFromOutOfScope.xml` [301.10 The instance contains two footnote links. The second one contains an arc with a from value that does not have a corresponding loc in the same extended link.]
- `301-11-FootnoteToOutOfScope.xml` [301.11 The instance contains two footnote links. The second one contains an arc with a to value that does not have a corresponding footnote resource in the same extended link.]
- `301-12-FootnoteLocOutOfScope.xml` [301.12 The instance contains a footnote link. In the footnote link there is a loc element that has an href that points to a fact in another instance document.]
- `301-13-FootnoteLocInScope.xml` [301.13 The instance contains a footnote link. In the footnote link there is a loc element that has an href that points to a fact in the instance document using the instance document file name with a fragment identifier.]
- `301-14-FootnoteFromResource.xml` [301.14 The instance contains a footnote link. The arc in the footnote link has a from value that matches a footnote resource. This is not valid for the fact-footnote arc role on the arc. The from must point to a loc which in turns points to a fact in the same instance document.]
- `301-15-FootnoteToLoc.xml` [301.15 The instance contains a footnote link. The arc in the footnote link has a from value that matches a footnote resource. This is not valid for the fact-footnote arc role on the arc. The from must point to a loc which in turns points to a fact in the same instance document.]
- `301-16-FootnoteWithoutLang.xml` [301.16 The xml:lang attribute is missing on the footnote resource.]
- `301-17-FootnoteCustomArcRole.xml` [301.17 The footnote custom arc role can relate two footnotes to each other instead of just fact to footnote. (Only the standard footnote arc role is restricted to being from item or tuple locators.) Maybe this might be used to indicate how some footnote is "footnoting" another footnote.]

## Context Segments and Scenarios [Section 4.4 The Context Element]

- `302-01-SegmentValid.xml` [302.01 Valid segment in a context]
- `302-02-SegmentNamespaceInvalid.xml` [302.02 Invalid segment in a context; contains an element defined in xbrli namespace]
- `302-03-ScenarioValid.xml` [302.03 Valid scenario in a context]
- `302-04-ScenarioNamespaceInvalid.xml` [302.04 Invalid scenario in a context; contains an element defined in xbrli namespace]
- `302-05-SegmentSubstitutionInvalid.xml` [302.05 Invalid segment in a context; contains an element in substitution group of xbrli:item]
- `302-06-ScenarioSubstitutionInvalid.xml` [302.06 Invalid scenario in a context; contains an element in substitution group of xbrli:item]
- `302-07-SegmentEmptyContent.xml` [302.07 Segment in a context contains an element with empty content]
- `302-08-ScenarioEmptyContent.xml` [302.08 Scenario in a context contains an element with empty content]
- `302-09-PeriodDateTimeValid.xml` [302.09 Valid duration context with start date earlier than end date]
- `302-10-PeriodDateTimeInvalid.xml` [302.10 Invalid duration context with start date later than end date]
- `302-11-DecimalAttributeOnSegmentInconsistent.xbrl` [302.11 Two contexts are S-Equal even though a decimal-valued attribute in their segment elements have different lexical representations. The contexts are S-equal, so a calculation inconsistency MUST be signaled.]
- `302-12-DecimalAttributeOnScenarioInconsistent.xbrl` [302.12 Two contexts are S-Equal even though a decimal-valued attribute in their scenario elements have different lexical representations. The contexts are S-equal, so a calculation inconsistency MUST be signaled.]

## Period Type Consistency [Section 4.3 The Item Element]

- `303-01-PeriodInstantValid.xml` [303.01 instant context and item defined with PeriodType="instant"]
- `303-02-PeriodDurationValid.xml` [303.02 duration context and item defined with PeriodType="duration"]
- `303-03-PeriodInstantInvalid.xml` [303.03 duration context and item defined with PeriodType="instant"]
- `303-04-PeriodDurationInvalid.xml` [303.04 instant context and item defined with PeriodType="duration"]
- `303-05-ForeverElementewithInstancePeriodTypeReportedasForever.xbrl` [ForeverConcept with Instant Period Type is not allowed]

## Unit of Measure Consistency [Section 4.4 The Context Element]

- `304-01-monetaryItemTypeUnitsRestrictions.xml` [304.01 An element with a monetary item type has an ISO currency code for its units (using the standard ISO namespace prefix).]
- `304-02-monetaryItemTypeUnitsRestrictions.xml` [304.02 An element with a monetary item type has an ISO currency code for its units (using a non-standard ISO namespace prefix).]
- `304-03-monetaryItemTypeUnitsRestrictions.xml` [304.03 An element with a type derived by restriction from the monetary item type has an ISO currency code for its units.]
- `304-04-monetaryItemTypeUnitsRestrictions.xml` [304.04 An element with a type derived by restriction from monetary item type has an ISO currency code for its units (using a non-standard ISO namespace prefix).]
- `304-05-monetaryItemTypeUnitsRestrictions.xml` [304.05 An element with a non-monetary item type has an ISO currency code for its units (using the standard ISO namespace prefix).]
- `304-06-monetaryItemTypeUnitsRestrictions.xml` [304.06 An element with a monetary item type does not have an ISO currency code for its units - the namespace is wrong.]
- `304-07-monetaryItemTypeUnitsRestrictions.xml` [304.07 An element with a monetaryItemType does not have an ISO currency code for its units - the local name is wrong.]
- `304-08-monetaryItemTypeUnitsRestrictions.xml` [304.08 An element with a type derived by restriction from monetaryItemType does not have an ISO currency code for its units - the namespace is wrong.]
- `304-09-monetaryItemTypeUnitsRestrictions.xml` [304.09 An element with a type derived by restriction from monetaryItemType does not have an ISO currency code for its units - the local name is wrong.]
- `304-10-pureItemTypeUnitsRestrictions.xml` [304.10 An item with a pureItemType data type MUST have a unit element and the local part of the measure MUST be "pure" with a namespace prefix that resolves to a namespace of "http://www.xbrl.org/2003/instance".]
- `304-11-pureItemTypeUnitsRestrictions.xml` [A measure element with a namespace prefix that resolves to the "http://www.xbrl.org/2003/instance" namespace MUST have a local part of either "pure" or "shares". The value 'impure' is not a valid measure in the XBRL instance namespace.]
- `304-12-pureItemTypeUnitsRestrictions.xml` [Unlike for monetaryItemType and sharesItemType, there is no constraint (in 4.8.2 or elsewhere) requiring an item with a pureItemType data type to have a particular kind of unit.]
- `304-12a-pureItemTypeUnitsRestrictions.xml` [Same as V-12, but the pure measure has no prefix and the default namespace is undefined.]
- `304-13-sharesItemTypeUnitsRestrictions.xml` [304.13 For facts that are of the sharesItemType, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance]
- `304-14-sharesItemTypeUnitsRestrictions.xml` [304.14 For facts that are DERIVED BY RESTRICTION from the sharesItemType, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance]
- `304-15-pureItemTypeUnitsRestrictions.xml` [304.15 For facts that are of shares item type, units MUST have A single measure element. The local part of the measure MUST be "shares" and the namespace prefix that MUST resolve to http://www.xbrl.org/2003/instance. In this case the unit has two measure elements, both of which are pure.]
- `304-15a-sharesItemTypeUnitsRestrictions.xml` [Same as V-15 but in this case the unit has has shares but no prefix and the default namespace is undefined.]
- `304-16-unitsInSimplestForm.xml` [304.16 The units must not have numerator and denominator measures that cancel.]
- `304-17-sameOrderMeasuresValid.xml` [304.17 The units equality test which two units have same order measures.]
- `304-18-sameOrderDivisionMeasuresValid.xml` [304.18 The units equality test which two units have same order divisions.]
- `304-19-differentOrderMeasuresValid.xml` [304.19 The units equality test which two units have different order measures.]
- `304-20-differentOrderDivisionMeasuresValid.xml` [304.20 The units equality test which two units have division elements which their order of child measures are different.]
- `304-21-measuresInvalid.xml` [304.21 it tries to essence-alias equality of two elements with different units : where one is pure-feet and the second is pure-pounds. so the alias essence check is invalid and it should throw an error in xbrl validation]
- `304-22-divisionMeasuresInvalid.xml` [304.22 The test tried to essense-alias equality check of two elements with different units : where one is unit between "pure-inch / pound-feet" and other "pure-feet / pound-inch". The tests is invalid as it should throw an error during xbrl validation.]
- `304-23-Calculation-item-does-not-match-unit.xml` [Variation of 304-15 where the type of the fact value does not match that of the type of the reported element. Shares type versus Monetary unit]
- `304-24-valid-ISO-unit-of-measue.xml` [Valid ISO unit of measurement example]
- `304-25-measure-reported-with-prefix-undefined-instance.xbrl` [Measure reported with prefix undefined is considered XBRL invalid]
- `304-26-monetaryItemTypeUnitsRestrictions.xml` [Monetary item reported with unit having a denominator.]

## Decimal and Precision Mutual Exclusion and prohibition on nil items [Section 4.4 Items]

- `305-01-DecimalOnlySpecified.xml` [305.01 item has only Decimals specified]
- `305-02-PrecisionOnlySpecified.xml` [305.02 item has only Precision specified]
- `305-03-NoDecimalOrPrecisionSpecified.xml` [305.03 item has neither Decimals nor Precision specified]
- `305-04-BothDecimalAndPrecisionSpecified.xml` [305.04 item has both Decimals and Precision specified]
- `305-05-DecimalSpecifiedOnNilItem.xml` [305.05 nil item has Decimals specified]
- `305-06-PrecisionSpecifiedOnNilItem.xml` [305.06 nil item has Precision specified]
- `305_07_invalid_instance.xbrl` [305.07 a genuine inconsistency due to roll up of child values]
- `305-08-UnitsSpecifiedOnNilItem.xml` [305.08 nil items have no decimals or precision, with unitref, but the type specifies fixed values for decimals and precision.]

## Required Arc in Definition Linkbase [Section 5.5.6.4.1.5]

- `306-01-RequiredInstanceValid.xml` [306.01 The instance contains two elements in the same context. The presence of one element forces the presence of the other.]
- `306-02-RequiredInstanceTupleValid.xml` [306.02 The instance contains an item and a tuple. The presence of the tuple forces the presence of the item.]
- `306-03-RequiredInstanceInvalid.xml` [306.03 The instance contains an item and a tuple. The presence of the tuple forces the presence of the item.]

## Schema References [Section 5 Taxonomies]

- `307-01-SchemaRefExample.xml` [307.01 A schemaRef element MUST hold the URI of a schema. In this case it does.]
- `307-02-SchemaRefCounterExample.xml` [307.01 A schemaRef element MUST hold the URI of a schema. In this case it does not because the second reference to a schema actually points to an XML document that is a label linkbase. ]
- `307-03-SchemaRefXMLBase.xml` [307.03 schemaRef elements MUST hold the URI of Schemas. In this case the requirement is not satisfied because the schema reference has to be resolved using the XML base attribute that ensures the schemaRef URI resolves to the XML document in the base directory. This document, however, is a label linkbase, not a schema. If the XML base attribute value is not used then the schema in the same directory as the instance is discovered and no issues are noticed.]

## Duplicate instance roleRef and duplicate arcroleRefs [3.5.2.4.5 and 3.5.2.5.5 duplicate instance roleRef and arcroleRef elements.]

- `308-01-instance.xml` [Instance contains two role references to the same URI, INVALID]
- `308-02-instance.xml` [Instance contains two arcrole references to the same URI, INVALID]

## LAX validation tests [Test that LAX validation is performed]

- `314-lax-validation-01.xml` [Segment has an element for which there is no definition, so it is allowed; item has an attribute with no definition, so it is allowed. The definitions are imported to the discovered taxonomy.]
- `314-lax-validation-02.xml` [Segment has an element for which there is no definition, so it is allowed; item has an attribute with no definition, so it is allowed. The definitions are found by schemaLocation from the instance document.]
- `314-lax-validation-03.xml` [Same as v-01 but segment has an element defined as integer with string contents]
- `314-lax-validation-04.xml` [Same as v-01 but item has an attribute defined as integer with string contents]
- `314-lax-validation-05.xml` [Same as v-02 but segment has an element defined as integer with string contents]
- `314-lax-validation-06.xml` [Same as v-02 but item has an attribute defined as integer with string contents]

## Identifier Scope [Relevant sections for calculation binding rules 5.2.5.2 and 4.6.6]

- `320-00-BindCalculationInferPrecision-instance.xbrl` [320.00 - Valid]
- `320-01-BindCalculationInferPrecision-instance.xbrl` [320.01 - Valid]
- `320-02-BindCalculationInferPrecision-instance.xbrl` [320.02 - InValid]
- `320-03-nestedtupleBindCalculationInferPrecision-instance.xbrl` [320.03 - Valid]
- `320-04-BindCalculationInferPrecision-instance.xbrl` [320.04 - Valid]
- `320-05-BindCalculationInferPrecision-instance.xbrl` [320.05 - Valid]
- `320-06-BindCalculationInferPrecision-instance.xbrl` [320.06 - inValid - inconsistent due to precision 0]
- `320-07-BindCalculationInferPrecision-instance.xbrl` [320.07 - inValid - inconsistent due to precision 0]
- `320-08-BindCalculationInferPrecision-instance.xbrl` [320.08 - inValid - inconsistent due to precision 0]
- `320-09-BindCalculationInferPrecision-instance.xbrl` [320.09 - Valid]
- `320-10-BindCalculationInferPrecision-instance.xbrl` [320.10 - Valid - precision stated as 15]
- `320-11-BindCalculationInferPrecision-instance.xbrl` [320.11 - inValid - inconsistent]
- `320-12-BindCalculationInferPrecision-instance.xbrl` [320.12 - Valid - Consistent - precision attribute stated as 15]
- `320-13-BindCalculationInferPrecision-instance.xbrl` [320.13 - Valid - Consistent]
- `320-14-BindCalculationInferPrecision-instance.xbrl` [320.14 - Valid - Consistent]
- `320-15-BindCalculationInferPrecision-instance.xbrl` [320.15 - Valid - COnsistent - Decimal attribute zero]
- `320-16-BindCalculationInferPrecision-instance.xbrl` [320.16 - InValid - Inconsistency contributing items 3200, summation value is 3201]
- `320-17-BindCalculationInferPrecision-instance.xbrl` [320.17 - InValid - Inconsistent roll up - weights stated as 1.0]
- `320-18-BindCalculationInferPrecision-instance.xbrl` [320.18 - InValid - 2.04 effective value generates inconsistent roll up - weight is defined as 1.01]
- `320-19-BindCalculationInferPrecision-instance.xbrl` [320.19 - InValid - effective value 1.02 generates inconsistent roll up - weight is defined as 1.01]
- `320-20-BindCalculationInferPrecision-instance.xbrl` [320.20 - Valid - Consistent; Weight is applied to the value after the decimal or precision is applied to the lexical value reported in the instance; Arc weights are defined with the value 1.01; Test ensures the sequence that we apply these rules of inferred precision and weight attribute]
- `320-21-BindCalculationInferPrecision-instance.xbrl` [320.21 - Duplicate Facts reported and thus calculation will not bind per 5.2.5.2; Test ensures that we do not bind the calculation due to a duplicated fact and do not infer precision.]
- `320-22-BindCalculationInferPrecision-instance.xbrl` [320.22 - Duplicate Facts reported and thus calculation will not bind per 5.2.5.2; Test ensures that we Do NOT bind the calculation and do not infer precision. If it did bind then there would be an inconsistency.]
- `320-23-BindCalculationInferPrecision-instance.xbrl` [320-24 - Valid - Consistent; Inferred precision and Weight Attribute test]
- `320-24-BindCalculationInferPrecision-instance.xbrl` [320-24 - Valid - Consistent; Inferred precision and Weight Attribute test in correct sequence]
- `320-25-BindCalculationInferPrecision-instance.xbrl` [320-25 - Invalid; Test: Verifies that IsNill facts do not bind to calculations per 5.2.5.2]
- `320-26-BindCalculationInferPrecision-instance.xbrl` [320-26 - Valid, but contains number likely to sneak in errors in floating point multiplication of the weight 0.45 x A113 (=1010103). For non-decimal CPU arithmetic rounding may be required on each weight multiplication to avoid floating point weirdnesses. (this irritation contributed by Herm Fischer)]
- `320-30-BindCalculationInferDecimals-instance.xbrl` [Tests that a decimals 0 value 0 is not treated as precision 0 (invalid) but as numeric zero. In the prior approach where decimals 0 value 0 converted to precision 0 value 0, this would have been invalid.]
- `320-31-BindCalculationInferDecimals-instance.xbrl` [Edge case tests that decimal rounding with is performed. In decimals representations, summation 0d1 is compared to contributing items 1.234d2 and -1.223d2. Contributing items round to 1.23 and -1.22, the contributing items rounded sum is .01. Summation has decimals 1 which is applied to comparing 0 decimals 1 to contributing items rounded sum .01, but in decimals 1, which makes the contributing rounded sum .01 be rounded as .0, which is equal to summation zero decimals 1, in decimals. If the calculation were performed the old way, treating summation 0d1 as 0p0 this alone would make the calculation roll up be invalid. But many processors have anyway treated 0d1 as 0p1, in which case such a processor still would be comparing numeric 0 for the summation, to the rounded contributing items sum .01 in some non-zero precision, which would still be invalid. So the point of this test is to be sure the processor is using the all-decimals calculation roll up.]
- `320-32-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even]
- `320-33-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even regardless]
- `320-34-BindCalculationInferDecimals-instance.xbrl` [Checks that .5 rounds half to nearest even regardless whether a processor uses float]
- `320-35-BindCalculationInferDecimals-instance.xbrl` [Same as V-34 but sum is 2.9 so if rounding were in float, the floating inprecision would make the 'bad' rounding look correct, whereas if rounding were in decimal, it would be invalid.]

## Internationalization of xlink:href content [Test the implementation of converting the xlink:href from the represented character string into the right URL for file names and the content of ID attributes]

- `321-01-internationalization-instance-invalid.xml` [321-01 the instance document refers to a taxonomy that contains item definitions using Spanish characters. The item definitions are references from the linkbases. The instance should be considered valid but inconsistent according to the relationships in the calculation linkbase]
- `321-01-internationalization-instance-valid.xml` [321-01 the instance document refers to a taxonomy that contains item definitions using Spanish characters. The item definitions are references from the linkbases. The instance should be considered valid and consistent according to the relationships in the calculation linkbase]

## XML-XBRL Interaction [Checks interaction between XML infoset and XBRL validation for decimals and precision, which are option in schema, but in this case are entered to infoset by default and fixed for calculation binding rules 5.2.5.2 and 4.6.6]

- `322-00-BindCalculationDefaultDecimals-instance.xbrl` [322.00 - Valid]
- `322-01-BindCalculationDefaultDecimals-instance.xbrl` [322.01 - Invalid]
- `322-02-BindCalculationDefaultPrecision-instance.xbrl` [322.02 - Valid]
- `322-03-BindCalculationDefaultPrecision-instance.xbrl` [322.03 - Invalid]
- `322-04-BindCalculationFixedDecimals-instance.xbrl` [322.04 - Valid]
- `322-05-BindCalculationFixedPrecision-instance.xbrl` [322.05 - Invalid]

## s-equal tests [Test equvalent relationships processing]

- `331-equivalentRelationships-instance-01.xml` [t:P1 is a summation of t:P2 and t:P3. The contributing items have a calculation inconsistency. Following tests will use relationship equivalence to prohibit (or not be successful at prohibiting) P3 so the sum is clean (or inconsistent if unsuccessful at prohibiting), and thus the test case determines if equivalency matches the spec (by prohibition successfulness in subsequent tests).]
- `331-equivalentRelationships-instance-02.xml` [Same as V-01 but t:P3 calculation arc is with an arc prohibited with nothing tricky, thus avoiding the calculation inconsistency. The prohibiting arc has the same arcrole, from, to, order, weight, t: attributes and use=prohibited.]
- `331-equivalentRelationships-instance-03.xml` [Same as V-02 but prohibiting arc has different weight causing nonequivalency and thus the prohibit is ineffective and calculation is inconsistent.]
- `331-equivalentRelationships-instance-04.xml` [Same as V-02 but prohibiting arc has has an xmlns not on the original arc, which is exempt. Also the xmlns provides different lexical prefixes for the home-made attributes on the arc.]
- `331-equivalentRelationships-instance-05.xml` [Same as V-02 but prohibiting arc has has an xlink:title not on the original arc, which is exempt. ]
- `331-equivalentRelationships-instance-06.xml` [Same as V-02 but prohibiting arc has has an default attribute matching the default value (the original arc has the default valued attribute missing). Because the arc allows this attribute with an any wildcard attribute definition, it is not considered an explicit attribute use in the schema declared attributes, and thus it the validator has no way of knowing this default attribute might be put into the PSVI by the validator, and is simply absent on the original arc in the PSVI. Thus the equivalency test fails here.]
- `331-equivalentRelationships-instance-07.xml` [Same as V-02 but prohibiting arc has has an defaulted attribute whose value does not match the default value (of the original arc, where the default valued attribute was missing, and per V-06 didn't enter the post-schema-validation infoset of attributes of the original arc).]
- `331-equivalentRelationships-instance-08.xml` [Same as V-02 but prohibiting arc has an fixed attribute of the correct fixed value (the original arc has the fixed valued attribute missing). Per testing by several vendors, the fixed is determined to behave like default, and thus this attribute is not present on the original arc and causes nonequivalence.]
- `331-equivalentRelationships-instance-09.xml` [Same as V-02 but prohibiting arc has the stringAttr differently valued.]
- `331-equivalentRelationships-instance-10.xml` [Same as V-02 but prohibiting arc has the decimalAttr lexically different but same value.]
- `331-equivalentRelationships-instance-11.xml` [Same as V-02 but prohibiting arc has the doubleAttr lexically different and scaled differently but same value.]
- `331-equivalentRelationships-instance-12.xml` [Same as V-02 but prohibiting arc has the doubleAttr lexically different and scaled differently to produce a different value.]
- `331-equivalentRelationships-instance-13.xml` [Same as V-02 but prohibiting arc has the boolAttr lexically different but same value (e.g., 1 == true, 0 == false).]

## s-equal tests [Test s-equal processing]

- `330-s-equal-instance-01.xml` [t:P1 is a summation of t:P2 and t:P3. The contributing items have identical contextRef and thus there is no calculation inconsistency. The context has a scenario contrived to show nesting, attributes, and elements for s-equality testing purposes in subsequent variations.]
- `330-s-equal-instance-02.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context is missing the scenario of the summation item and other contributing item, thus causing calculation inconsistency.]
- `330-s-equal-instance-03.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has a different t:strVal within its context scenario, thus causing calculation inconsistency. (Prior versions of V-03 and following had different nested context element ID attributes and expected them to be ignored, per bug 378, but now deemed significant, so nested IDs have been removed to effectuate the desired testing of this and following variations.)]
- `330-s-equal-instance-04.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has a ordering of t:strVal and t:decVal within its context scenario, thus causing calculation inconsistency.]
- `330-s-equal-instance-05.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has a different parenting of t:strVal within its context scenario, thus causing calculation inconsistency.]
- `330-s-equal-instance-06.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has a different attribute a1 on nested element t:scenarioVal within its context scenario, thus causing calculation inconsistency.]
- `330-s-equal-instance-07.xml` [t:P1 is a summation of t:P2 and t:P3. According to the 2006-12-18 spec, t:P2's context would be s-equal to that of t:P1 and t:P3 were it not for the id attributes in the scenario. Current 2.1 spec treats these IDs as significant, see bug 378. Thus this test is invalid.]
- `330-s-equal-instance-08.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but attributes on element dv2-v02 have been re-ordered, but it is still s-equal to the context of t:P1 and t:P3, so the calculation is valid. (Only the id attribute on the context differs, for the moment, id attributes have been removed from subelements.)]
- `330-s-equal-instance-12.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but doubles and booleans test lexical representations by scaling, though values are the same.]
- `330-s-equal-instance-13.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but numbers test lexical signing representations, + optional on positive, and +0 equal to -0.]
- `330-s-equal-instance-14.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but doubles and test lexical non-number representations, INF.]
- `330-s-equal-instance-15.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but doubles and test lexical non-number representations, INF. Attribute a3 differs, to check that + infinity and - infinity are detected as unequal.]
- `330-s-equal-instance-16.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but attributes a3 in both contexts are NaN which is always unequal to all values including itself.]
  Exception thrown: 'System.UriFormatException' in System.Private.Uri.dll
- `330-s-equal-instance-17.xml` [t:P1 is a summation of t:P2 and t:P3. t:P2's context has the same scenario, but booleans test lexical representations, 0 or 1 for false or true.]

## Infer Decimals and Precision [Section 4.4 Items]

- `391-01-InferPrecisionFromDecimals.xml` [Test variation 391-01 using a base lexical representation of .0000 for the data value with various exponent parts]
- `391-02-InferPrecisionFromDecimals.xml` [Test variation 391-02 using a base lexical representation of 0.0000 for the data value with various exponent parts]
- `391-03-InferPrecisionFromDecimals.xml` [Test variation 391-03 using a base lexical representation of 0. for the data value with various exponent parts]
- `391-04-InferPrecisionFromDecimals.xml` [Test variation 391-04 using a base lexical representation of 0000. for the data value with various exponent parts]
- `391-05-InferPrecisionFromDecimals.xml` [Test variation 391-05 using a base lexical representation of 0 for the data value with various exponent parts]
- `391-06-InferPrecisionFromDecimals.xml` [Test variation 391-06 using a base lexical representation of 0000 for the data value with various exponent parts]
- `391-07-InferPrecisionFromDecimals.xml` [Test variation 391-07 using a base lexical representation of .001234 for the data value with various exponent parts]
- `391-08-InferPrecisionFromDecimals.xml` [Test variation 391-08 using a base lexical representation of 0.001234 for the data value with various exponent parts]
- `391-09-InferPrecisionFromDecimals.xml` [Test variation 391-09 using a base lexical representation of 0000.001234 for the data value with various exponent parts]
- `391-10-InferPrecisionFromDecimals.xml` [Test variation 391-10 using a base lexical representation of .00123400 for the data value with various exponent parts]
- `391-11-InferPrecisionFromDecimals.xml` [Test variation 391-11 using a base lexical representation of 0.00123400 for the data value with various exponent parts]
- `391-12-InferPrecisionFromDecimals.xml` [Test variation 391-12 using a base lexical representation of 0000.00123400 for the data value with various exponent parts]
- `391-13-InferPrecisionFromDecimals.xml` [Test variation 391-13 using a base lexical representation of .1234 for the data value with various exponent parts]
- `391-14-InferPrecisionFromDecimals.xml` [Test variation 391-14 using a base lexical representation of 0.1234 for the data value with various exponent parts]
- `391-15-InferPrecisionFromDecimals.xml` [Test variation 391-15 using a base lexical representation of 0000.1234 for the data value with various exponent parts]
- `391-16-InferPrecisionFromDecimals.xml` [Test variation 391-16 using a base lexical representation of .123400 for the data value with various exponent parts]
- `391-17-InferPrecisionFromDecimals.xml` [Test variation 391-17 using a base lexical representation of 0.123400 for the data value with various exponent parts]
- `391-18-InferPrecisionFromDecimals.xml` [Test variation 391-18 using a base lexical representation of 0000.123400 for the data value with various exponent parts]
- `391-19-InferPrecisionFromDecimals.xml` [Test variation 391-19 using a base lexical representation of 1234 for the data value with various exponent parts]
- `391-20-InferPrecisionFromDecimals.xml` [Test variation 391-20 using a base lexical representation of 001234 for the data value with various exponent parts]
- `391-21-InferPrecisionFromDecimals.xml` [Test variation 391-21 using a base lexical representation of 001234. for the data value with various exponent parts]
- `391-22-InferPrecisionFromDecimals.xml` [Test variation 391-22 using a base lexical representation of 1234. for the data value with various exponent parts]
- `391-23-InferPrecisionFromDecimals.xml` [Test variation 391-23 using a base lexical representation of 1234.0000 for the data value with various exponent parts]
- `391-24-InferPrecisionFromDecimals.xml` [Test variation 391-24 using a base lexical representation of 001234.0000 for the data value with various exponent parts]
- `391-25-InferPrecisionFromDecimals.xml` [Test variation 391-25 using a base lexical representation of 123400 for the data value with various exponent parts]
- `391-26-InferPrecisionFromDecimals.xml` [Test variation 391-26 using a base lexical representation of 00123400 for the data value with various exponent parts]
- `391-27-InferPrecisionFromDecimals.xml` [Test variation 391-27 using a base lexical representation of 123400. for the data value with various exponent parts]
- `391-28-InferPrecisionFromDecimals.xml` [Test variation 391-28 using a base lexical representation of 00123400. for the data value with various exponent parts]
- `391-29-InferPrecisionFromDecimals.xml` [Test variation 391-29 using a base lexical representation of 123400.0000 for the data value with various exponent parts]
- `391-30-InferPrecisionFromDecimals.xml` [Test variation 391-30 using a base lexical representation of 00123400.0000 for the data value with various exponent parts]
- `391-31-InferPrecisionFromDecimals.xml` [Test variation 391-31 using a base lexical representation of 1234.001234 for the data value with various exponent parts]
- `391-32-InferPrecisionFromDecimals.xml` [Test variation 391-32 using a base lexical representation of 001234.001234 for the data value with various exponent parts]
- `391-33-InferPrecisionFromDecimals.xml` [Test variation 391-33 using a base lexical representation of 123400.001234 for the data value with various exponent parts]
- `391-34-InferPrecisionFromDecimals.xml` [Test variation 391-34 using a base lexical representation of 00123400.001234 for the data value with various exponent parts]
- `391-35-InferPrecisionFromDecimals.xml` [Test variation 391-35 using a base lexical representation of 1234.00123400 for the data value with various exponent parts]
- `391-36-InferPrecisionFromDecimals.xml` [Test variation 391-36 using a base lexical representation of 001234.00123400 for the data value with various exponent parts]
- `391-37-InferPrecisionFromDecimals.xml` [Test variation 391-37 using a base lexical representation of 123400.00123400 for the data value with various exponent parts]
- `391-38-InferPrecisionFromDecimals.xml` [Test variation 391-38 using a base lexical representation of 00123400.00123400 for the data value with various exponent parts]
- `391-39-InferPrecisionFromDecimals.xml` [Test variation 391-39 using a base lexical representation of .001204 for the data value with various exponent parts]
- `391-40-InferPrecisionFromDecimals.xml` [Test variation 391-40 using a base lexical representation of 0.001204 for the data value with various exponent parts]
- `391-41-InferPrecisionFromDecimals.xml` [Test variation 391-41 using a base lexical representation of 0000.001204 for the data value with various exponent parts]
- `391-42-InferPrecisionFromDecimals.xml` [Test variation 391-42 using a base lexical representation of .00120400 for the data value with various exponent parts]
- `391-43-InferPrecisionFromDecimals.xml` [Test variation 391-43 using a base lexical representation of 0.00120400 for the data value with various exponent parts]
- `391-44-InferPrecisionFromDecimals.xml` [Test variation 391-44 using a base lexical representation of 0000.00120400 for the data value with various exponent parts]
- `391-45-InferPrecisionFromDecimals.xml` [Test variation 391-45 using a base lexical representation of .1204 for the data value with various exponent parts]
- `391-46-InferPrecisionFromDecimals.xml` [Test variation 391-46 using a base lexical representation of 0.1204 for the data value with various exponent parts]
- `391-47-InferPrecisionFromDecimals.xml` [Test variation 391-47 using a base lexical representation of 0000.1204 for the data value with various exponent parts]
- `391-48-InferPrecisionFromDecimals.xml` [Test variation 391-48 using a base lexical representation of .120400 for the data value with various exponent parts]
- `391-49-InferPrecisionFromDecimals.xml` [Test variation 391-49 using a base lexical representation of 0.120400 for the data value with various exponent parts]
- `391-50-InferPrecisionFromDecimals.xml` [Test variation 391-50 using a base lexical representation of 0000.120400 for the data value with various exponent parts]
- `391-51-InferPrecisionFromDecimals.xml` [Test variation 391-51 using a base lexical representation of 1204 for the data value with various exponent parts]
- `391-52-InferPrecisionFromDecimals.xml` [Test variation 391-52 using a base lexical representation of 001204 for the data value with various exponent parts]
- `391-53-InferPrecisionFromDecimals.xml` [Test variation 391-53 using a base lexical representation of 001204. for the data value with various exponent parts]
- `391-54-InferPrecisionFromDecimals.xml` [Test variation 391-54 using a base lexical representation of 1204. for the data value with various exponent parts]
- `391-55-InferPrecisionFromDecimals.xml` [Test variation 391-55 using a base lexical representation of 1204.0000 for the data value with various exponent parts]
- `391-56-InferPrecisionFromDecimals.xml` [Test variation 391-56 using a base lexical representation of 001204.0000 for the data value with various exponent parts]
- `391-57-InferPrecisionFromDecimals.xml` [Test variation 391-57 using a base lexical representation of 120400 for the data value with various exponent parts]
- `391-58-InferPrecisionFromDecimals.xml` [Test variation 391-58 using a base lexical representation of 00120400 for the data value with various exponent parts]
- `391-59-InferPrecisionFromDecimals.xml` [Test variation 391-59 using a base lexical representation of 120400. for the data value with various exponent parts]
- `391-60-InferPrecisionFromDecimals.xml` [Test variation 391-60 using a base lexical representation of 00120400. for the data value with various exponent parts]
- `391-61-InferPrecisionFromDecimals.xml` [Test variation 391-61 using a base lexical representation of 120400.0000 for the data value with various exponent parts]
- `391-62-InferPrecisionFromDecimals.xml` [Test variation 391-62 using a base lexical representation of 00120400.0000 for the data value with various exponent parts]
- `391-63-InferPrecisionFromDecimals.xml` [Test variation 391-63 using a base lexical representation of 1204.001204 for the data value with various exponent parts]
- `391-64-InferPrecisionFromDecimals.xml` [Test variation 391-64 using a base lexical representation of 001204.001204 for the data value with various exponent parts]
- `391-65-InferPrecisionFromDecimals.xml` [Test variation 391-65 using a base lexical representation of 120400.001204 for the data value with various exponent parts]
- `391-66-InferPrecisionFromDecimals.xml` [Test variation 391-66 using a base lexical representation of 00120400.001204 for the data value with various exponent parts]
- `391-67-InferPrecisionFromDecimals.xml` [Test variation 391-67 using a base lexical representation of 1204.00120400 for the data value with various exponent parts]
- `391-68-InferPrecisionFromDecimals.xml` [Test variation 391-68 using a base lexical representation of 001204.00120400 for the data value with various exponent parts]
- `391-69-InferPrecisionFromDecimals.xml` [Test variation 391-69 using a base lexical representation of 120400.00120400 for the data value with various exponent parts]
- `391-70-InferPrecisionFromDecimals.xml` [Test variation 391-70 using a base lexical representation of 00120400.00120400 for the data value with various exponent parts]

## Essence-Alias Closure [Section 5.5.7.15 The DefinitionArc Element]

- `392-01-EssenceAliasValid.xml` [392.01 Valid example of essence-alias attribute.]
- `392-02-EssenceAliasDuplicate.xml` [392.02 Valid example of essence-alias attribute using duplicate.]
- `392-03-EssenceAliasDuplicateNoEssence.xml` [392.03 Valid example of essence-alias attribute using duplicate. No copy happens.]
- `392-04-EssenceAliasReverse.xml` [392.04 Valid example of essence-alias attribute not to apply reverse direction.]
- `392-05-EssenceAliasValidWithValue.xml` [392.05 Valid example of essence-alias attribute.]
- `392-06-EssenceAliasInvalid.xml` [392.06 Invalid example of essence-alias attribute.]
- `392-07-EssenceAliasDifferentScopeValid.xml` [392.07 Valid example of essence-alias attribute. This testset has items set to essence-alias, but Essence item and alias items are located in other tuples, so even if these values are not identical but it is still valid.]
- `392-08-EssenceAliasDifferentScopeInValid.xml` [392.08 Invalid example of essence-alias attribute. This testset has items set to essence-alias, but Essence item and alias items in same tuple are not identical but other essence item in other tuple is identical. So scoping is wrong, it is invalid.]
- `392-09-EssenceAliasDifferentScopeValidWithValue.xml` [392.09 Valid example of essence-alias attribute. This testset has items set to essence-alias, One Essence item in same tuple with alias is identical and other essence items located in other tuples is no identical, By scoping, it is valid.]
- `392-10-EssenceAliasDifferentUnit.xml` [392.10 Invalid example of essence-alias attribute. This testset has numeric items set to essence-alias, but Essence item and alias item have different units. so it is invalid.]
- `392-11-EssenceAliasDifferentContext.xml` [392.11 Valid example of essence-alias attribute. This testset has items set to essence-alias, but Essence item and alias item are different values and units. so it is valid.]
- `392-13-EssenceAliasDifferentPeriodType.xml` [392.13 Invalid example of essence-alias attribute. This testset make an essence-alias relation between two items which are different period type.]
- `392-14-EssenceAliasNonNumericValid.xml` [392.14 Valid example of essence-alias attribute regarding nonNumericContext.]
- `392-15-EssenceAliasNonNumericInValid.xml` [392.15 invalid example of essence-alias attribute regarding nonNumericContext.]
- `392-16-EssenceAliasNonNumericTupleValid.xml` [392.16 Valid example of essence-alias attribute regarding nonNumericContext.]
- `392-17-EssenceAliasNonNumericTupleInValid.xml` [392.17 invalid example of essence-alias attribute regarding nonNumericContext.]

## Infer Calculated Value Consistency [Section 5.2.5.2 The CalculationArc Element]

- `395-01-InferCalculatedValueConsistencyValid.xml` [395.01 Valid example of summation-item attribute.]
- `395-02-InferCalculatedValueConsistencyDifferentValue.xml` [395.02 This is a test for detecting inconsistency between value in an Instance and value calculated by calculation link. This is set to invalid but the document validity itself is still valid.]
- `395-03-InferCalculatedValueConsistencyWithDefaultContributorValid.xml` [395.03 Valid example of summation-item in which a contributor is given a default value and appears in the instance with an empty value. The example is valid and there are no calculation inconsistencies.]
