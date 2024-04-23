using System.Xml;

namespace JeffFerguson.Gepsio.Test
{
    /// <summary>
    /// A variation of a test case as defined in an XBRL-CONF-CR3-2012-01-24 test case document.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Test case variations are found in one of the conformance suite test documents.
    /// A test case document is an XML document with the following form:
    /// </para>
    /// <para>
    /// &lt;testcase xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" name="Identifier Scope" description="Section 4.3  The Item Element" outpath="out" owner="mg@fla.fujitsu.com" xsi:noNamespaceSchemaLocation="../lib/test.xsd" minimal="true"&gt;
    /// &lt;variation id="..." name="..."&gt;
    /// &lt;description&gt;...&lt;/description&gt;
    /// &lt;data&gt;
    /// &lt;xsd readMeFirst="[true|false]"&gt;...&lt;/xsd&gt;
    /// &lt;instance readMeFirst="[true|false]"&gt;...&lt;/instance&gt;
    /// &lt;/data&gt;
    /// &lt;result expected="[valid|invalid]"/&gt;
    /// &lt;/variation&gt;
    /// ... other &lt;testcase&gt; nodes ...
    /// &lt;/testcase&gt;
    /// </para>
    /// <para>
    /// This class represents one of the &lt;variation&gt; nodes as well as the content in its child nodes.
    /// </para>
    /// </remarks>
    internal class XbrlConformanceTestVariation
    {
        internal string Id { get; private set; }
        internal string Name { get; private set; }
        internal string Description { get; private set; }
        internal string Xsd { get; private set; }
        internal string Instance { get; private set; }
        internal bool ReadXsdFirst { get; private set; }
        internal bool ReadInstanceFirst { get; private set; }
        internal bool ValidityExpected { get; private set; }

        internal XbrlConformanceTestVariation(XmlNode VariationNode)
        {
            var descriptionNode = VariationNode.SelectSingleNode("child::description");
            var dataNode = VariationNode.SelectSingleNode("child::data");
            var resultNode = VariationNode.SelectSingleNode("child::result");
            var xsdNode = dataNode.SelectSingleNode("child::xsd");
            var instanceNode = dataNode.SelectSingleNode("child::instance");

            this.Id = VariationNode.Attributes["id"].Value;
            this.Name = VariationNode.Attributes["name"].Value;
            this.Description = descriptionNode.InnerText;
            if (xsdNode != null)
            {
                this.Xsd = xsdNode.InnerText;
                this.ReadXsdFirst = false;
                if (xsdNode.Attributes["readMeFirst"] != null)
                {
                    string ReadXsdFirstText = xsdNode.Attributes["readMeFirst"].Value;
                    if (ReadXsdFirstText == "true")
                        this.ReadXsdFirst = true;
                    else
                        this.ReadXsdFirst = false;
                }
            }
            else
            {
                this.Xsd = string.Empty;
                this.ReadXsdFirst = false;
            }

            if (instanceNode != null)
            {
                this.Instance = instanceNode.InnerText;
                string ReadInstanceFirstText = instanceNode.Attributes["readMeFirst"].Value;
                if (ReadInstanceFirstText == "true")
                    this.ReadInstanceFirst = true;
                else
                    this.ReadInstanceFirst = false;

                string ExpectedResultText = resultNode.Attributes["expected"].Value;
                if (ExpectedResultText == "valid")
                    this.ValidityExpected = true;
                else
                    this.ValidityExpected = false;
            }
            else
            {
                this.Instance = string.Empty;
            }
        }
    }
}
