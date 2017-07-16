
namespace JeffFerguson.Gepsio.Xsd
{
    internal class FacetPropertyDefinition
    {
        private string thisName;
        private System.Type thisDatatype;
        private string thisDefaultValue;
        private bool thisOptional;

        internal string Name
        {
            get
            {
                return thisName;
            }
        }

        internal System.Type Datatype
        {
            get
            {
                return thisDatatype;
            }
        }

        internal string DefaultValue
        {
            get
            {
                return thisDefaultValue;
            }
        }

        internal bool Optional
        {
            get
            {
                return thisOptional;
            }
        }

        internal FacetPropertyDefinition(string Name, System.Type PropertyType)
            : this(Name, PropertyType, string.Empty, false)
        {
        }

        internal FacetPropertyDefinition(string Name, System.Type PropertyType, string DefaultValue)
            : this(Name, PropertyType, DefaultValue, false)
        {
        }

        internal FacetPropertyDefinition(string Name, System.Type PropertyType, bool Optional)
            : this(Name, PropertyType, string.Empty, Optional)
        {
        }

        internal FacetPropertyDefinition(string Name, System.Type PropertyType, string DefaultValue, bool Optional)
        {
            thisName = Name;
            thisDatatype = PropertyType;
            thisDefaultValue = DefaultValue;
            thisOptional = Optional;
        }
    }
}
