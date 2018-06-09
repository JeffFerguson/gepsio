using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Gepsio.Test.IssueTests
{
    [TestClass]
    public class Issue1Test
    {
        [TestMethod]
        public void VerifyFixForIssue1()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml");
            Assert.IsTrue(xbrlDoc.IsValid);
        }
    }
}
