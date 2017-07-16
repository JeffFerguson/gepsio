
namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// An encapsulation of a URL and an element ID.
	/// </summary>
    public class HyperlinkReference
    {
        private string thisHrefValue;

		/// <summary>
		/// The URL associated with this hyperlink reference.
		/// </summary>
		public string Url { get; private set; }

		/// <summary>
		/// True if a URL was specified for this hyperlink reference; false otherwise.
		/// </summary>
        public bool UrlSpecified
        {
            get
            {
                if (this.Url.Length == 0)
                    return false;
                return true;
            }
        }

		/// <summary>
		/// The element ID associated with this hyperlink reference.
		/// </summary>
		public string ElementId { get; private set; }

		/// <summary>
		/// True if an element ID was specified for this hyperlink reference; false otherwise.
		/// </summary>
        public bool ElementIdSpecified
        {
            get
            {
                if (this.ElementId.Length == 0)
                    return false;
                return true;
            }
        }

        internal HyperlinkReference(string Href)
        {
            thisHrefValue = Href;
            int PoundSignIndex = thisHrefValue.IndexOf('#');
            if (PoundSignIndex == -1)
            {
                this.ElementId = thisHrefValue;
                this.Url = string.Empty;
                return;
            }
            if (PoundSignIndex == 0)
            {
                this.Url = string.Empty;
                this.ElementId = thisHrefValue.Substring(1);
                return;
            }
            char [] Pound = { '#' };
            string[] Values = thisHrefValue.Split(Pound);
            this.Url = Values[0];
            this.ElementId = Values[1];
        }

		/// <summary>
		/// Formats the hyperlink reference as a string.
		/// </summary>
		/// <returns>
		/// The hyperlink reference formatted as a string.
		/// </returns>
        public override string ToString()
        {
            return thisHrefValue;
        }
    }
}
