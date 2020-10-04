using JeffFerguson.Gepsio;

namespace TestXbrl
{
    class Program
    {
        static void Main()
        {
            var xbrlDoc = "..\\..\\..\\TestFiles\\mmm\\mmm-20161231.xml";
            //var xbrlDoc = "..\\..\\..\\TestFiles\\dutch\\offentliggorelse.xml";
            //var xbrlDoc = "..\\..\\..\\TestFiles\\ibm\\ibm-20170630.xml";

            // TODO: need to find valid XML test case; already found XBRLFO20191101 taxonomy on XBRL.FO
            //var xbrlDoc = "..\\..\\..\\TestFiles\\faroese\\testFOALLentry.xml";

            var xbrlDocument = new XbrlDocument();
            xbrlDocument.Load(xbrlDoc);
        }
    }
}
