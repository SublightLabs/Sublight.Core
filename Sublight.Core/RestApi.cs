using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sublight.Core.Types;

namespace Sublight.Core
{
    public static class RestApi
    {
        public static async Task<Result<Guid>> LogIn()
        {
            try
            {
                var urlQuery = new NameValueCollection();
                urlQuery.Add("clientId", Globals.API_CLIENT_ID);
                urlQuery.Add("apiKey", Globals.API_CLIENT_KEY);

                using (var client = new HttpClient())
                using (var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, GetAbsoluteUrl("login-4", urlQuery))).ConfigureAwait(false))
                using (var content = response.Content)
                {
                    var jsonResponse = await content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonObj = JObject.Parse(jsonResponse);
                    var strGuid = jsonObj.Value<string>("LogIn4Result");
                    var session = new Guid(strGuid);

                    if (session == Guid.Empty)
                    {
                        var strError = jsonObj.Value<string>(JSON_FIELD_ERROR);
                        if (string.IsNullOrWhiteSpace(strError))
                        {
                            return Result<Guid>.CreateError(Globals.Errors.SYSTEM_ERROR);
                        }
                        return Result<Guid>.CreateError(strError);
                    }

                    return Result<Guid>.CreateSuccess(session);
                }
            }
            catch (Exception ex)
            {
                return Result<Guid>.CreateException(ex);
            }
        }

        private const string JSON_FIELD_ERROR = "error";

        private static string GetAbsoluteUrl(string relativeUrl, NameValueCollection urlQuery = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}/{1}", Globals.API_URL, relativeUrl);

            if (urlQuery != null && urlQuery.Collection.Count > 0)
            {
                sb.Append("?");
                foreach (var nv in urlQuery.Collection)
                {
                    sb.AppendFormat("{0}={1}&", nv.Key, Uri.EscapeUriString(nv.Value.ToString()));
                }
            }

            return sb.ToString();
        }
    }
}
