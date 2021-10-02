using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Convertara.Core.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Convertara.Core
{
  public interface ITwitchService
  {
    ICollection<VideoDTO> GetVideosForUsername(string username);
    string GetUserIDFromUsername(string username);
    string GetToken();
  }

  public class TwitchService : ITwitchService
  {
    private string _client_id; 
    private string _client_secret;
    private string _token;
    private DateTime _expiration;
    private HttpClient _httpClient;

    public TwitchService()
    {
      var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      _client_id = config["twitch_client_id"];
      _client_secret = config["twitch_client_secret"];
      _httpClient = new HttpClient();
    }

    //Constructor for testing
    public TwitchService(HttpClient httpClient)
    {
      _client_id = "123424234";
      _client_secret = "1234313132";
      _httpClient = httpClient;
    }

    public ICollection<VideoDTO> GetVideosForUsername(string username) 
    {
      var userId = GetUserIDFromUsername(username);
      var url = $"https://api.twitch.tv/helix/videos?user_id={userId}";
        var respParsed = MakeGetRequest<GetVideosResponse>(url);
        return respParsed.Data;
    }

    private T MakePostRequest<T>(string url, string data, bool authenticated = true) where T : ITwitchResponse
    {
      if(authenticated)
      {
        var token = GetToken();
        if(token == null || token.Length == 0) {
          throw new Exception("No token ffs");
        }
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _httpClient.DefaultRequestHeaders.Add("Client-Id", _client_id);
      }
      var resp = _httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
      var respContent = resp.Content.ReadAsStringAsync().Result;
      var respParsed = JsonConvert.DeserializeObject<T>(respContent);
      return respParsed;
    }

    private T MakeGetRequest<T>(string url, bool authenticated = true)  where T : ITwitchResponse
    {
      var token = GetToken();
      if(token == null || token.Length == 0) {
        throw new Exception("No token ffs");
      }
      _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
      _httpClient.DefaultRequestHeaders.Add("Client-Id", _client_id);
      var resp = _httpClient.GetAsync(url).Result;
      var respParsed = JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
      return respParsed;
    }

    public string GetUserIDFromUsername(string username) 
    {
      var url = $"https://api.twitch.tv/helix/users?login={username}";
      var userId = "";
        var respParsed = MakeGetRequest<GetUsersResponse>(url);

        userId = respParsed.Data.First().Id;
      return userId;
    }

    public string GetToken() 
    {
      if(_token != null && _expiration > DateTime.Now.AddMinutes(1))
      {
        return _token;
      }
        var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={_client_id}&client_secret={_client_secret}&grant_type=client_credentials";
        var respParsed = MakePostRequest<GetAccessTokenResponse>(authUrl, "", false);
        _token = respParsed.AccessToken;
        _expiration = DateTime.Now.AddSeconds(respParsed.ExpiresIn);
        return _token;
    }
  }
}
