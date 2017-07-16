using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio.Xsd
{
    internal class PatternFacetDefinition : FacetDefinition
    {
        internal PatternFacetDefinition() : base("pattern")
        {
            AddFacetPropertyDefinition(new FacetPropertyDefinition("value", typeof(AnySimpleType)));
            AddFacetPropertyDefinition(new FacetPropertyDefinition("annotation", typeof(String), true));
        }
    }
}
