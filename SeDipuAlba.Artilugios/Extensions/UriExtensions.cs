using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SeDipuAlba.Artilugios.Extensions
{
    /// <summary>
    /// Extensions for Uri class
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// Based on Brinkie code: https://stackoverflow.com/a/19679135/2126607
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add.</param>
        /// <param name="urlEncode">Encode the parameter using HttpUtility.UrlEncode.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue, bool urlEncode = true)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = urlEncode?HttpUtility.UrlEncode(paramValue):paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }


        /// <summary>
        /// Adds the specified parameter to the Query String without encoding the parameter value.
        /// 
        /// Difference vs <see cref="AddParameter(Uri, string, string, bool)"/> when <c>urlEncode</c> is false:
        /// <list type="bullet">
        /// <item><description><c>AddParameterRaw</c> appends the parameter verbatim to the URL (no encoding or normalization).</description></item>
        /// <item><description><c>AddParameter(..., urlEncode: false)</c> does not call <c>HttpUtility.UrlEncode</c> for the value but still uses <c>HttpUtility.ParseQueryString</c> and <c>UriBuilder</c>, which may apply encoding or normalization when rebuilding the query.</description></item>
        /// </list>
        /// Use <c>AddParameterRaw</c> when you need the exact raw text inserted into the query string.
        /// </summary>
        /// <param name="url">The base URL.</param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add (not encoded).</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameterRaw(this Uri url, string paramName, string paramValue)
        {
            var separator = url.Query.Contains("?") ? "&" : "?";
            var newUrl = url + separator + paramName + "=" + paramValue;
            return new Uri(newUrl);
        }
    }
}
