using JeffFerguson.Gepsio;

namespace Load
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadFromLocalFile();
            LoadFromUrl();
        }

        static void LoadFromLocalFile()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");
        }

        static void LoadFromUrl()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml");
        }
    }
}
