using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace JeffFerguson.Gepsio.Test.IssueTests._3
{
    /// <summary>
    /// Issue 3 records a bug where XBRL using a Dutch taxonomy loaded into
    /// Gepsio but returned no facts.
    /// </summary>
    [TestClass]
    public class Issue3Test
    {
        [TestMethod]
        public void VerifyFixForIssue3()
        {
            var xbrlDoc = new XbrlDocument();
            var d = Directory.GetCurrentDirectory();
            xbrlDoc.Load(@"..\..\..\IssueTests\3\offentliggorelse.xml");
            Assert.IsTrue(xbrlDoc.XbrlFragments[0].Facts.Count > 0);
        }
    }
}
