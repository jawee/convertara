using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Convertara.Core.Clients;
using Convertara.Core.DTO;
using Convertara.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace Convertara.Core
{
    public class TwitchService : ITwitchService
    {
        private readonly string _clientId; 
        private readonly string _clientSecret;
        private string _token;
        private DateTime _expiration;
        private readonly ITwitchClient _httpClient;

        public TwitchService(ITwitchClient twitchClient, IConfiguration configuration)
        {
           Guard.IsNullCheck(twitchClient, nameof(twitchClient)); 
           Guard.IsNullCheck(configuration, nameof(configuration));
            _httpClient = twitchClient;
            _clientId = configuration["twitch_client_id"];
            _clientSecret = configuration["twitch_client_secret"];
        }

        public ICollection<VideoDTO> GetVideosForUsername(string username) 
        {
            var userId = GetUserIdFromUsername(username);
            var token = GetToken();
            var respParsed = _httpClient.GetVideosForUserId(userId, token);
            return respParsed.Data;
        }

        public string GetUserIdFromUsername(string username) 
        {
            var token = GetToken();
            var respParsed = _httpClient.GetUserIdFromUsername(username, token);
            var userId = respParsed.Data.First().Id;
            return userId;
        }

        public string GetToken() 
        {
            if(_token != null && _expiration > DateTime.Now.AddMinutes(1))
            {
                return _token;
            }
            var respParsed = _httpClient.GetToken(_clientId, _clientSecret);
            _token = respParsed.AccessToken;
            _expiration = DateTime.Now.AddSeconds(respParsed.ExpiresIn);
            return _token;
        }
    }
}
