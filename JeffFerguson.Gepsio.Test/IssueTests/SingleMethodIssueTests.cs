using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Gepsio.Test.IssueTests
{
    /// <summary>
    /// This class contains tests methods that test issues using a single method with no
    /// private helper methods are external files required.
    /// </summary>
    /// <remarks>
    /// Some of these tests are implemented in both sync and async versions just so that there
    /// is some test coverage on LoadAsync() as well as Load().
    /// </remarks>
    [TestClass]
    public class SingleMethodIssueTests
    {
        [TestMethod]
        public void VerifyFixForIssue1()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml");
            Assert.IsTrue(xbrlDoc.IsValid);
        }

        [TestMethod]
        public async Task VerifyFixForIssue1Async()
        {
            var xbrlDoc = new XbrlDocument();
            await xbrlDoc.LoadAsync("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml");
            Assert.IsTrue(xbrlDoc.IsValid);
        }

        /// <summary>
        /// The bug for this issue was throwing an exception. This test is not concerned
        /// with document validity but simply concerned with making sure that no exceptions
        /// are thrown during loading.
        /// </summary>
        /// <remarks>
        /// This is the synchronous version of the test.
        /// </remarks>
        [TestMethod]
        public void VerifyFixForIssue8()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
        }

        /// <summary>
        /// The bug for this issue was throwing an exception. This test is not concerned
        /// with document validity but simply concerned with making sure that no exceptions
        /// are thrown during loading.
        /// </summary>
        /// <remarks>
        /// This is the asynchronous version of the test.
        /// </remarks>
        [TestMethod]
        public async Task VerifyFixForIssue8Async()
        {
            var xbrlDoc = new XbrlDocument();
            await xbrlDoc.LoadAsync("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
        }

        /// <summary>
        /// Ensure that the namespace http://xbrl.sec.gov/dei/2014-01-31, which was added
        /// to Gepsio's support of industry standard namespaces, does not appear anywhere
        /// in a validation error message.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue9()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
            foreach (var validationError in xbrlDoc.ValidationErrors)
            {
                if (validationError.Message.Contains("http://xbrl.sec.gov/dei/2014-01-31") == true)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Ensure that the namespace http://fasb.org/us-gaap/2017-01-31, which was added
        /// to Gepsio's support of industry standard namespaces, does not appear anywhere
        /// in a validation error message.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue10()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
            foreach (var validationError in xbrlDoc.ValidationErrors)
            {
                if (validationError.Message.Contains("http://fasb.org/us-gaap/2017-01-31") == true)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Ensure that the namespace http://xbrl.sec.gov/invest/2013-01-31, which was added
        /// to Gepsio's support of industry standard namespaces, does not appear anywhere
        /// in a validation error message.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue11()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
            foreach (var validationError in xbrlDoc.ValidationErrors)
            {
                if (validationError.Message.Contains("http://xbrl.sec.gov/invest/2013-01-31") == true)
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// Ensure that decimal number parsing is culture independant.
        /// For example french uses , (comma) as decimal separator.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue16()
        {
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr");
                var xbrlDoc = new XbrlDocument();
                xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
            }
            catch (FormatException)
            {
                Assert.Fail("Decimal number format should be culture independant.");
            }
        }

        /// <summary>
        /// No Support for xsd:import in Schema Handling.
        /// As a consequence in ESRD taxonomy, label linkbases are not discovered.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue50()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\IssueTests\50\efrag-2026-12-31-en.xbrl");
            var xbrlSchema = xbrlDoc.XbrlFragments[0].Schemas[0];

            Assert.IsTrue(xbrlSchema.DefinitionLinkbases.Any());    //definition linkbases are in main schema
            Assert.IsTrue(xbrlSchema.LabelLinkbases.Any());     //label linkbases are in imported schema
        }

        /// <summary>
        /// Error parsing PresentationArc Order value when culture is french.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue52()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr");
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\IssueTests\52\efrag-2026-12-31-en.xbrl");
            var nodes = xbrlDoc.XbrlFragments[0].GetPresentableFactTree().TopLevelNodes;

            Assert.AreEqual(nodes[0].ChildNodes[3].PresentationLinkbaseLocator.HrefResourceId, "esrs_UndertakingIsNotRequiredToDrawupFinancialStatements");
            Assert.AreEqual(nodes[0].ChildNodes[4].PresentationLinkbaseLocator.HrefResourceId, "esrs_DisclosureOfExtentToWhichSustainabilityStatementCoversUpstreamAndDownstreamValueChainExplanatory");
        }

        /// <summary>
        /// Ensure that the taxonomy at http://xbrl.fasb.org/us-gaap/2018/elts/us-gaap-2018-01-31.xsd
        /// can be loaded without exceptions.
        /// </summary>
        /// <remarks>
        /// This is the synchronous version of the test.
        /// </remarks>
        [TestMethod]
        public void VerifyFixForIssue22()
        {
            var xbrlDoc = new XbrlDocument();
            try
            {
                xbrlDoc.Load(@"https://www.sec.gov/Archives/edgar/data/1018724/000101872419000004/amzn-20181231.xml");
            }
            catch (System.Exception)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Ensure that the taxonomy at http://xbrl.fasb.org/us-gaap/2018/elts/us-gaap-2018-01-31.xsd
        /// can be loaded without exceptions.
        /// </summary>
        /// <remarks>
        /// This is the asynchronous version of the test.
        /// </remarks>
        [TestMethod]
        public async Task VerifyFixForIssue22Async()
        {
            var xbrlDoc = new XbrlDocument();
            try
            {
                await xbrlDoc.LoadAsync(@"https://www.sec.gov/Archives/edgar/data/1018724/000101872419000004/amzn-20181231.xml");
            }
            catch (System.Exception)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Verify that GetHashCode() can be called on schema elements that lack defined ID attributes.
        /// </summary>
        /// <remarks>
        /// The hash code returned is not important to the test. The test is simply in place so that the
        /// GetHashCode() method can be called without a NullReferenceException being thrown.
        /// </remarks>
        [TestMethod]
        public void VerifyFixForIssue57()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml");
            var firstFragment = xbrlDoc.XbrlFragments[0];
            var elementWithoutDefinedId = firstFragment.Schemas.GetElement("explicitMember");
            var hashCode = elementWithoutDefinedId.GetHashCode();
        }
    }
}
