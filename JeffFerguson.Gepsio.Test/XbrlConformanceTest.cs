using JeffFerguson.Gepsio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

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
    public class XbrlConformanceTest
    {
        private int thisTestsPassed;

        public XbrlConformanceTest()
        {
        }

        [TestMethod]
        [Description("XBRL-CONF-2014-12-10")]
        public void ExecuteXBRLCONF20141210Testcases()
        {
            thisTestsPassed = 0;
            Debug.AutoFlush = true;
            var conformanceXmlSource = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}XBRL-CONF-2014-12-10{Path.DirectorySeparatorChar}xbrl.xml";
            var conformanceXmlSourcePath = Path.GetDirectoryName(conformanceXmlSource);
            var conformanceXmlDocument = new XmlDocument();
            conformanceXmlDocument.Load(conformanceXmlSource);
            var TestcaseNodes = conformanceXmlDocument.SelectNodes("//testcase");
            foreach (XmlNode TestcaseNode in TestcaseNodes)
            {
                ExecuteTestcase(conformanceXmlSourcePath, TestcaseNode);
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
        private void ExecuteTestcase(string ConformanceXmlSourcePath, XmlNode TestcaseNode)
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
            var variationNodes = testcaseXmlDocument.SelectNodes("//variation");
            foreach (XmlNode VariationNode in variationNodes)
            {
                ExecuteVariation(testcaseXmlSourceDirectory, VariationNode);
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
            thisTestsPassed++;
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
            Assert.Fail(failMessage.ToString());
        }

        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        private void AnnounceTestFailure(XbrlConformanceTestVariation CurrentVariation)
        {
            AnnounceTestFailure(CurrentVariation, null);
        }
    }
}
