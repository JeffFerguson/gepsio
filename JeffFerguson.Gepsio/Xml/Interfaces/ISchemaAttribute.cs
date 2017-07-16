
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
    public interface ISchemaAttribute
    {
        /// <summary>
        /// The name of the schema attribute.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// True if the schema attribute is optional; false otherwise.
        /// </summary>
        bool Optional { get; }

        /// <summary>
        /// True if the schema attribute is prohibited; false otherwise. 
        /// </summary>
        bool Prohibited { get; }

        /// <summary>
        /// True if the schema attribute is required; false otherwise. 
        /// </summary>
        bool Required { get; }

        /// <summary>
        /// The fixed value of the attribute.
        /// </summary>
        string FixedValue { get; }

        /// <summary>
        /// The type name for this attribute.
        /// </summary>
        IQualifiedName TypeName { get; }
    }
}
