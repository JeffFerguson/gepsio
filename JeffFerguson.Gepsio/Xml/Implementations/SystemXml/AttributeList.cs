using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class AttributeList : IAttributeList, IEnumerable, IEnumerator
    {
        private List<IAttribute> thisAttributeList;

        public object Current
        {
            get
            {
                return thisAttributeList.GetEnumerator().Current;
            }
        }

        public IAttribute this[string s]
        {
            get
            {
                return FindAttribute(s);
            }
        }

        public bool MoveNext()
        {
            return thisAttributeList.GetEnumerator().MoveNext();
        }

        public void Reset()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return thisAttributeList.GetEnumerator();
        }

        internal AttributeList()
        {
            thisAttributeList = new List<IAttribute>();
        }

        internal AttributeList(XmlAttribute[] xmlAttribute)
        {
            thisAttributeList = new List<IAttribute>();
            foreach (var currentAttribute in xmlAttribute)
            {
                var newAttribute = new Attribute(currentAttribute);
                thisAttributeList.Add(newAttribute);
            }
        }

        public void Add(IAttribute node)
        {
            thisAttributeList.Add(node);
        }

        public IAttribute FindAttribute(string name)
        {
            foreach (IAttribute currentAttribute in thisAttributeList)
            {
                if (currentAttribute.Name.Equals(name) == true)
                    return currentAttribute;
            }
            return null;
        }
    }
}
