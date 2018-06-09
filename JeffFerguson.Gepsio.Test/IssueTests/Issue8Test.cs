using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Gepsio.Test.IssueTests
{
    [TestClass]
    public class Issue8Test
    {
        /// <summary>
        /// The bug for this issue was throwing an exception. This test is not concerned
        /// with document validity but simply concerned with making sure that no exceptions
        /// are thrown during loading.
        /// </summary>
        [TestMethod]
        public void VerifyFixForIssue8()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
        }
    }
}
