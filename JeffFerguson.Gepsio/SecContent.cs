using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// Static methods supporting the capture of content from the SEC Web site.
    /// </summary>
    /// <remarks>
    /// The SEC Web site does not allow code to scrape documents from the site
    /// without supplying appropriate HTTP headers. Without the correct HTTP
    /// headers, simply calling XDocument.Load() for a document stored at the 
    /// SEC Web site will fail, most likely with an HTTP 403 error code. Since
    /// Gepsio contains unit tests that reference documents stored at the SEC Web
    /// site, support for SEC-compatible HTTP headers is necessary. See documentation
    /// at https://www.sec.gov/os/accessing-edgar-data for more information.
    /// </remarks>
    static internal class SecContent
    {
        private static string UserAgentValue = "Gepsio gepsioxbrl@outlook.com";
        private static string AcceptEncodingValue = "gzip, deflate";
        private static string HostValue = "www.sec.gov";

        /// <summary>
        /// Determines whether or not a URI references the SEC Web site.
        /// </summary>
        /// <param name="uriPath">
        /// The URI to check.
        /// </param>
        /// <returns>
        /// True if the supplied URI is an SEC URI; false otherwise.
        /// </returns>
        static internal bool IsSecUri(string uriPath)
        {
            try
            {
                var uriToInspect = new Uri(uriPath);
                return uriToInspect.Host.ToLower().Trim().Equals("www.sec.gov");
            }
            catch (UriFormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Synchronously gets a stream of data sourced from content hosted by the SEC.
        /// </summary>
        /// <param name="uriPath">
        /// The URI to the hosted content.
        /// </param>
        /// <returns>
        /// A stream of data from the hosted content.
        /// </returns>
        static internal Stream GetStream(string uriPath)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uriPath))
            {
                var httpClientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", UserAgentValue);
                    request.Headers.Add("Accept-Encoding", AcceptEncodingValue);
                    request.Headers.Add("Host", HostValue);
                    var response = httpClient.Send(request);
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStream();
                }
            }
        }

        /// <summary>
        /// Asynchronously gets a stream of data sourced from content hosted by the SEC.
        /// </summary>
        /// <param name="uriPath">
        /// The URI to the hosted content.
        /// </param>
        /// <returns>
        /// A stream of data from the hosted content.
        /// </returns>
        static internal async Task<Stream> GetStreamAsync(string uriPath)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uriPath))
            {
                var httpClientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", UserAgentValue);
                    request.Headers.Add("Accept-Encoding", AcceptEncodingValue);
                    request.Headers.Add("Host", HostValue);
                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStreamAsync();
                }
            }
        }
    }
}
