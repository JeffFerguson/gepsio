using JeffFerguson.Gepsio.Xsd;
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
    public interface ISchemaElement
    {
        /// <summary>
        /// The ID of the element.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// True if the element is abstract; false otherwise.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// The name of the element.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The fully qualified name of the schema's type.
        /// </summary>
        IQualifiedName SchemaTypeName { get; }

        /// <summary>
        /// the name of the substitution group for this element.
        /// </summary>
        IQualifiedName SubstitutionGroup { get; }

        /// <summary>
        /// The list of unhandled attributes for this element.
        /// </summary>
        IAttributeList UnhandledAttributes { get; }

        /// <summary>
        /// The list of attributes for this element.
        /// </summary>
        List<ISchemaAttribute> SchemaAttributes { get; }
    }
}
