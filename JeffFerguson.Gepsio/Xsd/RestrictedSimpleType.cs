using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xsd
{
    /// <summary>
    /// An encapsulation of the XML schema type "restrictedSimpleType" as defined in the http://www.w3.org/2001/XMLSchema namespace. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be considered deprecated and will most likely be removed in a future version of Gepsio. In early CTPs,
    /// Gepsio implemented its own XML schema parser, and this class was created for the implementation of the XML schema parser
    /// type system. In later CTPs, Gepsio levergaed the XML schema support already available in the .NET Framework, which rendered
    /// Gepsio's XML schema type system obsolete.
    /// </para>
    /// </remarks>
    public class RestrictedSimpleType : SimpleType
    {
        internal INode RestrictionNode
        {
            get;
            private set;
        }

        internal RestrictedSimpleType(INode SimpleTypeNode, INamespaceManager namespaceManager, INode RestrictionNode)
            : base(SimpleTypeNode, namespaceManager)
        {
            this.RestrictionNode = RestrictionNode;
        }
    }
}
