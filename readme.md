# What is Gepsio?
Gepsio is a document object model for [XBRL](https://www.xbrl.org/) documents. The object model is built using .NET Standard 2.0 and will work with any software development platform that can consume .NET Standard 2.0 assemblies.

Load your XBRL document with the `XbrlDocument` class and work with your XBRL document exposed as a set of .NET Standard classes with a variety of properties and methods. Loaded XBRL documents are automatically validated against the information against the XBRL specification, and exceptions are thrown when invalid XBRL documents are loaded. The Gepsio code base is unit tested using the XBRL-CONF-2014-12-10 Conformance Suite tests designed by the XBRL organization.

# Getting Started
Working with Gepsio is extremely straightforward:

1. Reference the `JeffFerguson.Gepsio.dll` assembly in your .NET Standard-compatible project.
2. Create an instance of the `JeffFerguson.Gepsio.XbrlDocument` class.  
3. Call `Load()` on the `JeffFerguson.Gepsio.XbrlDocument` class instance, passing in, as a parameter, the path to the XBRL document you wish to load.

 If the loading is successful, then the call from `Load()` will return and you can begin inspecting the properties of your `JeffFerguson.Gepsio.XbrlDocument` object to examine your XBRL document. If the load was not successful, then Gepsio will populate the `XbrlDocument` object with a collection of error messages describing the issues encountered. The document's `IsValid` property will describe whether or not the document is valid.
 
 ```csharp
    using System;
    using JeffFerguson.Gepsio;

    namespace Sample
    {
        class Program
        {  
            static void Main(string[] args)  
            {  
                var Doc = new XbrlDocument();
                Doc.Load("xbrldoc.xml");
                if(Doc.IsValid == true)
                {
                    // The document is valid XBRL.
                }
                else
                {
                    // The document is not valid XBRL.
                }
            }  
        }
    }  
```

Once the document is loaded, you can use properties on the `XbrlDocument` object to inspect the properties of the loaded document. The wiki will be updated with examples as they become available.