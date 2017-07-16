using System;

namespace JeffFerguson.Gepsio
{
    internal class Attribute
    {
        public string Name { get; private set; }

        public Type ValueType { get; private set; }

        public bool Required { get; private set; }

        internal Attribute(string Name, Type ValueType, bool Required)
        {
            this.Name = Name;
            this.ValueType = ValueType;
            this.Required = Required;
        }
    }
}
