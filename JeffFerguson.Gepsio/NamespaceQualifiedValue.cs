
using JeffFerguson.Gepsio.Xml.Interfaces;
namespace JeffFerguson.Gepsio
{
    internal class NamespaceQualifiedValue
    {
        private string thisFullyQualifiedValue;
        private string[] thisFullyQualifiedValueComponents;

        internal bool HasNamespace
        {
            get
            {
                if (this.Namespace.Length == 0)
                    return false;
                return true;
            }
        }

        internal string LocalName { get; private set; }

        internal string Namespace { get; private set; }

        internal string NamespaceUri { get; private set; }

        internal NamespaceQualifiedValue(INamespaceManager NamespaceManager, string FullyQualifiedValue)
        {
            thisFullyQualifiedValue = FullyQualifiedValue;
            thisFullyQualifiedValueComponents = thisFullyQualifiedValue.Split(':');
            if (thisFullyQualifiedValueComponents.Length == 1)
            {
                this.LocalName = thisFullyQualifiedValueComponents[0];
                this.Namespace = string.Empty;
                this.NamespaceUri = string.Empty;
            }
            else
            {
                this.LocalName = thisFullyQualifiedValueComponents[1];
                this.Namespace = thisFullyQualifiedValueComponents[0];
                this.NamespaceUri = NamespaceManager.LookupNamespace(this.Namespace);
            }
        }

        internal bool Equals(string NamespaceUri, string LocalName)
        {
            if (this.NamespaceUri.ToLower().Equals(NamespaceUri.ToLower()) == false)
                return false;
            if (this.LocalName.ToLower().Equals(LocalName.ToLower()) == false)
                return false;
            return true;
        }
    }
}
