using JeffFerguson.Gepsio.IoC;
using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A representation of all of the information in an XBRL schema file.
    /// </summary>
    public class XbrlSchema
    {
        private IDocument thisSchemaDocument;
        private ISchema thisXmlSchema;
        private ISchemaSet thisXmlSchemaSet;
        private ILookup<string, Element> thisLookupElements;
        private LinkbaseDocumentCollection thisLinkbaseDocuments;

        internal static string XmlSchemaInstanceNamespaceUri = "http://www.w3.org/2001/XMLSchema-instance";
        internal static string XmlSchemaNamespaceUri = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// The full path to the XBRL schema file, as specified in the schema reference
        /// element found in the containing XBRL fragment.
        /// </summary>
        public string SchemaReferencePath { get; private set; }

        /// <summary>
        /// The full path to the XBRL schema file actually loaded into memory.
        /// </summary>
        /// <remarks>
        /// In most cases, this value will match the value of <see cref="SchemaReferencePath"/>.
        /// If the schema reference path is found to be invalid, and a schema is loaded from an
        /// alternate location, then that location will beb specified by this property.
        /// </remarks>
        public string LoadPath { get; private set; }

        /// <summary>
        /// The root node of the parsed schema document.
        /// </summary>
        internal INode SchemaRootNode { get; private set; }

        /// <summary>
        /// The target namespace of the schema.
        /// </summary>
        public string TargetNamespace { get; private set; }

        /// <summary>
        /// An alias URI for the target namespace of the schema.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Some industry-standard schemas specify target namespaces that differ from
        /// their published target namespace URIs. For example, the Web page at
        /// https://xbrl.us/xbrl-taxonomy/2009-us-gaap/ describes the schemas for
        /// US GAAP Taxonomies, Release 2009. According to the Web site, the target
        /// namespace for the schemas is http://taxonomies.xbrl.us/us-gaap/2009-01-31.
        /// However, the XSD that implements the schema, which is available at
        /// http://taxonomies.xbrl.us/us-gaap/2009/elts/us-gaap-std-2009-01-31.xsd,
        /// specifies a target namespace of http://xbrl.us/us-gaap-std/2009-01-31. This
        /// differs from the target namespace listed in the spec, which does not list
        /// the "-std" suffix in the URI.
        /// </para>
        /// <para>
        /// To combat this discrepany, Gepsio offers a "target namespace alias" which
        /// may differ from the target namespace specified in the schema itself. This
        /// alias allows a place for Gepsio to specify an alias target namespace which
        /// matches the URI used in specification documents.
        /// </para>
        /// <para>
        /// When a schema is loaded, the alias will be set to the target namespace
        /// specified in the source schema. However, Gepsio can change this value to
        /// something else once the schema is loaded.
        /// </para>
        /// </remarks>
        public string TargetNamespaceAlias { get; internal set; }

        /// <summary>
        /// A collection of <see cref="Element"/> objects representing all elements defined in the schema.
        /// </summary>
        public List<Element> Elements { get; private set; }

        /// <summary>
        /// A reference to the schema's calculation linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public CalculationLinkbaseDocument CalculationLinkbase
        {
            get
            {
                return thisLinkbaseDocuments.CalculationLinkbase;
            }
        }

        /// <summary>
        /// A reference to the schema's definition linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public DefinitionLinkbaseDocument DefinitionLinkbase
        {
            get
            {
                return thisLinkbaseDocuments.DefinitionLinkbase;
            }
        }

        /// <summary>
        /// A reference to the schema's label linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public LabelLinkbaseDocument LabelLinkbase
        {
            get
            {
                return thisLinkbaseDocuments.LabelLinkbase;
            }
        }

        /// <summary>
        /// A reference to the schema's presentation linkbase. Null is returned if no such linkbase is available.
        /// </summary>
        public PresentationLinkbaseDocument PresentationLinkbase
        {
            get
            {
                return thisLinkbaseDocuments.PresentationLinkbase;
            }
        }

        /// <summary>
        /// The namespace manager associated with the parsed schema document.
        /// </summary>
        internal INamespaceManager NamespaceManager { get; private set; }

        /// <summary>
        /// A collection of <see cref="RoleType"/> objects representing all role types defined in the schema.
        /// </summary>
        public List<RoleType> RoleTypes { get; private set; }

        /// <summary>
        /// The <see cref="XbrlFragment"/> which references the schema.
        /// </summary>
        public XbrlFragment Fragment { get; private set; }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal XbrlSchema(XbrlFragment ContainingXbrlFragment, string SchemaFilename, string BaseDirectory)
        {
            this.Fragment = ContainingXbrlFragment;
            this.SchemaReferencePath = GetFullSchemaPath(SchemaFilename, BaseDirectory);
            this.LoadPath = this.SchemaReferencePath;
            try
            {
                if (ReadAndCompile(this.SchemaReferencePath) == false)
                    return;
            }
            catch (FileNotFoundException fnfEx)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("FileNotFoundDuringSchemaCreation");
                MessageBuilder.AppendFormat(StringFormat, this.SchemaReferencePath);
                this.Fragment.AddValidationError(new SchemaValidationError(this, MessageBuilder.ToString(), fnfEx));
                return;
            }
            catch (WebException webEx)
            {

                // Check to see if we got an HTTP 404 back from an attempt to open a schema up from a
                // URL. If we did, check to see if the schema is available locally. Some taxonomies are
                // specified through URLs that don't actually exist in a physical form but just appear
                // as placeholders. The Dutch taxonomies are an example of this. An XBRL instance might
                // have a schema reference of "http://archprod.service.eogs.dk/taxonomy/20171001/
                // entryDanishGAAPBalanceSheetAccountFormIncomeStatementByNatureIncludingManagements
                // ReviewStatisticsAndTax20171001.xsd", for example, but there might not be a schema
                // at that location. It is assumed, in these cases, that the schema is available with
                // the XBRL instance itself. Check for a locally-stored schema.

                var localSchemaAvailable = false;
                var schemaLocalPath = string.Empty;
                var webResponse = webEx.Response as HttpWebResponse;
                if(webResponse == null || webResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    schemaLocalPath = BuildSchemaPathLocalToFragment(ContainingXbrlFragment, SchemaFilename);
                    try
                    {
                        localSchemaAvailable = ReadAndCompile(schemaLocalPath);
                    }
                    catch(FileNotFoundException)
                    {
                        localSchemaAvailable = false;
                    }
                }
                if (localSchemaAvailable == false)
                {
                    StringBuilder MessageBuilder = new StringBuilder();
                    string StringFormat = AssemblyResources.GetName("WebExceptionThrownDuringSchemaCreation");
                    MessageBuilder.AppendFormat(StringFormat, this.SchemaReferencePath);
                    this.Fragment.AddValidationError(new SchemaValidationError(this, MessageBuilder.ToString(), webEx));
                    return;
                }
                this.LoadPath = schemaLocalPath;
            }
            thisSchemaDocument = Container.Resolve<IDocument>();
            this.thisLinkbaseDocuments = new LinkbaseDocumentCollection();
            this.RoleTypes = new List<RoleType>();
            thisSchemaDocument.Load(this.LoadPath);
            this.NamespaceManager = Container.Resolve<INamespaceManager>();
            this.NamespaceManager.Document = thisSchemaDocument;
            this.NamespaceManager.AddNamespace("schema", XbrlSchema.XmlSchemaNamespaceUri);
            ReadSchemaNode();
            ReadSimpleTypes();
            ReadComplexTypes();
            ReadElements();
            LookForAnnotations();
        }

        /// <summary>
        /// Reads a schema and compiles it into a schema set.
        /// </summary>
        /// <remarks>
        /// This code may throw exceptions. Exception handling is the responsibility of the caller.
        /// </remarks>
        /// <param name="schemaPath">
        /// The path of the schema to read.
        /// </param>
        /// <returns>
        /// True if the load was successful, false if the file is not a schema file.
        /// </returns>
        private bool ReadAndCompile(string schemaPath)
        {
            thisXmlSchema = Container.Resolve<ISchema>();
            thisXmlSchemaSet = Container.Resolve<ISchemaSet>();
            if (thisXmlSchema.Read(schemaPath) == false)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, schemaPath);
                this.Fragment.AddValidationError(new SchemaValidationError(this, MessageBuilder.ToString()));
                return false;
            }
            thisXmlSchemaSet.Add(thisXmlSchema);
            thisXmlSchemaSet.Compile();
            return true;
        }

        /// <summary>
        /// Builds a schema path local to a given fragment, ignoring any existing path information
        /// in the supplied schema path.
        /// </summary>
        /// <param name="ContainingXbrlFragment"></param>
        /// <param name="SchemaFilename"></param>
        /// <returns></returns>
        private string BuildSchemaPathLocalToFragment(XbrlFragment ContainingXbrlFragment, string SchemaFilename)
        {
            var schemaUri = new Uri(SchemaFilename);
            var schemaUriSegments = schemaUri.Segments.Length;
            var schemaUriFilename = schemaUri.Segments[schemaUriSegments - 1];
            var localPath = ContainingXbrlFragment.Document.Path;
            var schemaLocalPath = System.IO.Path.Combine(localPath, schemaUriFilename);
            return schemaLocalPath;
        }

        /// <summary>
        /// Gets the named element defined by the schema.
        /// </summary>
        /// <param name="ElementName">
        /// The name of the element to be returned.
        /// </param>
        /// <returns>
        /// A reference to the <see cref="Element"/> object representing the element with the given name.
        /// A null is returned if no <see cref="Element"/> object is available with the given name.
        /// </returns>
        public Element GetElement(string ElementName)
        {
            if (this.Elements == null)
                return null;
            if (thisLookupElements == null)
                thisLookupElements = Elements.ToLookup(a => a.Name);
            return thisLookupElements[ElementName].FirstOrDefault();
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
            foreach (var currentRoleType in RoleTypes)
            {
                if (currentRoleType.Id.Equals(RoleTypeId) == true)
                    return currentRoleType; 
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
            if (this.CalculationLinkbase == null)
                return null;
            var calculationLinkCandidate = CalculationLinkbase.GetCalculationLink(CalculationLinkRole);
            if (calculationLinkCandidate != null)
                return calculationLinkCandidate;
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private string GetFullSchemaPath(string SchemaFilename, string BaseDirectory)
        {

            // The first check is to see whether or not the "filename" is actually an HTTP-based
            // reference. If it is, then it will be returned without modification.

            var lowerCaseSchemaFilename = SchemaFilename.ToLower();
            if (lowerCaseSchemaFilename.StartsWith("http://") == true)
                return SchemaFilename;
            if (lowerCaseSchemaFilename.StartsWith("https://") == true)
                return SchemaFilename;

            // At this point, we're confident that we have an actual filename.

            string FullPath;
            int FirstPathSeparator = SchemaFilename.IndexOf(System.IO.Path.DirectorySeparatorChar);
            if (FirstPathSeparator == -1)
            {
                string DocumentUri = this.Fragment.XbrlRootNode.BaseURI;
                if(string.IsNullOrEmpty(DocumentUri) == true)
                {
                    DocumentUri = this.Fragment.Document.Filename;
                }
                int LastPathSeparator = DocumentUri.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                if (LastPathSeparator == -1)
                    LastPathSeparator = DocumentUri.LastIndexOf('/');
                string DocumentPath = DocumentUri.Substring(0, LastPathSeparator + 1);
                if (BaseDirectory.Length > 0)
                    DocumentPath = DocumentPath + BaseDirectory;
                FullPath = DocumentPath + SchemaFilename;
            }
            else
            {
                throw new NotImplementedException("XbrlSchema.GetFullSchemaPath() code path not implemented.");
            }
            return FullPath;
        }

        /// <summary>
        /// Reads the schema's root node and collects namespace data from the namespace attributes.
        /// </summary>
        private void ReadSchemaNode()
        {
            this.Elements = new List<Element>();
            this.SchemaRootNode = thisSchemaDocument.SelectSingleNode("//schema:schema", this.NamespaceManager);
            if (this.SchemaRootNode == null)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                string StringFormat = AssemblyResources.GetName("SchemaFileCandidateDoesNotContainSchemaRootNode");
                MessageBuilder.AppendFormat(StringFormat, this.SchemaReferencePath);
                this.Fragment.AddValidationError(new SchemaValidationError(this, MessageBuilder.ToString()));
                return;
            }
            this.TargetNamespace = this.SchemaRootNode.Attributes["targetNamespace"].Value;
            this.TargetNamespaceAlias = this.TargetNamespace;
            foreach (IAttribute CurrentAttribute in this.SchemaRootNode.Attributes)
                if (CurrentAttribute.Prefix == "xmlns")
                    this.NamespaceManager.AddNamespace(CurrentAttribute.LocalName, CurrentAttribute.Value);
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadSimpleTypes()
        {
            //this.SimpleTypes = new List<SimpleType>();
            //INodeList SimpleTypeNodes = thisSchemaDocument.SelectNodes("//schema:simpleType", this.NamespaceManager);
            //foreach (INode SimpleTypeNode in SimpleTypeNodes)
            //    this.SimpleTypes.Add(new SimpleType(SimpleTypeNode, this.NamespaceManager));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadComplexTypes()
        {
            //this.ComplexTypes = new List<ComplexType>();
            //INodeList ComplexTypeNodes = thisSchemaDocument.SelectNodes("//schema:complexType", this.NamespaceManager);
            //foreach (INode ComplexTypeNode in ComplexTypeNodes)
            //    this.ComplexTypes.Add(new ComplexType(ComplexTypeNode, this.NamespaceManager));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadElements()
        {
            foreach (var CurrentEntry in thisXmlSchemaSet.GlobalElements)
            {
                ISchemaElement CurrentElement = CurrentEntry.Value as ISchemaElement;
                this.Elements.Add(new Element(this, CurrentElement));
                thisLookupElements = null;
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal ISchemaType GetXmlSchemaType(IQualifiedName ItemTypeValue)
        {
            foreach (var CurrentEntry in thisXmlSchemaSet.GlobalTypes)
            {
                ISchemaType CurrentType = CurrentEntry.Value as ISchemaType;
                if (CurrentType.QualifiedName.FullyQualifiedName.Equals(ItemTypeValue.FullyQualifiedName) == true)
                {
                    return CurrentType;
                }
            }
            return null;
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void LookForAnnotations()
        {
            foreach (INode CurrentChild in this.SchemaRootNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("annotation") == true)
                    ReadAnnotations(CurrentChild);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadAnnotations(INode AnnotationNode)
        {
            foreach (INode CurrentChild in AnnotationNode.ChildNodes)
            {
                if (CurrentChild.LocalName.Equals("appinfo") == true)
                    ReadAppInfo(CurrentChild);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadAppInfo(INode AppInfoNode)
        {
            thisLinkbaseDocuments.ReadLinkbaseReferences(this.SchemaRootNode.BaseURI, AppInfoNode);
            foreach (INode CurrentChild in AppInfoNode.ChildNodes)
            {
                if ((CurrentChild.NamespaceURI.Equals(XbrlDocument.XbrlLinkbaseNamespaceUri) == true) && (CurrentChild.LocalName.Equals("roleType") == true))
                    ReadRoleType(CurrentChild);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ReadRoleType(INode RoleTypeNode)
        {
            this.RoleTypes.Add(new RoleType(this, RoleTypeNode));
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        internal Element LocateElement(Locator ElementLocator)
        {
            foreach (Element CurrentElement in this.Elements)
            {
                if (string.IsNullOrEmpty(CurrentElement.Id) == false)
                {
                    if (CurrentElement.Id.Equals(ElementLocator.HrefResourceId) == true)
                        return CurrentElement;
                }
            }
            return null;
        }

        /// <summary>
        /// Given a URI, return the namespace prefix associated with the URI.
        /// </summary>
        /// <param name="uri">A namespace URI.</param>
        /// <returns>A string representing the namespace prefix. An empty string is returned if the URI is not defined in the schema.</returns>
        internal string GetPrefixForUri(string uri)
        {
            var NamespacesArray = thisXmlSchema.Namespaces.ToArray();
            foreach (var CurrentName in NamespacesArray)
            {
                if (CurrentName.Namespace.Equals(uri) == true)
                    return CurrentName.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// Given a namespace prefix, return the URI associated with the namespace.
        /// </summary>
        /// <param name="prefix">A namespace prefix.</param>
        /// <returns>The URI associated with the namespace.</returns>
        internal string GetUriForPrefix(string prefix)
        {
            var NamespacesArray = thisXmlSchema.Namespaces.ToArray();
            foreach (var CurrentName in NamespacesArray)
            {
                if (CurrentName.Name.Equals(prefix) == true)
                    return CurrentName.Namespace;
            }
            return string.Empty;
        }

        internal AnyType GetNodeType(INode node)
        {
            var matchingElement = GetElement(node.LocalName);
            if(matchingElement == null)
                return null;
            return AnyType.CreateType(matchingElement.TypeName.Name, this);
        }

        internal AnyType GetAttributeType(IAttribute attribute)
        {
            foreach(var currentGlobalAttribute in thisXmlSchemaSet.GlobalAttributes)
            {
                var currentGlobalAttributeQualifiedName = currentGlobalAttribute.Key;
                if((attribute.LocalName.Equals(currentGlobalAttributeQualifiedName.Name) == true) && (attribute.NamespaceURI.Equals(currentGlobalAttributeQualifiedName.Namespace) == true))
                {
                    return AnyType.CreateType(currentGlobalAttribute.Value.TypeName.Name, this);
                }
            }
            return null;
        }
    }
}
