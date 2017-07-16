using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
	/// <summary>
	/// The representation of a summation concept used in a calculation arc.
	/// </summary>
	/// <remarks>>
	/// Section 5.2.5.2 of the XBRL 2.1 Specification discusses summation concepts.
	/// </remarks>
	public class SummationConcept
	{
		/// <summary>
		/// The locator for the summation concept.
		/// </summary>
		public Locator SummationConceptLocator { get; private set; }

		/// <summary>
		/// The calculation link that contains this summation concept.
		/// </summary>
		public CalculationLink Link { get; private set; }

		/// <summary>
		/// A collection of <see cref="Locator"/> objects for the contributing concepts.
		/// </summary>
		public List<Locator> ContributingConceptLocators { get; private set; }

		//------------------------------------------------------------------------------------
		//------------------------------------------------------------------------------------
		internal SummationConcept(CalculationLink link, Locator SummationConceptLocator)
		{
			this.Link = link;
			this.SummationConceptLocator = SummationConceptLocator;
			this.ContributingConceptLocators = new List<Locator>();
		}

		//------------------------------------------------------------------------------------
		//------------------------------------------------------------------------------------
		internal void AddContributingConceptLocator(Locator ContributingConceptLocator)
		{
			if (this.ContributingConceptLocators.Contains(ContributingConceptLocator) == false)
				this.ContributingConceptLocators.Add(ContributingConceptLocator);
		}
	}
}
