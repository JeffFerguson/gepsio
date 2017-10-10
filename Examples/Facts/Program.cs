using JeffFerguson.Gepsio;
using System;

namespace Facts
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrlDoc1 = new XbrlDocument();
            xbrlDoc1.Load(@"..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");
            ShowFactsInDocument(xbrlDoc1);
            FindFactInDocument(xbrlDoc1, "changeInRetainedEarnings");

            var xbrlDoc2 = new XbrlDocument();
            xbrlDoc2.Load(@"..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\306-02-RequiredInstanceTupleValid.xml");
            ShowFactsInDocument(xbrlDoc2);
        }

        private static void ShowFactsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowFactsInFragment(currentFragment);
            }
        }

        private static void FindFactInDocument(XbrlDocument doc, string factName)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                FindFactInFragment(currentFragment, factName);
            }
        }

        private static void ShowFactsInFragment(XbrlFragment currentFragment)
        {
            foreach(var currentFact in currentFragment.Facts)
            {
                ShowFact(currentFact);
            }
        }

        private static void FindFactInFragment(XbrlFragment currentFragment, string factName)
        {
            var factFound = currentFragment.Facts.GetFactByName(factName);
            if (factFound != null)
                ShowFact(factFound);
        }

        private static void ShowFact(Fact fact)
        {
            Console.WriteLine($"FACT {fact.Name}");
            if (fact is Item)
            {
                ShowItem(fact as Item);
            }
            else if (fact is JeffFerguson.Gepsio.Tuple)
            {
                ShowTuple(fact as JeffFerguson.Gepsio.Tuple);
            }
        }

        private static void ShowItem(Item item)
        {
            Console.WriteLine("\tType     : Item");
            Console.WriteLine($"\tNamespace: {item.Namespace}");
            Console.WriteLine($"\tValue    : {item.Value}");
        }

        private static void ShowTuple(JeffFerguson.Gepsio.Tuple tuple)
        {
            Console.WriteLine("\tType     : Tuple");
            foreach(var currentFact in tuple.Facts)
            {
                ShowFact(currentFact);
            }
        }
    }
}
