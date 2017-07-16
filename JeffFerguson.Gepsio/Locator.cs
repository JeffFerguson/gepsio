using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An encapsulation of the locator functionality as defined in the http://www.w3.org/1999/xlink namespace. 
	/// </summary>
	public class Locator : XlinkNode
	{
		/// <summary>
		/// The document URI for the locator's hyperlink reference.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Hyperlink references for locators may be specified with a fragment identifier,
		/// which has the form "url#id". If the locator's hyperlink reference includes a
		/// fragment identifier, then the document URI will contain the URL portion of the
		/// hyperlink reference. If the locator's hyperlink reference does not include a
		/// fragment identifier, then the document URI will match the hyperlink reference.
		/// </para>
		/// <list type="table">
		/// <listeader>
		/// <term>
		/// Href
		/// </term>
		/// <description>
		/// HrefDocumentUri
		/// </description>
		/// </listeader>
		/// <item>
		/// <term>
		/// http://www.xbrl.org/doc.xml#ElementID
		/// </term>
		/// <description>
		/// http://www.xbrl.org/doc.xml
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// http://www.xbrl.org/doc.xml
		/// </term>
		/// <description>
		/// http://www.xbrl.org/doc.xml
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		public string HrefDocumentUri { get; private set; }

		/// <summary>
		/// The resource ID for the locator's hyperlink reference.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Hyperlink references for locators may be specified with a fragment identifier,
		/// which has the form "url#id". If the locator's hyperlink reference includes a
		/// fragment identifier, then the resource ID will specify the identifer. If the
		/// locator's hyperlink reference does not include a fragment identifier, then the
		/// resource ID will be empty.
		/// </para>
		/// <list type="table">
		/// <listeader>
		/// <term>
		/// Href
		/// </term>
		/// <description>
		/// HrefResourceId
		/// </description>
		/// </listeader>
		/// <item>
		/// <term>
		/// http://www.xbrl.org/doc.xml#ElementID
		/// </term>
		/// <description>
		/// ElementID
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// http://www.xbrl.org/doc.xml
		/// </term>
		/// <description>
		/// (empty string)
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		public string HrefResourceId { get; private set; }

		//-------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------
		internal Locator(INode LocatorNode) : base(LocatorNode)
		{
            if (string.IsNullOrEmpty(this.Href) == false)
                ParseHref();
		}

		/// <summary>
		/// Compares a supplied href references with the href stored in the Locator.
		/// </summary>
		/// <remarks>
		/// This method describes a match on the href portion of the locator, not the ID
		/// portion.
		/// </remarks>
		/// <param name="HrefMatchCandidate">
		/// The hyperlink reference to compare against the locator.
		/// </param>
		/// <returns>
		/// Returns true if the supplied href references the same location as the href
		/// stored in the Locator, and false otherwise.
		/// </returns>
		internal bool HrefEquals(string HrefMatchCandidate)
		{
			if (HrefMatchCandidate.Length == 0)
				return true;
			if (this.HrefDocumentUri.Length < HrefMatchCandidate.Length)
				return HrefMatchCandidate.EndsWith(this.HrefDocumentUri);
			return this.Href.Equals(HrefMatchCandidate);
		}

        /// <summary>
        /// Parses the href into its component parts.
        /// </summary>
        /// <remarks>
        /// This method supports the possibility that the resource ID may
        /// be URI encoded. For example, one of the XBRL conformance tests
        /// use a locator with a resource ID of "tx_Espa%c3%b1a1". After
        /// decoding, the resource ID should be "tx_España1".
        /// </remarks>
		private void ParseHref()
		{
			string[] HrefSplit = this.Href.Split(new char[] { '#' });
			this.HrefDocumentUri = HrefSplit[0];
			if (HrefSplit.Length > 1)
				this.HrefResourceId = Uri.UnescapeDataString(HrefSplit[1]);
		}
	}
}
