using JeffFerguson.Gepsio;
using System;

namespace ValidityChecks
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadValidInstance();
            LoadInvalidInstance();
        }

        static void LoadValidInstance()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml");
            CheckValidity(xbrlDoc);
        }

        static void LoadInvalidInstance()
        {
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(@"..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-10-FootnoteFromOutOfScope.xml");
            CheckValidity(xbrlDoc);
        }

        static void CheckValidity(XbrlDocument doc)
        {
            if(doc.IsValid == true)
            {
                Console.WriteLine("Congratulations! This document is valid according to the XBRL specification.");
            }
            else
            {
                Console.WriteLine("This document is invalid according to the XBRL specification.");
                foreach(var validationError in doc.ValidationErrors)
                {
                    Console.WriteLine(validationError.Message);
                }
            }
        }
    }
}
