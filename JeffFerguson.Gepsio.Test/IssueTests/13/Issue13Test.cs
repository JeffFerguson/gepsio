using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Gepsio.Test.IssueTests._13
{
    [TestClass]
	[TestCategory("Fix for issue")]
    public class Issue13Test
    {
        /// <summary>
        /// This test loads an XBRL instance that eventually loads a reference linkbase
        /// document found at http://sbr.gov.au/taxonomy/sbr_au_taxonomy/icls/py/pyin/pyin.02.00.refLink.xml.
        /// Ensure that Gepsio can correctly process this linkbase document.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue13()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\IssueTests\13\Issue13.xml");
        }
    }
}
