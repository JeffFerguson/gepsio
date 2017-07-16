using JeffFerguson.Gepsio.Xml.Interfaces;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An encapsulation of the label arc element as defined in the http://www.xbrl.org/2003/linkbase namespace. 
	/// </summary>
	public class LabelArc
	{
		/// <summary>
		/// The ID of the "from" label referenced in the label arc.
		/// </summary>
		public string FromId { get; private set; }

		/// <summary>
		/// The ID of the "to" label referenced in the label arc.
		/// </summary>
		public string ToId { get; private set; }

		/// <summary>
		/// The locator of the "from" label referenced in the label arc.
		/// </summary>
		public Locator FromLocator { get; internal set; }

		//------------------------------------------------------------------------------------
		//------------------------------------------------------------------------------------
		internal LabelArc(INode LabelArcNode)
		{
            this.FromId = LabelArcNode.GetAttributeValue(Xlink.XlinkNode.xlinkNamespace, "from");
            this.ToId = LabelArcNode.GetAttributeValue(Xlink.XlinkNode.xlinkNamespace, "to");
		}
	}
}
