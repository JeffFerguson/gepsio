using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    internal class QualifiedName : IQualifiedName
    {
        private XmlQualifiedName thisQualifiedName;

        public string Name
        {
            get
            {
                return thisQualifiedName.Name;
            }
            set
            {
                var newQualifiedName = new XmlQualifiedName(value, thisQualifiedName.Namespace);
                thisQualifiedName = newQualifiedName;
            }
        }

        public string Namespace
        {
            get
            {
                return thisQualifiedName.Namespace;
            }
            set
            {
                var newQualifiedName = new XmlQualifiedName(thisQualifiedName.Name, value);
                thisQualifiedName = newQualifiedName;
            }
        }

        public string FullyQualifiedName
        {
            get
            {
                if (thisQualifiedName == null)
                    return string.Empty;
                if (string.IsNullOrEmpty(thisQualifiedName.Namespace) == true)
                    return thisQualifiedName.Name;
                return thisQualifiedName.Namespace + ":" + thisQualifiedName.Name;
            }
            set
            {
                var NamespaceAndName = value.Split(new char[] { ':' });
                if (NamespaceAndName.Length == 2)
                    thisQualifiedName = new XmlQualifiedName(NamespaceAndName[1], NamespaceAndName[0]);
                else
                    thisQualifiedName = new XmlQualifiedName(value);
            }
        }

        public QualifiedName()
        {
            thisQualifiedName = new XmlQualifiedName();
        }

        internal QualifiedName(XmlQualifiedName xmlQualified)
        {
            thisQualifiedName = xmlQualified;
        }
    }
}
