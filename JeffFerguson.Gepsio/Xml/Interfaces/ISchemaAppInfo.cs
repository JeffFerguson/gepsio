using System.Collections.Generic;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Interfaces;

public interface ISchemaAppInfo 
{
    IEnumerable< INode > Markup { get; }
}
