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
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = HttpUtility.UrlEncode(paramValue);
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

    }
}
