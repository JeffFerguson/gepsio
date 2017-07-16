using JeffFerguson.Gepsio.Xlink;
using JeffFerguson.Gepsio.Xml.Interfaces;
using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// An encapsulation of the XBRL element "calculationLink" as defined in the http://www.xbrl.org/2003/linkbase namespace. 
    /// </summary>
    public class CalculationLink : XlinkNode
    {
        /// <summary>
        /// The linkbase document containing this calculation link.
        /// </summary>
        public LinkbaseDocument Linkbase { get; private set; }

        /// <summary>
        /// A collection of <see cref="Locator"/> objects that apply to this calculation link.
        /// </summary>
        public List<Locator> Locators { get; private set; }

        /// <summary>
        /// A collection of <see cref="CalculationArc"/> objects that apply to this calculation link.
        /// </summary>
        public List<CalculationArc> CalculationArcs { get; private set; }

        /// <summary>
        /// A collection of <see cref="SummationConcept"/> objects that apply to this calculation link.
        /// </summary>
        public List<SummationConcept> SummationConcepts { get; private set; }

        /// <summary>
        /// The URI of the role for this calculation link.
        /// </summary>
        /// <remarks>
        /// See http://gepsio.wordpress.com/2014/03/12/searching-for-balance-sheet-calculations-with-the-sep-2013-ctp
        /// for more information regarding the rationale for this property.
        /// </remarks>
        public Uri RoleUri { get; private set; }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        internal CalculationLink(LinkbaseDocument linkbaseDoc, INode CalculationLinkNode) : base(CalculationLinkNode)
        {
            this.Linkbase = linkbaseDoc;
            this.Locators = new List<Locator>();
            this.CalculationArcs = new List<CalculationArc>();
            this.SummationConcepts = new List<SummationConcept>();
            this.RoleUri = new Uri(this.Role);
            ReadChildLocators(CalculationLinkNode);
            ReadChildCalculationArcs(CalculationLinkNode);
            ResolveLocators();
            BuildSummationConcepts();
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void BuildSummationConcepts()
        {
            foreach (CalculationArc CurrentCalculationArc in this.CalculationArcs)
                BuildSummationConcepts(CurrentCalculationArc);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void BuildSummationConcepts(CalculationArc CurrentCalculationArc)
        {
            SummationConcept CurrentSummationConcept;

            CurrentSummationConcept = FindSummationConcept(CurrentCalculationArc.FromLocator);
            if (CurrentSummationConcept == null)
            {
                CurrentSummationConcept = new SummationConcept(this, CurrentCalculationArc.FromLocator);
                this.SummationConcepts.Add(CurrentSummationConcept);
            }
            foreach(var CurrentToLocator in CurrentCalculationArc.ToLocators)
                CurrentSummationConcept.AddContributingConceptLocator(CurrentToLocator);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private SummationConcept FindSummationConcept(Locator SummationConceptLocator)
        {
            foreach (SummationConcept CurrentSummationConcept in this.SummationConcepts)
            {
                if (CurrentSummationConcept.SummationConceptLocator.Equals(SummationConceptLocator) == true)
                    return CurrentSummationConcept;
            }
            return null;
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators()
        {
            foreach (CalculationArc CurrentCalculationArc in this.CalculationArcs)
                ResolveLocators(CurrentCalculationArc);
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ResolveLocators(CalculationArc CurrentCalculationArc)
        {
            CurrentCalculationArc.FromLocator = GetLocator(CurrentCalculationArc.FromId);
            foreach (Locator CurrentLocator in this.Locators)
            {
                if (CurrentLocator.Label.Equals(CurrentCalculationArc.ToId) == true)
                    CurrentCalculationArc.AddToLocator(CurrentLocator);
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildCalculationArcs(INode CalculationLinkNode)
        {
            foreach (INode CurrentChildNode in CalculationLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("calculationArc") == true)
                    this.CalculationArcs.Add(new CalculationArc(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private void ReadChildLocators(INode CalculationLinkNode)
        {
            foreach (INode CurrentChildNode in CalculationLinkNode.ChildNodes)
            {
                if (CurrentChildNode.LocalName.Equals("loc") == true)
                    this.Locators.Add(new Locator(CurrentChildNode));
            }
        }

        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        private Locator GetLocator(string LocatorLabel)
        {
            foreach (Locator CurrentLocator in this.Locators)
            {
                if (CurrentLocator.Label.Equals(LocatorLabel) == true)
                    return CurrentLocator;
            }
            return null;
        }

        /// <summary>
        /// Find the calculation arc that is referenced by the given locator.
        /// </summary>
        /// <remarks>
        /// The "to" link is searched.
        /// </remarks>
        /// <param name="SourceLocator">The locator used as the source of the search.</param>
        /// <returns>The CalculationArc referenced by the Locator, or null if a calculation arc cannot be found.</returns>
        internal CalculationArc GetCalculationArc(Locator SourceLocator)
        {
            foreach (CalculationArc CurrentCalculationArc in CalculationArcs)
            {
                if (CurrentCalculationArc.ToId.Equals(SourceLocator.Label) == true)
                    return CurrentCalculationArc;
            }
            return null;
        }
    }
}
