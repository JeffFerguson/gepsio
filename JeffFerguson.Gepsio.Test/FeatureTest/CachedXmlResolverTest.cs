using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using JeffFerguson.Gepsio.Xml.Implementation.SystemXml;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace JeffFerguson.Gepsio.Test.FeatureTest {
	[TestClass]
	public class CachedXmlResolverTest {
		[TestMethod]
		[Microsoft.VisualStudio.TestTools.UnitTesting.Description("Cached taxonomy files")]
		public void LoadRanTaxononmyFromCache( ) {
			IoC.Container.RegisterInstance< XmlResolver >( new CachedXmlResolver( @"..\..\..\FeatureTest\CachedXmlResolver\Cache", false ) );
			var xbrlDoc = new XbrlDocument();
			xbrlDoc.Load(@"..\..\..\FeatureTest\CachedXmlResolver\BILCANTON_RAN.xbrl");
			if( !xbrlDoc.IsValid ) {
				Assert.Fail( xbrlDoc.ValidationErrors.First().Message );
			}
		}
		[TestMethod]
		[Microsoft.VisualStudio.TestTools.UnitTesting.Description("Populate cache with remote taxonomy files")]
		public void PopulateCacheFromUrl( ) {
			//create temporary cache directory
			var cachePath = @"..\..\..\FeatureTest\CachedXmlResolver\Cache\tmp";
			Directory.CreateDirectory( cachePath );

			//load without cache
			Logger.LogMessage( "Loading instance wihout cache..." );
			IoC.Container.RegisterInstance<XmlResolver>( new XmlUrlResolver( ) );
			var startTime = DateTime.Now;

			var xbrlDoc = new XbrlDocument();
			xbrlDoc.Load(@"https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
			if( !xbrlDoc.IsValid ) {
				Assert.Fail( xbrlDoc.ValidationErrors.First().Message );
			}

			var span = DateTime.Now - startTime;
			Logger.LogMessage( "duration (without cache) : {0}", span );

			//reload populating cache
			Logger.LogMessage( "Loading instance populating cache..." );
			
			IoC.Container.RegisterInstance< XmlResolver >( new CachedXmlResolver( cachePath, true ) );
			startTime = DateTime.Now;

			xbrlDoc = new XbrlDocument();
			xbrlDoc.Load(@"https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
			if( !xbrlDoc.IsValid ) {
				Assert.Fail( xbrlDoc.ValidationErrors.First().Message );
			}

			span = DateTime.Now - startTime;
			Logger.LogMessage( "duration (populating) : {0}", span );

			//reload from cache
			Logger.LogMessage( "Loading instance using cache..." );
			startTime = DateTime.Now;

			xbrlDoc = new XbrlDocument();
			xbrlDoc.Load(@"https://www.sec.gov/Archives/edgar/data/1688568/000168856818000036/csc-20170331.xml");
			if( !xbrlDoc.IsValid ) {
				Assert.Fail( xbrlDoc.ValidationErrors.First().Message );
			}

			span = DateTime.Now - startTime;
			Logger.LogMessage( "duration (from cache) : {0}", span );

			//clear cache
			Directory.Delete( cachePath, true );
		}
	}
}
