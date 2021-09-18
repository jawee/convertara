using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace dotnet_ffmpeg_console 
{

  public class GetAccessTokenResponse
  {
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public int expires_in { get; set; }
    public List<string> scope { get; set; }
    public string token_type { get; set; }
  }

  public class VideoDTO 
  {
    public string id { get; set; }
    public string stream_id { get; set; }
    public string user_id { get; set; }
    public string user_login { get; set; }
    public string user_name { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime created_at { get; set; }
    public DateTime published_at { get; set; }
    public string url { get; set; }
    public string thumbnail_url { get; set; }
    public string viewable { get; set; }
    public int view_count { get; set; }
    public string language { get; set; }
    public string type { get; set; }
    public string duration { get; set; }
    public List<MutedSegment> muted_segments { get; set; } 

  }
  public class MutedSegment
  {
    public int duration {get;set;}
    public int offset {get;set;}
  }

  public class Pagination
  {
    public string cursor {get;set;}
  }

  public class GetVideosResponse
  {
    public List<VideoDTO> data {get;set;}
    public Pagination pagination {get;set;}
  }

  public class UserDTO 
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("login")]
    public string Login { get; set; }

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("broadcaster_type")]
    public string BroadcasterType { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; }

    [JsonPropertyName("offline_image_url")]
    public string OfflineImageUrl { get; set; }

    [JsonPropertyName("view_count")]
    public int ViewCount { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
  }

  public class GetUsersResponse
  {
    [JsonPropertyName("data")]
    public List<UserDTO> Data {get;set;}
  }

  public class TwitchClient
  {
    private string client_id; 
    private string client_secret;

    private string _token;
    private DateTime _expiration;

    public TwitchClient()
    {
      var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      client_id = config["twitch_client_id"];
      client_secret = config["twitch_client_secret"];
    }

    public ICollection<VideoDTO> GetVideosForUsername(string username) 
    {
      var userId = GetUserIDFromUsername(username);
      var url = $"https://api.twitch.tv/helix/videos?user_id={userId}";
      var respText = "";
      using(var client = GetHttpClient())
      {
        AddAuthHeadersToClient(client);
        var resp = client.GetAsync(url).Result;
        respText = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<GetVideosResponse>(respText);
        return respParsed.data;
      }
    }

    private void AddAuthHeadersToClient(HttpClient client) 
    {
      var token = GetToken();
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
      client.DefaultRequestHeaders.Add("Client-Id", client_id);
    }

    private string GetUserIDFromUsername(string username) 
    {
      var url = $"https://api.twitch.tv/helix/users?login={username}";
      var userId = "";
      using(var client = GetHttpClient())
      {
        AddAuthHeadersToClient(client);
        var resp = client.GetAsync(url).Result;
        var respContent = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<GetUsersResponse>(respContent);

        userId = respParsed.Data.First().Id;
      }
      return userId;
    }

    private string GetToken() 
    {
      if(_token != null && _expiration > DateTime.Now.AddMinutes(1))
      {
        return _token;
      }

      using(var client = GetHttpClient())
      {
        var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={client_id}&client_secret={client_secret}&grant_type=client_credentials";
        var resp = client.PostAsync(authUrl, new StringContent("", Encoding.UTF8, "application/json")).Result;
        var respContent = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<GetAccessTokenResponse>(respContent);
        _token = respParsed.access_token;
        _expiration = DateTime.Now.AddSeconds(respParsed.expires_in);
        return _token;
      }
    }

    private HttpClient GetHttpClient()
    {
      var hc = new HttpClient();
      return hc;
    }
  }
}
