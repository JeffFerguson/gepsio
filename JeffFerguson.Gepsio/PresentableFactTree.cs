using JeffFerguson.Gepsio.Xsd;
using System.Collections.Generic;
using System.Linq;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A tree of facts arranged in a parent/child relationship
    /// according to a presentation linkbase.
    /// </summary>
    public class PresentableFactTree
    {
        /// <summary>
        /// A collection of top-level nodes in the tree.
        /// </summary>
        /// <remarks>
        /// This property is a list of top-level presentable tree nodes,
        /// each of which is represented by a class called
        /// <see cref="PresentableFactTreeNode"/>. Unlike traditional trees
        /// in computer science, which have a single "root" node at the top,
        /// presentation linkbases do not necessarily define a single "root"
        /// top level node. Gepsio, therefore, must prepare for the scenario
        /// in which the presentation linkbase has multiple "top-level" nodes
        /// and defines a list of top level nodes in a presentable fact tree.
        /// </remarks>
        public List<PresentableFactTreeNode> TopLevelNodes { get; private set; }

        internal PresentableFactTree(XbrlSchema schema, FactCollection facts)
        {
            TopLevelNodes = new List<PresentableFactTreeNode>();
            var presentationLinkbase = schema.PresentationLinkbases;
			foreach( var presentationLink in presentationLinkbase.SelectMany( x => x.PresentationLinks ) )
            {
                var unorderedPresentationArcs = presentationLink.PresentationArcs;
                var orderedPresentationArcs = unorderedPresentationArcs.OrderBy(o => o.Order).ToList();
                var newTreeNode = new PresentableFactTreeNode();
                TopLevelNodes.Add(newTreeNode);
                foreach(var orderedPresentationArc in orderedPresentationArcs)
                {

                    // Set the tree node's node fact, if it is not already set.
                    //
                    // This code is making an assumption that each arc in the link is set up with
                    // the same "from" locator, so locating the "from" fact only needs to be done
                    // if it is not already set. If it is already set, the code assumes that it
                    // was set from a previous arc and doesn't not need to be set again (presumably
                    // with the same information it would get this time if it did all of the work
                    // again).
                    //
                    // If the locator references an abstract element, then there will be no fact
                    // and the value will remain null.

                    if (newTreeNode.NodeFact == null)
                    {
                        var fromLocator = GetLocator(orderedPresentationArc.From, presentationLink);
                        newTreeNode.PresentationLinkbaseLocator = fromLocator;
                        newTreeNode.NodeLabel = GetLabel(schema, fromLocator);
                        var fromElement = GetElement(schema, fromLocator.HrefResourceId); 
                        if (fromElement.IsAbstract == false)
                        {
                            var fromFact = facts.GetFactByName(fromElement.Name);
                            newTreeNode.NodeFact = fromFact;
                        }                        
                    }
                    var toLocator = GetLocator(orderedPresentationArc.To, presentationLink);
                    var newChildTreeNode = new PresentableFactTreeNode();
                    newTreeNode.ChildNodes.Add(newChildTreeNode);
                    newTreeNode.NodeLabel = GetLabel(schema, toLocator);
                    newChildTreeNode.PresentationLinkbaseLocator = toLocator;                    
                    var toElement = GetElement(schema, toLocator.HrefResourceId);
                    if (toElement.IsAbstract == false)
                    {
                        var toFact = facts.GetFactByName(toElement.Name);
                        newChildTreeNode.NodeFact = toFact;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the label stored in a label linkbase for a given locator.
        /// </summary>
        /// <param name="schema">
        /// The compiled schema which whose elements should be searched.
        /// </param>
        /// <param name="labelLocator">
        /// The locator of the label to be returned.
        /// </param>
        /// <returns>
        /// The label for the given locator, or an empty string if there is no 
        /// label.
        /// </returns>
        private string GetLabel(XbrlSchema schema, Locator labelLocator)
        {
            var labelLinkbase = schema.LabelLinkbases;
            if (labelLinkbase == null)
                return string.Empty;
            foreach(var labelLink in labelLinkbase.SelectMany(x => x.LabelLinks))
            {
                var foundLabel = GetLabel(schema, labelLocator, labelLink);
                if (string.IsNullOrEmpty(foundLabel) == false)
                    return foundLabel;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the label stored in a label linkbase for a given locator.
        /// </summary>
        /// <param name="schema">
        /// The compiled schema which whose elements should be searched.
        /// </param>
        /// <param name="labelLocator">
        /// The locator of the label to be returned.
        /// </param>
        /// <param name="labelLink">
        /// The link label to be searched.
        /// </param>
        /// <returns>
        /// The label for the given locator, or an empty string if there is no 
        /// label.
        /// </returns>
        private string GetLabel(XbrlSchema schema, Locator labelLocator, LabelLink labelLink)
        {
            var labelLinkbase = schema.LabelLinkbases;
            if (labelLinkbase == null)
                return string.Empty;

            // Find label locator with href matching labelLocator.Href
            // Find label arc with from matching label locator.Label
            // find label with label matching label arc.To
            // grab innerText ... GET HYPE

            var labelLinkLocator = labelLink.GetLocator(labelLocator.Href);
            if (labelLinkLocator == null)
                return string.Empty;
            var labelArc = labelLink.GetLabelArc(labelLinkLocator.Label);
            if (labelArc == null)
                return string.Empty;
            var finalLabel = labelLink.GetLabel(labelArc.ToId);
            if (finalLabel == null)
                return string.Empty; 
            return finalLabel.Text;
        }

        /// <summary>
        /// Finds an element in the given schema with the given ID.
        /// </summary>
        /// <param name="schema">
        /// The compiled schema which whose elements should be searched.
        /// </param>
        /// <param name="id">
        /// The ID of the element to return.
        /// </param>
        /// <returns>
        /// The element whose ID matches the given ID, or null if no element can be found.
        /// </returns>
        private Element GetElement(XbrlSchema schema, string id)
        {
            foreach(var candidateElement in schema.Elements)
            {
                if (string.IsNullOrEmpty(candidateElement.Id) == false)
                {
                    if (candidateElement.Id.Equals(id) == true)
                        return candidateElement;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a locator in the given presentation link with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the locator to return.
        /// </param>
        /// <param name="presentationLink">
        /// The presentation link whose locators should be searched.
        /// </param>
        /// <returns>
        /// The locator whose label matches the given name, or null if no locator can be found.
        /// </returns>
        private Locator GetLocator(string name, PresentationLink presentationLink)
        {
            foreach(var candidateLocator in presentationLink.Locators)
            {
                if (candidateLocator.Label.Equals(name) == true)
                    return candidateLocator;
            }
            return null;
        }
    }
}
