using JeffFerguson.Gepsio;
using System;

/// <summary>
/// This sample, written by request from a Gepsio user, illustrates the correct way to
/// find fact weights from a calculation linkbase.
/// </summary>
namespace FactWeights
{
    class Program
    {
        static void Main(string[] args)
        {
            var xbrlDoc = new XbrlDocument();
            Console.Write("Loading...");
            xbrlDoc.Load("https://www.sec.gov/Archives/edgar/data/845877/000084587717000155/agm-20170930.xml");
            Console.WriteLine("done.");
            var calculationLinkbase = FindFirstAvailableCalculationLinkbase(xbrlDoc);
            PrintArcFactsAndWeights(calculationLinkbase);
        }

        /// <summary>
        /// Prints all facts and weights in calculation arcs to the console.
        /// </summary>
        /// <param name="calculationLinkbase">
        /// A calculation linkbase containing the calculation arcs to be output to the console.
        /// </param>
        private static void PrintArcFactsAndWeights(CalculationLinkbaseDocument calculationLinkbase)
        {
            if (calculationLinkbase == null)
                return;
            foreach(var currentLink in calculationLinkbase.CalculationLinks)
            {
                foreach(var currentArc in currentLink.CalculationArcs)
                {
                    Console.WriteLine($"From: {currentArc.FromLocator.HrefResourceId}");
                    foreach(var currentToLocator in currentArc.ToLocators)
                    {
                        Console.WriteLine($"\tTo: {currentToLocator.HrefResourceId}");
                    }
                    Console.WriteLine($"\tWeight: {currentArc.Weight}");
                }
            }
        }

        /// <summary>
        /// Find the first available calculation linkbase for a loaded XBRL document.
        /// </summary>
        /// <param name="xbrlDoc">
        /// The loaded XBRL document to be searched.
        /// </param>
        /// <returns>
        /// The first available calculation linkbase in the XBRL document. Null is
        /// returned if no document is available.
        /// </returns>
        private static CalculationLinkbaseDocument FindFirstAvailableCalculationLinkbase(XbrlDocument xbrlDoc)
        {
            foreach(var currentFragment in xbrlDoc.XbrlFragments)
            {
                foreach(var currentSchema in currentFragment.Schemas)
                {
                    if(currentSchema.CalculationLinkbase != null)
                    {
                        return currentSchema.CalculationLinkbase;
                    }
                }
            }
            return null;
        }
    }
}
