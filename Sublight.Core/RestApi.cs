using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sublight.Core.Types;

namespace Sublight.Core
{
    public static class RestApi
    {
        public static async Task<Result<Guid>> LogIn(string username, string password)
        {
            try
            {
                var urlQuery = new NameValueCollection();
                urlQuery.Add("clientId", Globals.API_CLIENT_ID);
                urlQuery.Add("apiKey", Globals.API_CLIENT_KEY);

                var postParams = new NameValueCollection();
                postParams.Add(JSON_FIELD_USERNAME, username);
                postParams.Add(JSON_FIELD_PASSWORD, password);
                var postData = JsonConvert.SerializeObject(postParams.Collection, new KeyValuePairConverter());

                using (var client = new HttpClient())
                using (var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, GetAbsoluteUrl("login-4", urlQuery)) { Content = new StringContent(postData) }).ConfigureAwait(false))
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

        public static async Task<Result<bool>> LogOut(Guid session)
        {
            try
            {
                var postParams = new NameValueCollection();
                postParams.Add(JSON_FIELD_SESSION, session);
                var postData = JsonConvert.SerializeObject(postParams.Collection, new KeyValuePairConverter());

                using (var client = new HttpClient())
                using (var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, GetAbsoluteUrl("logout")) { Content = new StringContent(postData) }).ConfigureAwait(false))
                using (var content = response.Content)
                {
                    var jsonResponse = await content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonObj = JObject.Parse(jsonResponse);

                    var strError = jsonObj.Value<string>(JSON_FIELD_ERROR);
                    if (!string.IsNullOrWhiteSpace(strError))
                    {
                        return Result<bool>.CreateError(strError);
                    }

                    return Result<bool>.CreateSuccess(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.CreateException(ex);
            }
        }

        public static async Task<Result<ImdbDetails>> GetImdbDetailsByHashOrFileName(Guid session, VideoDetails videoDetails)
        {
            try
            {
                var postParams = new NameValueCollection();
                postParams.Add(JSON_FIELD_SESSION, session);
                postParams.Add(JSON_FIELD_VIDEO_DETAILS, videoDetails);
                var postData = JsonConvert.SerializeObject(postParams.Collection, new KeyValuePairConverter());

                using (var client = new HttpClient())
                using (var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, GetAbsoluteUrl("get-details-by-hash-or-filename-2")) { Content = new StringContent(postData) }).ConfigureAwait(false))
                using (var content = response.Content)
                {
                    var jsonResponse = await content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonObj = JObject.Parse(jsonResponse);

                    var anyErrors = GetAnyErrors<ImdbDetails>(jsonObj, "GetDetailsByHashOrFileName2Result");
                    if (anyErrors != null)
                    {
                        return anyErrors;
                    }

                    var imdbDetails = CreateFromJson(jsonObj.GetValue("details"));
                    return Result<ImdbDetails>.CreateSuccess(imdbDetails);
                }
            }
            catch (Exception ex)
            {
                return Result<ImdbDetails>.CreateException(ex);
            }
        }

        private static Result<T> GetAnyErrors<T>(JObject jsonObj, string resultField)
        {
            if (jsonObj == null)
            {
                return Result<T>.CreateException(new NullReferenceException("jsonObj"));
            }

            if (!jsonObj.Value<bool>(resultField))
            {
                var strError = jsonObj.Value<string>(JSON_FIELD_ERROR);
                if (string.IsNullOrWhiteSpace(strError))
                {
                    return Result<T>.CreateError(Globals.Errors.SYSTEM_ERROR);
                }

                return Result<T>.CreateError(strError);
            }

            return null;
        }

        private static ImdbDetails CreateFromJson(JToken jsonObj)
        {
            if (jsonObj == null) return null;

            var res = new ImdbDetails();

            res.Id = jsonObj.Value<string>("Id");
            res.Title = jsonObj.Value<string>("Title");
            res.TitleDisplay = jsonObj.Value<string>("TitleDisplay");
            res.Year = jsonObj.Value<int?>("Year");
            res.YearTo = jsonObj.Value<int?>("YearTo");
            res.Season = jsonObj.Value<int?>("Season");
            res.Episode = jsonObj.Value<int?>("Episode");

            return res;
        }

        private const string JSON_FIELD_ERROR = "error";
        private const string JSON_FIELD_SESSION = "session";
        private const string JSON_FIELD_USERNAME = "username";
        private const string JSON_FIELD_PASSWORD = "password";
        private const string JSON_FIELD_VIDEO_DETAILS = "videoDetails";

        private static string GetAbsoluteUrl(string relativeUrl, NameValueCollection urlQuery = null)
        {
            StringBuilder sb = new StringBuilder();

            if (Globals.IsSandboxMode)
            {
                sb.AppendFormat("{0}/{1}", Globals.API_URL_SANDBOX, relativeUrl);
            }
            else
            {
                sb.AppendFormat("{0}/{1}", Globals.API_URL, relativeUrl);
            }

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
