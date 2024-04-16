using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class NodeList : INodeList, IEnumerable, IEnumerator
    {
        private List<INode> thisNodeList;

        public int Count => thisNodeList.Count;

        public object Current => thisNodeList.GetEnumerator().Current;

        public INode this[int i] => thisNodeList[i];

        public bool MoveNext() => thisNodeList.GetEnumerator().MoveNext();

        public void Reset()
        {
        }

        public IEnumerator GetEnumerator() => thisNodeList.GetEnumerator();

        internal NodeList() => thisNodeList = new List<INode>();

        public void Add(INode node) => thisNodeList.Add(node);

        public bool StructureEquals(INodeList OtherNodeList, XbrlFragment containingFragment)
        {
            if (OtherNodeList == null)
                return false;
            if (this.Count != OtherNodeList.Count)
                return false;
            for (int NodeIndex = 0; NodeIndex < this.Count; NodeIndex++)
            {
                if (this[NodeIndex].StructureEquals(OtherNodeList[NodeIndex], containingFragment) == false)
                    return false;
            }
            return true;
        }
    }
}
