using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAndroidApp.Exceptions
{
    /// <summary>
    /// Custom exception for API-related errors
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public string RequestUrl { get; }

        public ApiException(HttpStatusCode statusCode, string content, string requestUrl, string message)
            : base(message)
        {
            StatusCode = statusCode;
            Content = content;
            RequestUrl = requestUrl;
        }

        public ApiException(HttpStatusCode statusCode, string content, string requestUrl, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Content = content;
            RequestUrl = requestUrl;
        }

        // Simplified constructor for common use
        public ApiException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public override string ToString()
        {
            return $"API Error: {Message}\nStatus: {StatusCode}\nURL: {RequestUrl}\nContent: {Content}";
        }
    }
}
