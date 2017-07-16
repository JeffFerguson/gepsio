using JeffFerguson.Gepsio.Xml.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml
{
    internal class NodeList : INodeList, IEnumerable, IEnumerator
    {
        private List<INode> thisNodeList;

        public int Count
        {
            get
            {
                return thisNodeList.Count;
            }
        }

        public object Current
        {
            get
            {
                return thisNodeList.GetEnumerator().Current;
            }
        }

        public INode this[int i]
        {
            get
            {
                return thisNodeList[i];
            }
        }

        public bool MoveNext()
        {
            return thisNodeList.GetEnumerator().MoveNext();
        }

        public void Reset()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return thisNodeList.GetEnumerator();
        }

        internal NodeList()
        {
            thisNodeList = new List<INode>();
        }

        public void Add(INode node)
        {
            thisNodeList.Add(node);
        }

        public bool StructureEquals(INodeList OtherNodeList)
        {
            if (OtherNodeList == null)
                return false;
            if (this.Count != OtherNodeList.Count)
                return false;
            for (int NodeIndex = 0; NodeIndex < this.Count; NodeIndex++)
            {
                if (this[NodeIndex].StructureEquals(OtherNodeList[NodeIndex]) == false)
                    return false;
            }
            return true;
        }
    }
}
