using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System.Text;

namespace JeffFerguson.Gepsio.Validators.Xbrl2Dot1
{
    /// <summary>
    /// A validator for XBRL context elements.
    /// </summary>
    /// <remarks>
    /// This validator validates the information within a context element and
    /// needs no information from any other fragment-level object.
    /// </remarks>
    internal class ContextValidator
    {
        private XbrlFragment validatingFragment;
        private Context validatingContext;

        internal void Validate(XbrlFragment fragment, Context context)
        {
            this.validatingFragment = fragment;
            this.validatingContext = context;
            ValidateContextPeriod();
            ValidateScenario();
            ValidateSegment();
        }

        private void ValidateContextPeriod()
        {
            if ((validatingContext.PeriodStartDate != System.DateTime.MinValue) && (validatingContext.PeriodEndDate != System.DateTime.MinValue))
            {
                if (validatingContext.PeriodEndDate < validatingContext.PeriodStartDate)
                {
                    string MessageFormat = AssemblyResources.GetName("PeriodEndDateLessThanPeriodStartDate");
                    var MessageBuilder = new StringBuilder();
                    MessageBuilder.AppendFormat(MessageFormat, validatingContext.Id);
                    this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
                }
            }
        }

        private void ValidateScenario()
        {
            if (validatingContext.Scenario != null)
            {
                foreach (INode CurrentChild in validatingContext.Scenario.ChildNodes)
                    ValidateScenarioNode(CurrentChild);
            }
        }

        private void ValidateScenarioNode(INode ScenarioNode)
        {
            if (ScenarioNode.NamespaceURI.Equals(XbrlDocument.XbrlNamespaceUri) == true)
            {
                string MessageFormat = AssemblyResources.GetName("ScenarioNodeUsingXBRLNamespace");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, validatingContext.Id, ScenarioNode.Name);
                this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
            }
            if (ScenarioNode.Prefix.Length > 0)
            {
                XbrlSchema NodeSchema = this.validatingFragment.GetXbrlSchemaForPrefix(ScenarioNode.Prefix);
                if (NodeSchema != null)
                {
                    Element NodeElement = NodeSchema.GetElement(ScenarioNode.LocalName);
                    if (NodeElement != null)
                    {
                        if (NodeElement.SubstitutionGroup != Element.ElementSubstitutionGroup.Unknown)
                        {
                            string MessageFormat = AssemblyResources.GetName("ScenarioNodeUsingSubGroupInXBRLNamespace");
                            StringBuilder MessageBuilder = new StringBuilder();
                            MessageBuilder.AppendFormat(MessageFormat, validatingContext.Id, ScenarioNode.Name, NodeSchema.Path);
                            this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
                        }
                    }
                }
            }
            foreach (INode CurrentChild in ScenarioNode.ChildNodes)
                ValidateScenarioNode(CurrentChild);
        }

        private void ValidateSegment()
        {
            if(validatingContext.Segment != null)
            {
                foreach (INode CurrentChild in validatingContext.Segment.ChildNodes)
                    ValidateSegmentNode(CurrentChild);
            }
        }

        private void ValidateSegmentNode(INode SegmentNode)
        {
            ValidateSegmentNodeNamespace(SegmentNode);
            ValidateSegmentNodePrefix(SegmentNode);
            ValidateSegmentInnerText(SegmentNode);
            foreach (INode CurrentChild in SegmentNode.ChildNodes)
                ValidateSegmentNode(CurrentChild);
        }

        private void ValidateSegmentNodeNamespace(INode SegmentNode)
        {
            if (SegmentNode.NamespaceURI.Equals(XbrlDocument.XbrlNamespaceUri) == true)
            {
                string MessageFormat = AssemblyResources.GetName("SegmentNodeUsingXBRLNamespace");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, validatingContext.Id, SegmentNode.Name);
                this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
            }
        }

        private void ValidateSegmentNodePrefix(INode SegmentNode)
        {
            if (SegmentNode.Prefix.Length > 0)
            {
                XbrlSchema NodeSchema = this.validatingFragment.GetXbrlSchemaForPrefix(SegmentNode.Prefix);
                if (NodeSchema != null)
                {
                    Element NodeElement = NodeSchema.GetElement(SegmentNode.LocalName);
                    if (NodeElement != null)
                    {
                        if (NodeElement.SubstitutionGroup != Element.ElementSubstitutionGroup.Unknown)
                        {
                            string MessageFormat = AssemblyResources.GetName("SegmentNodeUsingSubGroupInXBRLNamespace");
                            StringBuilder MessageBuilder = new StringBuilder();
                            MessageBuilder.AppendFormat(MessageFormat, validatingContext.Id, SegmentNode.Name, NodeSchema.Path);
                            this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
                        }
                    }
                }
            }
        }

        private void ValidateSegmentInnerText(INode SegmentNode)
        {
            var text = SegmentNode.InnerText;
            if (string.IsNullOrEmpty(text) == true)
                return;
            if (this.validatingFragment.Schemas.Count == 0)
                return;
            var segmentNodeType = this.validatingFragment.Schemas.GetNodeType(SegmentNode);
            if (segmentNodeType == null)
                return;
            if (segmentNodeType.CanConvert(text) == false)
            {
                string MessageFormat = AssemblyResources.GetName("SegmentTextNotConvertable");
                StringBuilder MessageBuilder = new StringBuilder();
                MessageBuilder.AppendFormat(MessageFormat, text);
                this.validatingFragment.AddValidationError(new ContextValidationError(validatingContext, MessageBuilder.ToString()));
            }
        }
    }
}
