namespace Sublight.Core
{
    public abstract class Globals
    {
        /// <summary>
        /// To get your API key please contact us at: http://www.sublight.me/contact/
        /// </summary>
        public static string API_CLIENT_ID = "YOUR_CLIENT_ID";

        /// <summary>
        /// To get your API secret please contact us at: http://www.sublight.me/contact/
        /// </summary>
        public static string API_CLIENT_KEY = "YOUR_CLIENT_KEY";

        //internal static string API_URL_HTTPS = "https://www.sublight.me/API/REST/JSON/ServiceSecure.svc";
        public static string API_URL = "http://www.sublight.me/API/REST/JSON/Service.svc";
        public static string API_URL_SANDBOX = "http://www.sublight.me/API/REST/JSON/ServiceSandbox.svc";

        public static bool IsSandboxMode
        {
            get; set;
        }

        internal abstract class Errors
        {
            public const string SYSTEM_ERROR = "SystemError";
        }
    }
}
