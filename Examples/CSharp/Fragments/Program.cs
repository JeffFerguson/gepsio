using JeffFerguson.Gepsio;
using System;

namespace Fragments
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");

            Console.WriteLine($"Number of fragments in the loaded document: {xbrlDoc.XbrlFragments.Count}.");
            foreach(var currentFragment in xbrlDoc.XbrlFragments)
            {
                // Work with the fragment here.
            }
        }
    }
}
