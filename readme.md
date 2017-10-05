# What is Gepsio?
Gepsio is a document object model for [XBRL](https://www.xbrl.org/) documents. The object model is built using .NET Standard 2.0 and will work with any software development platform that can consume .NET Standard 2.0 assemblies.

Load your XBRL document with the `XbrlDocument` class and work with your XBRL document exposed as a set of .NET Standard classes with a variety of properties and methods. Loaded XBRL documents are automatically validated against the information against the XBRL specification, and exceptions are thrown when invalid XBRL documents are loaded. The Gepsio code base is unit tested using the XBRL-CONF-2014-12-10 Conformance Suite tests designed by the XBRL organization.

The [Wiki](https://github.com/JeffFerguson/gepsio/wiki) includes a section called "Working with Gepsio" that describes how to use Gepsio to work with XBRL document instances.