using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Xml;

namespace JeffFerguson.Gepsio.Xml.Implementation.SystemXml {
	/// <summary>
	/// Cached Xml Resolver
	/// </summary>
	public class CachedXmlResolver : XmlUrlResolver {
		/// <summary>
		/// Path to the directory containing cached files.
		/// </summary>
		private readonly string _path;
		/// <summary>
		/// Should the cache be populated by remote files ?
		/// </summary>
		private readonly bool _populate;
		ICredentials _credentials;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">Path to the cache directory.</param>
		/// <param name="populate">Populate cache with new remote files.</param>
		public CachedXmlResolver( string path, bool populate ) {
			var fullPath = Path.GetFullPath( path );
			if( !Directory.Exists( fullPath ) ) throw new Exception( $"Directory not found at {fullPath}." );
			this._path = fullPath;
			this._populate = populate;
		}

		public override ICredentials Credentials {
			set {
				this._credentials = value;
				base.Credentials = value;
			}
		}

		public override object GetEntity( Uri absoluteUri, string role, Type ofObjectToReturn ) {
			if( absoluteUri == null ) {
				throw new ArgumentNullException( "absoluteUri" );
			}
			//resolve resources from cache (if possible)
			if( absoluteUri.Scheme == "http" && (ofObjectToReturn == null || ofObjectToReturn == typeof( Stream )) ) {
				var filename = Path.Combine( this._path, absoluteUri.Host + absoluteUri.AbsolutePath );
				if( File.Exists( filename ) ) {
					return new FileStream( filename, FileMode.Open, FileAccess.Read );
				}

				WebRequest webReq = WebRequest.Create( absoluteUri );
				webReq.CachePolicy = new HttpRequestCachePolicy( HttpRequestCacheLevel.Default );
				if( this._credentials != null ) {
					webReq.Credentials = this._credentials;
				}
				WebResponse resp = webReq.GetResponse( );
				var responseStream = resp.GetResponseStream( );
				if( _populate ) {
					Directory.CreateDirectory( Path.GetDirectoryName( filename ) );
					var fileStream = File.Create( filename );
					responseStream.CopyTo( fileStream );
					responseStream.Close(  );
					fileStream.Position = 0;
					return fileStream;
				}
				return responseStream;
			}
			//otherwise use the default behavior of the XmlUrlResolver class (resolve resources from source)
			else {
				return base.GetEntity( absoluteUri, role, ofObjectToReturn );
			}
		}
	}
}
