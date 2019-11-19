using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace JeffFerguson.Test.Gepsio
{
    /// <summary>
    /// This class executes the XBRL-CONF-2014-12-10 conformance test suite.
    /// </summary>
    /// <remarks>
    /// The test suite is driven by a document in the root of the conformance suite
    /// folder named "xbrl.xml". The root document is an XML document that contains
    /// a set of &lt;testcase&gt; elements, each of which references a separate
    /// test case document.
    /// </remarks>
    [TestClass]
	[TestCategory("Xbrl conformance")]
    public class XbrlConformanceTest
    {
		private const string CONFORMANCE_XML_SOURCE = @"..\..\..\XBRL-CONF-2014-12-10\xbrl.xml";
		private int _failedTest = 0;
		private readonly XmlDocument thisConformanceXmlDocument;

		public XbrlConformanceTest() {
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			thisConformanceXmlDocument = new XmlDocument();
			this.thisConformanceXmlDocument.Load(CONFORMANCE_XML_SOURCE);
		}

		#region Tests

		[DataTestMethod]
		[DataRow("100")]
		[DataRow("200")]
		[DataRow("300")]
		[DataRow("400")]
		[DataRow("related")]
		[Description("XBRL-CONF-2014-12-10 all tests")]
		public void execute_all_test_cases(string group)
		{
			this.ExecuteSelectedTests( $"//group[@id='{@group}']/testcase" );
			Assert.IsTrue( this._failedTest == 0 );
		}
		[DataTestMethod]
		[DataRow("100")]
		[DataRow("200")]
		[DataRow("300")]
		[DataRow("400")]
		[DataRow("related")]
		[Description("XBRL-CONF-2014-12-10 passing tests")]
		public void execute_passing_test_cases(string group)
		{
			this.ExecuteSelectedTests( $"//passing/group[@id='{@group}']/testcase" );
			Assert.IsTrue( this._failedTest == 0 );
		}
		[TestMethod]
		[TestCategory("Failing")]
		public void execute_failing_tests_for_schema_validation()
		{
			this.ExecuteFailingTestcasesForGroup( "100" );
		}
		[TestMethod]
		[TestCategory("Failing")]
		public void execute_failing_tests_for_linkbase_validation()
		{
			this.ExecuteFailingTestcasesForGroup( "200" );
		}
		[TestMethod]
		[TestCategory("Failing")]
		public void execute_failing_tests_for_instance_validation()
		{
			this.ExecuteFailingTestcasesForGroup( "300" );
		}
		[TestMethod]
		[TestCategory("Failing")]
		public void execute_failing_tests_for_misc_validation()
		{
			this.ExecuteFailingTestcasesForGroup( "400" );
		}
		[DataTestMethod]
		[DataRow("104")]
		[Description("XBRL-CONF-2014-12-10 specified test case")]
		[TestCategory("Debug")]
		public void execute_specified_test_case(string id)
		{
			this.ExecuteSelectedTests( $"//testcase[@id='{id}']" );
			Assert.IsTrue( this._failedTest == 0 );
		}
		[DataTestMethod]
		[DataRow("307", "V-3")]
		[DataRow("307", "V-2")]
		[DataRow("301", "V-4")]
		[Description("XBRL-CONF-2014-12-10 specified test case variation")]
		[TestCategory("Debug")]
		public void execute_specified_test_case_variation(string caseId, string variationId)
		{
			this.ExecuteSelectedTests( $"//testcase[@id='{caseId}']", $"//variation[@id='{variationId}']" );
			Assert.IsTrue( this._failedTest == 0 );
		}

		#endregion

		private void ExecuteFailingTestcasesForGroup(string group)
		{
			this.ExecuteSelectedTests( $"//failing/group[@id='{@group}']/testcase" );
			Assert.IsTrue( this._failedTest == 0 );
		}

		private void ExecuteSelectedTests(string xpath, string VariationsXPath = "//variation") {
			var TestcaseNodes = this.thisConformanceXmlDocument.SelectNodes( xpath );
            Debug.Assert( TestcaseNodes.Count > 0 );
			foreach( XmlNode TestcaseNode in TestcaseNodes ) {
				this.ExecuteTestcase( Path.GetDirectoryName( CONFORMANCE_XML_SOURCE ), TestcaseNode, VariationsXPath );
			}
		}

		/// <summary>
		/// Executes the test case referenced in the supplied test case node.
		/// </summary>
		/// <param name="ConformanceXmlSourcePath">
		/// The path to the conformance suite.
		/// </param>
		/// <param name="TestcaseNode">
		/// A reference to one of the &lt;testcase&gt; elements in the root test suiteb ocument.
		/// </param>
		/// <param name="VariationsXPath"></param>
		private void ExecuteTestcase(string ConformanceXmlSourcePath, XmlNode TestcaseNode, string VariationsXPath)
        {
            var uriAttribute = TestcaseNode.Attributes["uri"];
            var testcaseXmlSource = uriAttribute.Value;
            var testcaseXmlSourceFullPathBuilder = new StringBuilder();
            testcaseXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", ConformanceXmlSourcePath, Path.DirectorySeparatorChar, testcaseXmlSource);
            var testcaseXmlSourceFullPath = testcaseXmlSourceFullPathBuilder.ToString();
            var testcaseXmlSourceDirectory = Path.GetDirectoryName(testcaseXmlSourceFullPath);
            var testcaseXmlDocument = new XmlDocument();
            testcaseXmlDocument.Load(testcaseXmlSourceFullPath);
            var testcaseNode = testcaseXmlDocument.SelectSingleNode("//testcase");
            var testcaseName = testcaseNode.Attributes["name"].Value;
            var testcaseDescription = testcaseNode.Attributes["description"].Value;
            Debug.WriteLine("+-----");
            Debug.WriteLine($"| {testcaseName} [{testcaseDescription}]");
            Debug.WriteLine("+-----");
            var variationNodes = testcaseXmlDocument.SelectNodes(VariationsXPath);
            foreach (XmlNode VariationNode in variationNodes)
            {
                this.ExecuteVariation(testcaseXmlSourceDirectory, VariationNode);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void ExecuteVariation(string TestcaseXmlSourceDirectory, XmlNode VariationNode)
        {
            var currentVariation = new XbrlConformanceTestVariation(VariationNode);
            if (string.IsNullOrEmpty(currentVariation.Instance) == true)
                return;

            Debug.Write(currentVariation.Instance);
            Debug.Write(" [");
            Debug.Write(currentVariation.Description);
            Debug.WriteLine("]");
            var instanceXmlSourceFullPathBuilder = new StringBuilder();
            instanceXmlSourceFullPathBuilder.AppendFormat("{0}{1}{2}", TestcaseXmlSourceDirectory, Path.DirectorySeparatorChar, currentVariation.Instance);
            var instanceXmlSourceFullPath = instanceXmlSourceFullPathBuilder.ToString();

            var newXbrlDocument = new XbrlDocument();
            newXbrlDocument.Load(instanceXmlSourceFullPath);
            if (newXbrlDocument.IsValid == true)
            {
                if (currentVariation.ValidityExpected == false)
                    AnnounceTestFailure(currentVariation);
            }
            if (newXbrlDocument.IsValid == false)
            {
                if (currentVariation.ValidityExpected == true)
                    AnnounceTestFailure(currentVariation, newXbrlDocument);
            }
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(XbrlConformanceTestVariation CurrentVariation, XbrlDocument xbrlDoc)
        {
            var failMessage = new StringBuilder();

            failMessage.AppendFormat("{0}Instance: {1}{2}", Environment.NewLine, CurrentVariation.Instance, Environment.NewLine);
            failMessage.AppendFormat("Name: {0}{1}", CurrentVariation.Name, Environment.NewLine);
            failMessage.AppendFormat("Description: {0}", CurrentVariation.Description);
            if (xbrlDoc != null)
            {
                foreach (var currentValidationError in xbrlDoc.ValidationErrors)
                {
                    failMessage.AppendFormat("{0}{1}Validation Error Type: {2}{3}", Environment.NewLine, Environment.NewLine, currentValidationError.GetType().ToString(), Environment.NewLine);
                    failMessage.AppendFormat("Validation Error Description: {0}{1}", currentValidationError.Message, Environment.NewLine);
                }
            }
            Debug.WriteLine("***");
            Debug.Write("*** FAIL *** [");
            if (CurrentVariation.ValidityExpected == true)
                Debug.WriteLine("Gepsio failed a test expected to pass.]");
            else
                Debug.WriteLine("Gepsio passed a test expected to fail.]");
            Debug.WriteLine("***");
            Debug.WriteLine(failMessage.ToString());
			Logger.LogMessage( failMessage.ToString() );
            //Assert.Fail(failMessage.ToString());
			this._failedTest++;
		}

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(XbrlConformanceTestVariation CurrentVariation)
        {
            AnnounceTestFailure(CurrentVariation, null);
        }
    }
}
