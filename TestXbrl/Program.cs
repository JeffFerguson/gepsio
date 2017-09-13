using JeffFerguson.Gepsio;

namespace TestXbrl
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrl = new XbrlDocument();
            xbrl.Load("..\\..\\TestFiles\\mmm-20161231.xml");
        }
    }
}
