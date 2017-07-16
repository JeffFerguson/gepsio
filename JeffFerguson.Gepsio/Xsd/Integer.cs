using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "integer" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class Integer : Decimal
    {
        internal Integer(INode StringRootNode) : base(StringRootNode)
        {
        }

        /// <summary>
        /// Determines whether or not the supplied string value can be converted to an integer.
        /// </summary>
        /// <param name="valueAsString">
        /// The original string-based value.
        /// </param>
        /// <returns>
        /// True if the supplied string value can be converted to an integer; false otherwise.
        /// </returns>
        internal override bool CanConvert(string valueAsString)
        {
            int ParsedResult;

            return int.TryParse(valueAsString, out ParsedResult);
        }
    }
}
