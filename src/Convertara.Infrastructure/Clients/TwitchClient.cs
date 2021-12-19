using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using Convertara.Core.Clients;
using Convertara.Core.Dto;
using Newtonsoft.Json;

namespace Convertara.Infrastructure.Clients
{
    public class TwitchClient : ITwitchClient
    {
        private string _clientId;

        public TwitchClient()
        {
        }
        public async Task<GetVideosResponse> GetVideosForUserId(string userId, string token)
        {
            var url = $"https://api.twitch.tv/helix/videos?user_id={userId}";
            var respParsed = await MakeGetRequest<GetVideosResponse>(url, token);
            return respParsed;
        }

        public async Task<GetUsersResponse> GetUserIdFromUsername(string username, string token)
        {
            var url = $"https://api.twitch.tv/helix/users?login={username}";
            var respParsed = await MakeGetRequest<GetUsersResponse>(url, token);
            return respParsed;
        }

        public async Task<GetAccessTokenResponse> GetToken(string clientId, string clientSecret)
        {
            _clientId = clientId; 
            var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials";
            var respParsed = await MakePostRequest<GetAccessTokenResponse>(authUrl, "", null, false);
            return respParsed;
        }
        private async Task<T> MakePostRequest<T>(string url, string data, string token = null, bool authenticated = true) where T : ITwitchResponse
        {
            using(var httpClient = new HttpClient())
            {
                if(authenticated)
                {
                    if(token == null || token.Length == 0) {
                        throw new InvalidCredentialException("No token ffs in MakePostRequest");
                    }
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    httpClient.DefaultRequestHeaders.Add("Client-Id", _clientId);
                }
                var resp = await httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                var respContent = resp.Content.ReadAsStringAsync().Result;
                var respParsed = JsonConvert.DeserializeObject<T>(respContent);
                return respParsed;
            }

        }

        private async Task<T> MakeGetRequest<T>(string url, string token = null, bool authenticated = true)  where T : ITwitchResponse
        {
            using(var httpClient = new HttpClient())
            {
                if(authenticated) {
                    if(token == null || token.Length == 0) {
                        throw new InvalidCredentialException("No token ffs in MakePostRequest");
                    }
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    httpClient.DefaultRequestHeaders.Add("Client-Id", _clientId);
                }

                var resp = await httpClient.GetAsync(url);
                var respParsed = JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
                return respParsed;
            }
        }
    }
}
