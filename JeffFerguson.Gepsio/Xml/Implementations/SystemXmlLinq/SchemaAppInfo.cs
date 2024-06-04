using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq;

internal class SchemaAppInfo : ISchemaAppInfo 
{
    private readonly XmlSchemaAppInfo thisXmlSchemaAppInfo;

    public SchemaAppInfo(XmlSchemaAppInfo xmlSchemaAppInfo) 
    {
        thisXmlSchemaAppInfo = xmlSchemaAppInfo;
    }

    #region Implementation of ISchemaAppInfo

        public IEnumerable< INode > Markup => thisXmlSchemaAppInfo.Markup?.Select( x=>new SystemXml.Node( x ) );

    #endregion
}
