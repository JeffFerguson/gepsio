using JeffFerguson.Gepsio;
using System;

namespace Units
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrlDoc1 = new XbrlDocument();
            xbrlDoc1.Load(@"..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");
            ShowUnitsInDocument(xbrlDoc1);
            ShowFactsInDocument(xbrlDoc1);
        }

        private static void ShowUnitsInDocument(XbrlDocument doc)
        {
            foreach(var currentFragment in doc.XbrlFragments)
            {
                ShowUnitsInFragment(currentFragment);
            }
        }

        private static void ShowUnitsInFragment(XbrlFragment currentFragment)
        {
            foreach(var currentUnit in currentFragment.Units)
            {
                ShowUnit(currentUnit);
            }
        }

        private static void ShowUnit(Unit currentUnit)
        {
            Console.WriteLine("UNIT");
            Console.WriteLine($"\tID  : {currentUnit.Id}");
            foreach(var currentMeasureQualifiedName in currentUnit.MeasureQualifiedNames)
            {
                Console.WriteLine($"\tName: {currentMeasureQualifiedName.Namespace}:{currentMeasureQualifiedName.LocalName}");
            }
        }

        private static void ShowFactsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowFactsInFragment(currentFragment);
            }
        }

        private static void ShowFactsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentFact in currentFragment.Facts)
            {
                ShowFact(currentFact);
            }
        }

        private static void ShowFact(Fact fact)
        {
            Console.WriteLine($"FACT {fact.Name}");
            if (fact is Item)
            {
                ShowItem(fact as Item);
            }
        }

        private static void ShowItem(Item item)
        {
            Console.WriteLine("\tType     : Item");
            Console.WriteLine($"\tNamespace: {item.Namespace}");
            Console.WriteLine($"\tValue    : {item.Value}");
            Console.WriteLine($"\tUnit ID  : {item.UnitRefName}");
            if (item.UnitRef != null)
                ShowUnit(item.UnitRef);
        }
    }
}
