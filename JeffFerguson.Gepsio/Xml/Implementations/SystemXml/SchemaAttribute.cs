using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Xml.Schema;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class SchemaAttribute : ISchemaAttribute
    {
        public string Name { get; private set; }

        public bool Optional { get; private set; }

        public bool Prohibited { get; private set; }

        public bool Required { get; private set; }

        public string FixedValue { get; internal set; }

        /// <summary>
        /// The type name of this attribute.
        /// </summary>
        public IQualifiedName TypeName { get; private set; }

        internal SchemaAttribute(XmlSchemaAttribute attribute)
        {
            this.Name = attribute.Name;
            this.Optional = false;
            this.Prohibited = false;
            this.Required = false;
            this.FixedValue = attribute.FixedValue;
            this.TypeName = new QualifiedName(attribute.SchemaTypeName);
            switch(attribute.Use)
            {
                case XmlSchemaUse.None:
                    break;
                case XmlSchemaUse.Optional:
                    this.Optional = true;
                    break;
                case XmlSchemaUse.Prohibited:
                    this.Prohibited = true;
                    break;
                case XmlSchemaUse.Required:
                    this.Required = true;
                    break;
                default:
                    var attributeUseAsString = attribute.Use.ToString();
                    throw new NotSupportedException($"Unsupported XmlSchemaUse enumeration '{attributeUseAsString}'.");
            }
        }
    }
}
