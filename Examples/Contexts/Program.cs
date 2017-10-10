using JeffFerguson.Gepsio;
using System;

namespace Contexts
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrlDoc1 = new XbrlDocument();
            xbrlDoc1.Load(@"..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");
            ShowContextsInDocument(xbrlDoc1);
            ShowFactsInDocument(xbrlDoc1);
        }

        private static void ShowContextsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowContextsInFragment(currentFragment);
            }
        }

        private static void ShowContextsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentContext in currentFragment.Contexts)
            {
                ShowContext(currentContext);
            }
        }

        private static void ShowContext(Context currentContext)
        {
            Console.WriteLine("CONTEXT");
            Console.WriteLine($"\tID          : {currentContext.Id}");
             Console.Write($"\tPeriod Type : ");
            if(currentContext.InstantPeriod)
            {
                Console.WriteLine("instant");
                Console.WriteLine($"\tInstant Date: {currentContext.InstantDate}");
            }
            else if(currentContext.DurationPeriod)
            {
                Console.WriteLine("period");
                Console.WriteLine($"\tPeriod Date : from {currentContext.PeriodStartDate} to {currentContext.PeriodEndDate}");
            }
            else if(currentContext.ForeverPeriod)
            {
                Console.WriteLine("forever");
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
            Console.WriteLine("\tType      : Item");
            Console.WriteLine($"\tNamespace : {item.Namespace}");
            Console.WriteLine($"\tValue     : {item.Value}");
            Console.WriteLine($"\tContext ID: {item.ContextRefName}");
            if (item.ContextRef != null)
                ShowContext(item.ContextRef);
        }
    }
}
