using System.Collections.Generic;

namespace JeffFerguson.Gepsio.Xml.Interfaces
{
    /// <summary>
    /// An interface to a specific XML implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface provides an abstraction to the actual XML service layer used by Gepsio. Different XML
    /// service layers may be used: the .NET 3.5 implementation may use the System.Xml classes, while a Portable
    /// Class Library implementation may use LINQ-to-XML. This interface abstracts away the XML implementation
    /// specifics so that the rest of Gepsio can use a standard interface to the XML service layer without
    /// knowledge of a specific implementation.
    /// </para>
    /// <para>
    /// The <see cref="JeffFerguson.Gepsio.IoC.Container"/> class provides a simple container mechanism for resolving interface types
    /// into a specific implementation.
    /// </para>
    /// </remarks>
    public interface ISchema
    {
        /// <summary>
        /// The list of namespaces referenced by the schema.
        /// </summary>
        List<IQualifiedName> Namespaces { get; }

        /// <summary>
        /// Reads a schema.
        /// </summary>
        /// <param name="path">
        /// The path to the schema file to be read.
        /// </param>
        /// <returns>
        /// True if the schema was read successfully; false otherwise.
        /// </returns>
        bool Read(string path);
    }
}
