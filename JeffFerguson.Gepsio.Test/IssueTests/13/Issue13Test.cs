using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JeffFerguson.Gepsio.Test.IssueTests._13
{
    [TestClass]
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
            xbrlDoc.Load($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}IssueTests{Path.DirectorySeparatorChar}13{Path.DirectorySeparatorChar}Issue13.xml");
        }
    }
}
