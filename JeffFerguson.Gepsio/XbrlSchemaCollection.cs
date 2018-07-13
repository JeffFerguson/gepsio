using JeffFerguson.Gepsio.Xml.Interfaces;
using JeffFerguson.Gepsio.Xsd;
using System.Collections;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// A collection of XBRL schemas.
    /// </summary>
    /// <remarks>
    /// This class contains, among other things, wrappers for methdods on the <see cref="XbrlSchema"/> class. The wrappers
    /// iterate through each schema to find requested information, freeing the caller from having to iterate through
    /// multiple schemas to find information.
    /// </remarks>
    public class XbrlSchemaCollection : IEnumerable<XbrlSchema>
    {
        internal List<XbrlSchema> SchemaList { get; private set; }

        private static Dictionary<string, string> StandardNamespaceSchemaLocationDictionary;

        /// <summary>
        /// The number of schemas in the collection.
        /// </summary>
        public int Count
        {
            get { return SchemaList.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public XbrlSchema this[int index]
        {
            get { return SchemaList[index]; }
            set { SchemaList.Insert(index, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<XbrlSchema> GetEnumerator()
        {
            return SchemaList.GetEnumerator();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        static XbrlSchemaCollection()
        {
            BuildStandardNamespaceSchemaLocationDictionary();
        }

        internal XbrlSchemaCollection()
        {
            SchemaList = new List<XbrlSchema>();
        }

        /// <summary>
        /// Builds a dictionary of standard, pre-defined namespaces and
        /// corresponding schema locations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Some XBRL document instances may contain facts that reference namespaces
        /// defined by external specifications and schemas. For example, the Document
        /// Information and Entity Information schema defines a namespace of
        /// http://xbrl.us/dei/2009-01-31. This namespace is defined by the schema at
        /// http://taxonomies.xbrl.us/us-gaap/2009/non-gaap/dei-2009-01-31.xsd.
        /// </para>
        /// <para>
        /// XBRL instances will not (generally) explictly load schemas defined by external
        /// specifications with a &gt;schemaRef&lt; tag; they may, however, define facts
        /// with the namespaces defined by these external specifications.
        /// </para>
        /// <para>
        /// This method builds a dictionary of standard, well-known, externally defined
        /// namespaces and corresponding schema locations so that, if Gepsio needs
        /// element information from one of these schemas, it knows where to find the
        /// corresponding schema.
        /// </para>
        /// <para>
        /// Some of these entries were collected from the following sources:
        /// <list type="bullet">
        /// <listitem>
        /// https://www.sec.gov/info/edgar/edgartaxonomies.xml
        /// </listitem>
        /// </list>
        /// </para>
        /// </remarks>
        private static void BuildStandardNamespaceSchemaLocationDictionary()
        {
            StandardNamespaceSchemaLocationDictionary = new Dictionary<string, string>
            {
                { "http://xbrl.us/dei/2009-01-31", "http://taxonomies.xbrl.us/us-gaap/2009/non-gaap/dei-2009-01-31.xsd" },
                { "http://xbrl.us/us-gaap/2009-01-31", "http://taxonomies.xbrl.us/us-gaap/2009/elts/us-gaap-std-2009-01-31.xsd" },
                { "http://xbrl.sec.gov/dei/2014-01-31", "http://xbrl.sec.gov/dei/2014/dei-2014-01-31.xsd" },
                { "http://fasb.org/dis/fifvdtmp01/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-fifvdtmp01-2018-01-31.xsd" },
                { "http://fasb.org/dis/fifvdtmp02/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-fifvdtmp02-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp011/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp011-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp012/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp012-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp021/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp021-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp022/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp022-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp03/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp03-2018-01-31.xsd" },
                { "http://fasb.org/dis/idestmp04/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-idestmp04-2018-01-31.xsd" },
                { "http://fasb.org/dis/leasestmp01/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-leasestmp01-2017-01-31.xsd" },
                { "http://fasb.org/dis/leasestmp02/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-leasestmp02-2017-01-31.xsd" },
                { "http://fasb.org/dis/leasestmp03/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-leasestmp03-2017-01-31.xsd" },
                { "http://fasb.org/dis/leasestmp04/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-leasestmp04-2017-01-31.xsd" },
                { "http://fasb.org/dis/leasestmp05/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-leasestmp05-2017-01-31.xsd" },
                { "http://fasb.org/dis/leastmp01/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-leastmp01-2018-01-31.xsd" },
                { "http://fasb.org/dis/leastmp02/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-leastmp02-2018-01-31.xsd" },
                { "http://fasb.org/dis/leastmp03/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-leastmp03-2018-01-31.xsd" },
                { "http://fasb.org/dis/leastmp04/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-leastmp04-2018-01-31.xsd" },
                { "http://fasb.org/dis/leastmp05/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-leastmp05-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp011/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp011-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp011/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp011-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp012/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp012-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp012/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp012-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp02/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp02-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp02/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp02-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp03/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp03-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp03/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp03-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp04/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp04-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp04/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp04-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp041/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp041-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp041/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp041-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp05/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp05-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp05/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp05-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp06/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp06-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp06/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp06-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp07/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp07-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp07/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp07-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp08/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp08-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp08/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp08-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp09/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp09-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp09/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp09-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp102/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp102-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp102/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp102-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp103/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp103-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp103/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp103-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp104/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp104-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp104/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp104-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp105/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp105-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp105/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp105-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp111/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp111-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp111/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp111-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp112/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp112-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp112/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp112-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp121/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp121-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp121/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp121-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp122/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp122-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp122/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp122-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp123/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp123-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp123/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp123-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp125/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp125-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp125/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp125-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp131/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp131-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp131/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp131-2018-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp141/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rbtmp141-2017-01-31.xsd" },
                { "http://fasb.org/dis/rbtmp141/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rbtmp141-2018-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp01/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rcctmp01-2017-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp01/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rcctmp01-2018-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp03/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rcctmp03-2017-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp03/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rcctmp03-2018-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp04/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rcctmp04-2017-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp04/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rcctmp04-2018-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp05/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/dis/us-gaap-dis-rcctmp05-2017-01-31.xsd" },
                { "http://fasb.org/dis/rcctmp05/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/dis/us-gaap-dis-rcctmp05-2018-01-31.xsd" },
                { "http://fasb.org/srt-roles/2018-01-31", "http://xbrl.fasb.org/srt/2018/elts/srt-roles-2018-01-31.xsd" },
                { "http://fasb.org/srt-types/2018-01-31", "http://xbrl.fasb.org/srt/2018/elts/srt-types-2018-01-31.xsd" },
                { "http://fasb.org/srt/2018-01-31", "http://xbrl.fasb.org/srt/2018/elts/srt-2018-01-31.xsd" },
                { "http://fasb.org/us-gaap/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/elts/us-gaap-2017-01-31.xsd" },
                { "http://fasb.org/us-gaap/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/elts/us-gaap-2018-01-31.xsd" },
                { "http://fasb.org/us-roles/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/elts/us-roles-2017-01-31.xsd" },
                { "http://fasb.org/us-roles/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/elts/us-roles-2018-01-31.xsd" },
                { "http://fasb.org/us-types/2017-01-31", "http://xbrl.fasb.org/us-gaap/2017/elts/us-types-2017-01-31.xsd" },
                { "http://fasb.org/us-types/2018-01-31", "http://xbrl.fasb.org/us-gaap/2018/elts/us-types-2018-01-31.xsd" },
                { "http://www.xbrl.org/2009/arcrole/fact-explanatoryFact", "http://www.xbrl.org/lrr/arcrole/factExplanatory-2009-12-16.xsd" },
                { "http://www.xbrl.org/2009/role/negated", "http://www.xbrl.org/lrr/role/negated-2009-12-16.xsd" },
                { "http://www.xbrl.org/2009/role/net", "http://www.xbrl.org/lrr/role/net-2009-12-16.xsd" },
                { "http://www.xbrl.org/dtr/type/non-numeric", "http://www.xbrl.org/dtr/type/nonNumeric-2009-12-16.xsd" },
                { "http://www.xbrl.org/dtr/type/numeric", "http://www.xbrl.org/dtr/type/numeric-2009-12-16.xsd" },
                { "http://xbrl.ifrs.org/taxonomy/2016-03-31/ifrs-full", "http://xbrl.ifrs.org/taxonomy/2016-03-31/full_ifrs/full_ifrs-cor_2016-03-31.xsd" },
                { "http://xbrl.ifrs.org/taxonomy/2017-03-09/ifrs-full", "http://xbrl.ifrs.org/taxonomy/2017-03-09/full_ifrs/full_ifrs-cor_2017-03-09.xsd" },
                { "http://xbrl.ifrs.org/taxonomy/2018-03-16/ifrs-full", "http://xbrl.ifrs.org/taxonomy/2018-03-16/full_ifrs/full_ifrs-cor_2018-03-16.xsd" },
                { "http://xbrl.sec.gov/country/2016-01-31", "http://xbrl.sec.gov/country/2016/country-2016-01-31.xsd" },
                { "http://xbrl.sec.gov/country/2017-01-31", "https://xbrl.sec.gov/country/2017/country-2017-01-31.xsd" },
                { "http://xbrl.sec.gov/currency/2016-01-31", "http://xbrl.sec.gov/currency/2016/currency-2016-01-31.xsd" },
                { "http://xbrl.sec.gov/currency/2017-01-31", "https://xbrl.sec.gov/currency/2017/currency-2017-01-31.xsd" },
                { "http://xbrl.sec.gov/dei/2012-01-31", "http://xbrl.sec.gov/dei/2012/dei-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/dei/2018-01-31", "https://xbrl.sec.gov/dei/2018/dei-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/exch/2017-01-31", "http://xbrl.sec.gov/exch/2017/exch-2017-01-31.xsd" },
                { "http://xbrl.sec.gov/exch/2018-01-31", "https://xbrl.sec.gov/exch/2018/exch-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/invest/2012-01-31", "http://xbrl.sec.gov/invest/2012/invest-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/invest/2013-01-31", "https://xbrl.sec.gov/invest/2013/invest-2013-01-31.xsd" },
                { "http://xbrl.sec.gov/naics/2011-01-31", "http://xbrl.sec.gov/naics/2011/naics-2011-01-31.xsd" },
                { "http://xbrl.sec.gov/naics/2017-01-31", "http://xbrl.sec.gov/naics/2017/naics-2017-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-cal/2012-01-31", "http://xbrl.sec.gov/rr/2012/rr-cal-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-cal/2018-01-31", "https://xbrl.sec.gov/rr/2018/rr-cal-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-def/2012-01-31", "http://xbrl.sec.gov/rr/2012/rr-def-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-ent/2012-01-31", "http://xbrl.sec.gov/rr/2012/rr-ent-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-ent/2018-01-31", "https://xbrl.sec.gov/rr/2018/rr-ent-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-pre/2012-01-31", "http://xbrl.sec.gov/rr/2012/rr-pre-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/rr-pre/2018-01-31", "https://xbrl.sec.gov/rr/2018/rr-pre-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/rr/2012-01-31", "http://xbrl.sec.gov/rr/2012/rr-2012-01-31.xsd" },
                { "http://xbrl.sec.gov/rr/2018-01-31", "https://xbrl.sec.gov/rr/2018/rr-2018-01-31.xsd" },
                { "http://xbrl.sec.gov/sic/2011-01-31", "https://xbrl.sec.gov/sic/2011/sic-2011-01-31.xsd" },
                { "http://xbrl.sec.gov/stpr/2011-01-31", "http://xbrl.sec.gov/stpr/2011/stpr-2011-01-31.xsd" },
                { "http://xbrl.sec.gov/stpr/2018-01-31", "https://xbrl.sec.gov/stpr/2018/stpr-2018-01-31.xsd" }
            };
        }

        /// <summary>
        /// Adds a schema to the schema collection.
        /// </summary>
        /// <remarks>
        /// The supplied schema will not be added if its target namespace is already in the list. This will help
        /// with some of the XBRL instance documents in the XBRL Conformance Suite which uses both the "schemaLocation"
        /// attribute as well as a "schemaRef" node to specify the same schema. The "301-01-IdScopeValid.xml"
        /// instance document in the XBRL-CONF-CR5-2012-01-24 suite is one such example.
        /// </remarks>
        /// <param name="schemaToAdd">
        /// The schema to be added.
        /// </param>
        internal void Add(XbrlSchema schemaToAdd)
        {
            var targetNamespaceAlreadyInList = false;
            foreach (var currentSchema in SchemaList)
            {
                if (schemaToAdd.TargetNamespace.Equals(currentSchema.TargetNamespace) == true)
                    targetNamespaceAlreadyInList = true;
            }
            if (targetNamespaceAlreadyInList == false)
                SchemaList.Add(schemaToAdd);
        }

        /// <summary>
        /// Gets the schema containing the element with the given local name.
        /// </summary>
        /// <param name="elementLocalName">
        /// The local name of the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the schema containing the element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        public XbrlSchema GetSchemaContainingElement(string elementLocalName)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var FoundElement = CurrentSchema.GetElement(elementLocalName);
                if (FoundElement != null)
                    return CurrentSchema;
            }
            return null;
        }

        /// <summary>
        /// Finds an defined schema element with a given local name.
        /// </summary>
        /// <param name="elementLocalName">
        /// The local name of the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the matching element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        public Element GetElement(string elementLocalName)
        {
            var containingSchema = GetSchemaContainingElement(elementLocalName);
            if(containingSchema == null)
                return null;
            return containingSchema.GetElement(elementLocalName);
        }

        /// <summary>
        /// Gets the schema having the target namespace.
        /// </summary>
        /// <param name="targetNamespace">
        /// The namespace whose schema should be returned.
        /// </param>
        /// <param name="parentFragment">
        /// The fragment containing the schema reference.
        /// </param>
        /// <returns>
        /// A reference to the schema matching the target namespace. A null reference will be returned
        /// if no matching schema can be found. 
        /// </returns>
        public XbrlSchema GetSchemaFromTargetNamespace(string targetNamespace, XbrlFragment parentFragment)
        {
            var foundSchema = FindSchema(targetNamespace);
            if(foundSchema == null)
            {

                // There is no loaded schema for the target namespace. The target
                // namespace be an industry-standard namespace referencing a schema
                // that has not been explicitly loaded. Find out if the namespace is
                // a standard one, and, if so, load its corresponding schema and
                // retry the search.

                string schemaLocation;
                StandardNamespaceSchemaLocationDictionary.TryGetValue(targetNamespace, out schemaLocation);
                if (string.IsNullOrEmpty(schemaLocation) == true)
                    return null;
                var newSchema = new XbrlSchema(parentFragment, schemaLocation, string.Empty);
                newSchema.TargetNamespaceAlias = targetNamespace;
                Add(newSchema);
                foundSchema = FindSchema(targetNamespace);
            }
            return foundSchema;
        }

        /// <summary>
        /// Gets the schema having the target namespace.
        /// </summary>
        /// <param name="targetNamespace">
        /// The namespace whose schema should be returned.
        /// </param>
        /// <returns>
        /// A reference to the schema matching the target namespace. A null reference will be returned
        /// if no matching schema can be found. 
        /// </returns>
        private XbrlSchema FindSchema(string targetNamespace)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                if (CurrentSchema.TargetNamespace.Equals(targetNamespace) == true)
                    return CurrentSchema;
                if (CurrentSchema.TargetNamespaceAlias.Equals(targetNamespace) == true)
                    return CurrentSchema;
            }
            return null;
        }

        /// <summary>
        /// Gets the data type for the supplied node.
        /// </summary>
        /// <param name="node">
        /// The node whose data type is returned.
        /// </param>
        /// <returns>
        /// The data type of the supplied node. A null reference will be returned
        /// if no matching element can be found for the supplied node.
        /// </returns>
        internal AnyType GetNodeType(INode node)
        {
            var containingSchema = GetSchemaContainingElement(node.LocalName);
            if (containingSchema == null)
                return null;
            var matchingElement = containingSchema.GetElement(node.LocalName);
            if (matchingElement == null)
                return null;
            return AnyType.CreateType(matchingElement.TypeName.Name, containingSchema);
        }

        /// <summary>
        /// Gets the data type for the supplied attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute whose data type is returned.
        /// </param>
        /// <returns>
        /// The data type of the supplied attribute. A null reference will be returned
        /// if no matching element can be found for the supplied attribute.
        /// </returns>
        internal AnyType GetAttributeType(IAttribute attribute)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingAttributeType = CurrentSchema.GetAttributeType(attribute);
                if (matchingAttributeType != null)
                    return matchingAttributeType;
            }
            return null;
        }

        /// <summary>
        /// Locates and element using an element locator.
        /// </summary>
        /// <param name="ElementLocator">
        /// A locator for the element to be found.
        /// </param>
        /// <returns>
        /// A reference to the matching element. A null reference will be returned
        /// if no matching element can be found.
        /// </returns>
        internal Element LocateElement(Locator ElementLocator)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingElement = CurrentSchema.LocateElement(ElementLocator);
                if (matchingElement != null)
                    return matchingElement;
            }
            return null;
        }

        /// <summary>
        /// Finds the <see cref="RoleType"/> object having the given ID.
        /// </summary>
        /// <param name="RoleTypeId">
        /// The ID of the role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="RoleType"/> object having the given ID, or null if no
        /// object can be found.
        /// </returns>
        internal RoleType GetRoleType(string RoleTypeId)
        {
            foreach (var CurrentSchema in SchemaList)
            {
                var matchingRoleType = CurrentSchema.GetRoleType(RoleTypeId);
                if (matchingRoleType != null)
                    return matchingRoleType;
            }
            return null;
        }

        /// <summary>
        /// Finds the <see cref="CalculationLink"/> object having the given role.
        /// </summary>
        /// <param name="CalculationLinkRole">
        /// The role type to find.
        /// </param>
        /// <returns>
        /// The <see cref="CalculationLink"/> object having the given role, or
        /// null if no object can be found.
        /// </returns>
        public CalculationLink GetCalculationLink(RoleType CalculationLinkRole)
        {
            foreach (var currentSchema in SchemaList)
            {
                var calculationLinkCandidate = currentSchema.GetCalculationLink(CalculationLinkRole);
                if (calculationLinkCandidate != null)
                    return calculationLinkCandidate;
            }
            return null;
        }
    }
}
