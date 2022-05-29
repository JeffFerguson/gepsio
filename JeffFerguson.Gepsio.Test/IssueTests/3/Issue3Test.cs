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
        public void VerifyFixForIssue3_EnsureLocalSchemaLoad()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}IssueTests{Path.DirectorySeparatorChar}3{Path.DirectorySeparatorChar}offentliggorelse.xml");
            Assert.IsTrue(xbrlDoc.XbrlFragments[0].Schemas.Count > 0);
        }

        [TestMethod]
        public void VerifyFixForIssue3_EnsureAlternativeSchemaLoadPath()
        {
            var xbrlDoc = new XbrlDocument();
            var unitTestDir = Directory.GetCurrentDirectory();
            xbrlDoc.Load($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}IssueTests{Path.DirectorySeparatorChar}3{Path.DirectorySeparatorChar}offentliggorelse.xml");
            var loadedSchema = xbrlDoc.XbrlFragments[0].Schemas[0];
            Assert.AreNotEqual(loadedSchema.SchemaReferencePath, loadedSchema.LoadPath);
        }
    }
}
