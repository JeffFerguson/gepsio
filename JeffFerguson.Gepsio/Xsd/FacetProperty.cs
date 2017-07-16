using System;

namespace JeffFerguson.Gepsio.Xsd
{
    internal class FacetProperty
    {
        private FacetPropertyDefinition thisDefinition;
        private AnyType thisValue;

        internal FacetPropertyDefinition Definition
        {
            get
            {
                return thisDefinition;
            }
        }

        internal AnyType Value
        {
            get
            {
                return thisValue;
            }
        }

        internal FacetProperty(FacetPropertyDefinition Definition, string ValueAsString)
        {
            thisDefinition = Definition;
            try
            {
                thisValue = Activator.CreateInstance(Definition.Datatype) as AnyType;
                thisValue.ValueAsString = ValueAsString;
            }
            catch (Exception e)
            {
                throw new NotSupportedException("INTERNAL ERROR: CreateInstance error in FacetProperty construction", e);
            }
        }
    }
}
