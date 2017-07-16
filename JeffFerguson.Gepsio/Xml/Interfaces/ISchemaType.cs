
namespace JeffFerguson.Gepsio.Xml.Interfaces
{
    /// <summary>
    /// An interface to a global type defined in an XML schema. The interface supports both simple and complex types.
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
    public interface ISchemaType
    {
        /// <summary>
        /// The qualified name of the type.
        /// </summary>
        IQualifiedName QualifiedName { get; }

        /// <summary>
        /// The name of the type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// True if the type is numeric; false otherwise.
        /// </summary>
        bool IsNumeric { get; }

        /// <summary>
        /// True if the type is complex; false otherwise.
        /// </summary>
        bool IsComplex { get; }

        /// <summary>
        /// True if the type is derived by restriction; false otherwise.
        /// </summary>
        bool DerivedByRestriction { get; }

        /// <summary>
        /// The base type for this type.
        /// </summary>
        ISchemaType BaseSchemaType { get; }

        /// <summary>
        /// Gets a named schema attribute.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute to be returned.
        /// </param>
        /// <returns>
        /// The attribute with the given name.
        /// </returns>
        ISchemaAttribute GetAttribute(string name);
    }
}
