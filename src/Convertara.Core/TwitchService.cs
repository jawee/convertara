using System;
using System.Collections.Generic;
using System.Linq;
using Convertara.Core.Clients;
using Convertara.Core.DTO;

namespace Convertara.Core
{
  public interface ITwitchService
  {
    ICollection<VideoDTO> GetVideosForUsername(string username);
    string GetUserIdFromUsername(string username);
    string GetToken();
  }

  public class TwitchService : ITwitchService
  {
    private string _client_id; 
    private string _client_secret;
    private string _token;
    private DateTime _expiration;
    private ITwitchClient _httpClient;

    public TwitchService(ITwitchClient twitchClient, string clientId, string clientSecret)
    {
      _client_id = clientId;
      _client_secret = clientSecret;
      _httpClient = twitchClient;
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
      var respParsed = _httpClient.GetToken(_client_id, _client_secret);
      Console.WriteLine($"Setting token to {respParsed.AccessToken}");
      _token = respParsed.AccessToken;
      _expiration = DateTime.Now.AddSeconds(respParsed.ExpiresIn);
      return _token;
    }
  }
}
