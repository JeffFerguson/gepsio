using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An XML document containing one or more XBRL fragments.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An XBRL fragment is a fragment of XBRL data having an xbrl tag as its root. In the generic case,
    /// an XBRL document will have an xbrl tag as the root tag of the XML document, and, in this case,
    /// the entire XBRL document is one large XBRL fragment. However, section 4.1 of the XBRL 2.1 Specification
    /// makes provisions for multiple XBRL fragments to be stored in a single document:
    /// </para>
    /// <para>
    /// "If multiple 'data islands' of XBRL mark-up are included in a larger document, the xbrl element is
    /// the container for each [fragment]."
    /// </para>
    /// <para>
    /// Gepsio supports this notion by defining an XBRL document containing a collection of one or more
    /// XBRL fragments, as in the following code sample:
    /// </para>
    /// <code>
    /// var myDocument = new XbrlDocument();
    /// myDocument.Load("myxbrldoc.xml");
    /// foreach(var currentFragment in myDocument.XbrlFragments)
    /// {
    ///     // XBRL data is available from the "currentFragment" variable
    /// }
    /// </code>
    /// <para>
    /// In the vast majority of cases, an XBRL document will be an XML document with the xbrl tag at its
    /// root, and, as a result, the <see cref="XbrlDocument"/> uses to load the XBRL document will have
    /// a single <see cref="XbrlFragment"/> in the document's fragments container. Consider, however, the
    /// possibility of having more than one fragment in a document, in accordance of the text in section
    /// 4.1 of the XBRL 2.1 Specification.
    /// </para>
    /// </remarks>
    public class XbrlDocument
    {

        // namespace URIs

        internal static string XbrlNamespaceUri = "http://www.xbrl.org/2003/instance";
        internal static string XbrlLinkbaseNamespaceUri = "http://www.xbrl.org/2003/linkbase";
        internal static string XbrlDimensionsNamespaceUri = "http://xbrl.org/2005/xbrldt";
        internal static string XbrlEssenceAliasArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/essence-alias";
        internal static string XbrlGeneralSpecialArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/general-special";
        internal static string XbrlSimilarTuplesArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/similar-tuples";
        internal static string XbrlRequiresElementArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/requires-element";
        internal static string XbrlFactFootnoteArcroleNamespaceUri = "http://www.xbrl.org/2003/arcrole/fact-footnote";
        internal static string XbrlIso4217NamespaceUri = "http://www.xbrl.org/2003/iso4217";
        internal static string XmlNamespaceUri = "http://www.w3.org/XML/1998/namespace";

        // role URIs

        internal static string XbrlLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/label";
        internal static string XbrlTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/terseLabel";
        internal static string XbrlVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/verboseLabel";
        internal static string XbrlPositiveLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveLabel";
        internal static string XbrlPositiveTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveTerseLabel";
        internal static string XbrlPositiveVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/positiveVerboseLabel";
        internal static string XbrlNegativeLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeLabel";
        internal static string XbrlNegativeTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeTerseLabel";
        internal static string XbrlNegativeVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/negativeVerboseLabel";
        internal static string XbrlZeroLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroLabel";
        internal static string XbrlZeroTerseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroTerseLabel";
        internal static string XbrlZeroVerboseLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/zeroVerboseLabel";
        internal static string XbrlTotalLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/totalLabel";
        internal static string XbrlPeriodStartLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/periodStartLabel";
        internal static string XbrlPeriodEndLabelRoleNamespaceUri = "http://www.xbrl.org/2003/role/periodEndLabel";
        internal static string XbrlDocumentationRoleNamespaceUri = "http://www.xbrl.org/2003/role/documentation";
        internal static string XbrlDocumentationGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/definitionGuidance";
        internal static string XbrlDisclosureGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/disclosureGuidance";
        internal static string XbrlPresentationGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/presentationGuidance";
        internal static string XbrlMeasurementGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/measurementGuidance";
        internal static string XbrlCommentaryGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/commentaryGuidance";
        internal static string XbrlExampleGuidanceRoleNamespaceUri = "http://www.xbrl.org/2003/role/exampleGuidance";
        internal static string XbrlCalculationLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/calculationLinkbaseRef";
        internal static string XbrlDefinitionLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/definitionLinkbaseRef";
        internal static string XbrlLabelLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/labelLinkbaseRef";
        internal static string XbrlPresentationLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/presentationLinkbaseRef";
        internal static string XbrlReferenceLinkbaseReferenceRoleNamespaceUri = "http://www.xbrl.org/2003/role/referenceLinkbaseRef";

        /// <summary>
        /// The name of the XML document used to contain the XBRL data.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// The path to the XML document used to contain the XBRL data.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// A collection of <see cref="XbrlFragment"/> objects that contain the document's
        /// XBRL data.
        /// </summary>
        public List<XbrlFragment> XbrlFragments { get; private set; }

        /// <summary>
        /// Evaluates to true if the document contains no XBRL validation errors. Evaluates to
        /// false if the document contains at least one XBRL validation error.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (this.XbrlFragments == null)
                    return true;
                if (this.XbrlFragments.Count == 0)
                    return true;
                foreach (var currentFragment in this.XbrlFragments)
                {
                    if (currentFragment.IsValid == false)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// A collection of all validation errors found while validating the fragment.
        /// </summary>
        public List<ValidationError> ValidationErrors
        {
            get
            {
                if (this.XbrlFragments == null)
                    return null;
                if (this.XbrlFragments.Count == 0)
                    return null;
                if (this.XbrlFragments.Count == 1)
                    return this.XbrlFragments[0].ValidationErrors;
                var aggregatedValidationErrors = new List<ValidationError>();
                foreach (var currentFragment in this.XbrlFragments)
                {
                    aggregatedValidationErrors.AddRange(currentFragment.ValidationErrors);
                }
                return aggregatedValidationErrors;
            }
        }

        /// <summary>
        /// The constructor for the XbrlDocument class.
        /// </summary>
        public XbrlDocument()
        {
            this.XbrlFragments = new List<XbrlFragment>();
        }

        /// <summary>
        /// Loads a local filesystem or Internet-accessible XBRL document containing
        /// XBRL data.
        /// </summary>
        /// <remarks>
        /// This method supports documents located on the local file system, and also
        /// documents specified through a URL, as shown in the following example:
        /// <code>
        /// var localDoc = new XbrlDocument();
        /// localDoc.Load(@"..\..\..\JeffFerguson.Test.Gepsio\InferPrecisionTestDocuments\Example13Row7.xbrl");
        /// var internetDoc = new XbrlDocument();
        /// internetDoc.Load("http://www.xbrl.org/taxonomy/int/fr/ias/ci/pfs/2002-11-15/SampleCompany-2002-11-15.xml");
        /// </code>
        /// </remarks>
        /// <param name="Filename">
        /// The filename of the XML document to load.
        /// </param>
        public void Load(string Filename)
        {
            var SchemaValidXbrl = Container.Resolve<IDocument>();
            SchemaValidXbrl.Load(Filename);
            this.Filename = Filename;
            this.Path = System.IO.Path.GetDirectoryName(this.Filename);
            Parse(SchemaValidXbrl);
        }

        /// <summary>
        /// Loads an XBRL document containing XBRL data from a stream.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gepsio supports streams using the .NET Stream base class, which means that any type of stream
        /// supported by .NET and having the Stream class as a base class will be supported by Gepsio.
        /// This means that code like this will work:
        /// </para>
        /// <code>
        /// var webClient = new WebClient();
        /// string readXml = webClient.DownloadString("http://www.xbrl.org/taxonomy/int/fr/ias/ci/pfs/2002-11-15/SampleCompany-2002-11-15.xml");
        /// byte[] byteArray = Encoding.ASCII.GetBytes(readXml);
        /// MemoryStream memStream = new MemoryStream(byteArray);
        /// var newDoc = new XbrlDocument();
        /// newDoc.Load(memStream);
        /// </code>
        /// <para>
        /// Schema references found in streamed XBRL instances must specify an absolute location, and not
        /// a relative location. For example, this schema reference is fine:
        /// </para>
        /// <code>
        /// xsi:schemaLocation=http://www.xbrlsolutions.com/taxonomies/iso4217/2002-06-30/iso4217.xsd
        /// </code>
        /// <para>
        /// However, this one is not:
        /// </para>
        /// <code>
        /// &lt;xbrll:schemaRef xlink:href="msft-20141231.xsd" ... /&gt;
        /// </code>
        /// <para>
        /// The reason behind this restriction is that Gepsio must load schema references using an absolute
        /// location, and uses the location of the XBRL document instance as the reference path when
        /// resolving schema relative paths to an absolute location. A schema reference without a path, for
        /// example, says "find this schema in the same location as the XBRL document instance referencing
        /// the schema". When the XBRL document instance is located through a file path or URL, then the
        /// location is known, and the schema reference can be found. When the XBRL document instance is
        /// passed in as a stream, however, the instance has no location, per se. Since it has no location,
        /// there is no "location starting point" for resolving schema locations using relative paths.
        /// </para>
        /// <para>
        /// If you try to load an XBRL document instance through a stream, and that stream references a schema
        /// through a relative path, then the document will be marked as invalid when the Load() method
        /// returns. This code, for example, will load an invalid document instance, since the XBRL document
        /// instance references a schema through a relative path:
        /// </para>
        /// <code>
        /// var webClient = new WebClient();
        /// string readXml = webClient.DownloadString("http://www.sec.gov/Archives/edgar/data/789019/000119312515020351/msft-20141231.xml");
        /// byte[] byteArray = Encoding.ASCII.GetBytes(readXml);
        /// MemoryStream memStream = new MemoryStream(byteArray);
        /// var newDoc = new XbrlDocument();
        /// newDoc.Load(memStream);
        /// // newDoc.IsValid property will be FALSE here. Sad face.
        /// </code>
        /// <para>
        /// The document's ValidationErrors collection will contain a SchemaValidationError object, which will
        /// contain a message similar to the following:
        /// </para>
        /// <code>
        /// "The XBRL schema at msft-20141231.xsd could not be read because the file could not be found. Because
        /// the schema cannot be loaded, some validations will not be able to be performed. Other validation errors
        /// reported against this instance may stem from the fact that the schema cannot be loaded. More information
        /// on the "file not found" condition is available from the validation error object's inner exception
        /// property."
        /// </code>
        /// <para>
        /// XBRL document instances loaded through a stream which use absolute paths for schema references will be
        /// valid (assuming that all of the other XBRL semantics in the instance are correct).
        /// </para>
        /// </remarks>
        /// <param name="dataStream">
        /// A stream of data containing the XML document to load.
        /// </param>
        public void Load(Stream dataStream)
        {
            var SchemaValidXbrl = Container.Resolve<IDocument>();
            SchemaValidXbrl.Load(dataStream);
            this.Filename = string.Empty;
            this.Path = string.Empty;
            Parse(SchemaValidXbrl);
        }

        /// <summary>
        /// Parse the document, looking for fragments that can be processed.
        /// </summary>
        private void Parse(IDocument doc)
        {
            var NewNamespaceManager = Container.Resolve<INamespaceManager>();
            NewNamespaceManager.Document = doc;
            NewNamespaceManager.AddNamespace("instance", XbrlNamespaceUri);
            INodeList XbrlNodes = doc.SelectNodes("//instance:xbrl", NewNamespaceManager);
            foreach (INode XbrlNode in XbrlNodes)
                this.XbrlFragments.Add(new XbrlFragment(this, NewNamespaceManager, XbrlNode));
        }

        /// <summary>
        /// Finds the <see cref="RoleType"/> object having the given ID.
        /// </summary>
        /// <param name="RoleTypeId">
        /// The ID of the role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="RoleType"/> object having the given ID, or null if no
        /// object can be found.
        /// </returns>
        public RoleType GetRoleType(string RoleTypeId)
        {
            foreach (var currentFragment in XbrlFragments)
            {
                var roleTypeCandidate = currentFragment.GetRoleType(RoleTypeId);
                if (roleTypeCandidate != null)
                    return roleTypeCandidate;
            }
            return null;
        }

        /// <summary>
        /// Finds the <see cref="CalculationLink"/> object having the given role.
        /// </summary>
        /// <param name="CalculationLinkRole">
        /// The role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="CalculationLink"/> object having the given role, or
        /// null if no object can be found.
        /// </returns>
        public CalculationLink GetCalculationLink(RoleType CalculationLinkRole)
        {
            foreach (var currentFragment in XbrlFragments)
            {
                var calculationLinkCandidate = currentFragment.GetCalculationLink(CalculationLinkRole);
                if (calculationLinkCandidate != null)
                    return calculationLinkCandidate;
            }
            return null;
        }
    }
}
