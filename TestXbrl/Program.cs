using JeffFerguson.Gepsio;

namespace TestXbrl
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrl = new XbrlDocument();
            //var xbrlDoc = "..\\..\\TestFiles\\\mmm\\mmm-20161231.xml";
            var xbrlDoc = "..\\..\\TestFiles\\dutch\\offentliggorelse.xml";

            xbrl.Load(xbrlDoc);
        }
    }
}
