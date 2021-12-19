using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convertara.Core.Clients;
using Convertara.Core.Dto;

namespace Convertara.Core
{
    public class TwitchService : ITwitchService
    {
        private readonly string _client_id; 
        private readonly string _client_secret;
        private string _token;
        private DateTime _expiration;
        private readonly ITwitchClient _httpClient;

        public TwitchService(ITwitchClient twitchClient, string clientId, string clientSecret)
        {
            _client_id = clientId;
            _client_secret = clientSecret;
            _httpClient = twitchClient;
        }

        public async Task<ICollection<VideoDto>> GetVideosForUsername(string username) 
        {
            var userId = await GetUserIdFromUsername(username);
            var token = await GetToken();
            var respParsed = await _httpClient.GetVideosForUserId(userId, token);
            return respParsed.Data;
        }

        public async Task<string> GetUserIdFromUsername(string username) 
        {
            var token = await GetToken();
            var respParsed = await _httpClient.GetUserIdFromUsername(username, token);
            var userId = respParsed.Data.First().Id;
            return userId;
        }

        public async Task<string> GetToken() 
        {
            if(_token != null && _expiration > DateTime.Now.AddMinutes(1))
            {
                return _token;
            }
            var respParsed = await _httpClient.GetToken(_client_id, _client_secret);
            _token = respParsed.AccessToken;
            _expiration = DateTime.Now.AddSeconds(respParsed.ExpiresIn);
            return _token;
        }
    }
}
