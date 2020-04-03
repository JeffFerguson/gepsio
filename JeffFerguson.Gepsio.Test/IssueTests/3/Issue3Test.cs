using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace JeffFerguson.Gepsio.Test.IssueTests._3
{
    /// <summary>
    /// Issue 3 records a bug where XBRL using a Dutch taxonomy loaded into
    /// Gepsio but returned no facts.
    /// </summary>
    [TestClass]
	[TestCategory("Fix for issue")]
    public class Issue3Test
    {
        [TestMethod]
        public void VerifyFixForIssue3_EnsureLocalSchemaLoad()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\IssueTests\3\offentliggorelse.xml");
            Assert.IsTrue(xbrlDoc.XbrlFragments[0].Schemas.Count > 0);
        }

        [TestMethod]
        public void VerifyFixForIssue3_EnsureAlternativeSchemaLoadPath()
        {
            var xbrlDoc = new XbrlDocument();
            var unitTestDir = Directory.GetCurrentDirectory();
            xbrlDoc.Load(@"..\..\..\IssueTests\3\offentliggorelse.xml");
            var loadedSchema = xbrlDoc.XbrlFragments[0].Schemas[0];
            Assert.AreNotEqual(loadedSchema.SchemaReferencePath, loadedSchema.LoadPath);
        }
    }
}
