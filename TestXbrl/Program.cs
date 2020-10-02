using JeffFerguson.Gepsio;

namespace TestXbrl
{
    class Program
    {
        static void Main()
        {
            var xbrl = new XbrlDocument();
            var xbrlDoc = "..\\..\\..\\TestFiles\\mmm\\mmm-20161231.xml";
            //var xbrlDoc = "..\\..\\..\\TestFiles\\dutch\\offentliggorelse.xml";
            //var xbrlDoc = "..\\..\\..\\TestFiles\\ibm\\ibm-20170630.xml";

            xbrl.Load(xbrlDoc);
        }
    }
}
