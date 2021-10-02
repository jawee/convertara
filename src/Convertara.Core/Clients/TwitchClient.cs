using System;
using System.Net.Http;
using System.Text;
using Convertara.Core.DTO;
using Newtonsoft.Json;

namespace Convertara.Core.Clients
{
    public class TwitchClient : ITwitchClient
    {
        private string _clientId;

        public TwitchClient()
        {
        }
        public GetVideosResponse GetVideosForUserId(string userId, string token)
        {
            var url = $"https://api.twitch.tv/helix/videos?user_id={userId}";
            var respParsed = MakeGetRequest<GetVideosResponse>(url, token);
            return respParsed;
        }

        public GetUsersResponse GetUserIdFromUsername(string username, string token)
        {
            var url = $"https://api.twitch.tv/helix/users?login={username}";
            var respParsed = MakeGetRequest<GetUsersResponse>(url, token);
            return respParsed;
        }

        public GetAccessTokenResponse GetToken(string clientId, string clientSecret)
        {
            //TODO 1: This is ugly. Should require clientId to be passed with each request.
            _clientId = clientId; 
            var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials";
            var respParsed = MakePostRequest<GetAccessTokenResponse>(authUrl, "", null, false);
            return respParsed;
        }
        private T MakePostRequest<T>(string url, string data, string token = null, bool authenticated = true) where T : ITwitchResponse
        {
            using(var httpClient = new HttpClient())
            {
                if(authenticated)
                {
                    if(token == null || token.Length == 0) {
                    throw new Exception("No token ffs in MakePostRequest");
                    }
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    httpClient.DefaultRequestHeaders.Add("Client-Id", _clientId);
                }
                var resp = httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
                var respContent = resp.Content.ReadAsStringAsync().Result;
                var respParsed = JsonConvert.DeserializeObject<T>(respContent);
                return respParsed;
            }
        
        }

        private T MakeGetRequest<T>(string url, string token = null, bool authenticated = true)  where T : ITwitchResponse
        {
            using(var httpClient = new HttpClient())
            {
                if(token == null || token.Length == 0) {
                    throw new Exception("No token ffs in MakeGetRequest");
                }
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                httpClient.DefaultRequestHeaders.Add("Client-Id", _clientId);
                var resp = httpClient.GetAsync(url).Result;
                var respParsed = JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
                return respParsed;
            }
        }
    }
}