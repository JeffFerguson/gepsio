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
    public interface ISchemaSet
    {
        IEnumerable< ISchema > Schemas { get; }
        /// <summary>
        /// A dictionary of qualified names and elements for all of the gobal elements in this schema set.
        /// </summary>
        Dictionary<IQualifiedName, ISchemaElement> GlobalElements { get; }

        /// <summary>
        /// A dictionary of qualified names and types for all of the gobal types in this schema set.
        /// </summary>
        Dictionary<IQualifiedName, ISchemaType> GlobalTypes { get; }

        /// <summary>
        /// A dictionary of qualified names and attributes for all of the gobal attributes in this schema set.
        /// </summary>
        Dictionary<IQualifiedName, ISchemaAttribute> GlobalAttributes { get; }

        /// <summary>
        /// Adds a schema to the schema set.
        /// </summary>
        /// <param name="schemaToAdd">
        /// The schema to be added to the schema set.
        /// </param>
        void Add(ISchema schemaToAdd);

        /// <summary>
        /// Compiles the schema set.
        /// </summary>
        void Compile();
    }
}
