namespace Sublight.Core
{
    abstract class Globals
    {
        /// <summary>
        /// To get your API key please contact us at: http://www.sublight.me/contact/
        /// </summary>
        internal static string API_KEY = "YOUR_API_KEY";

        /// <summary>
        /// To get your API secret please contact us at: http://www.sublight.me/contact/
        /// </summary>
        internal static string API_SECRET = "YOUR_API_SECRET";

        //internal static string API_URL_HTTPS = "https://www.sublight.me/API/REST/JSON/ServiceSecure.svc";
        internal static string API_URL = "http://www.sublight.me/API/REST/JSON/Service.svc";

        internal abstract class Errors
        {
            public const string SYSTEM_ERROR = "SystemError";
        }
    }
}
