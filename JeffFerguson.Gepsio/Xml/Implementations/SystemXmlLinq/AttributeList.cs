using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXmlLinq
{
    internal class AttributeList : IAttributeList, IEnumerable, IEnumerator
    {
        private List<IAttribute> thisAttributeList;

        public object Current => thisAttributeList.GetEnumerator().Current;

        public IAttribute this[string s] => FindAttribute(s);

        public bool MoveNext() => thisAttributeList.GetEnumerator().MoveNext();

        public void Reset()
        {
        }

        public IEnumerator GetEnumerator() => thisAttributeList.GetEnumerator();

        internal AttributeList() => thisAttributeList = new List<IAttribute>();

        internal AttributeList(XAttribute[] xmlAttribute)
        {
            thisAttributeList = new List<IAttribute>();
            foreach (var currentAttribute in xmlAttribute)
            {
                var newAttribute = new Attribute(currentAttribute);
                thisAttributeList.Add(newAttribute);
            }
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

        public void Add(IAttribute node) => thisAttributeList.Add(node);

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
