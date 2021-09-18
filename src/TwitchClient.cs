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
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("scope")]
    public List<string> Scope { get; set; }
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
  }

  public class VideoDTO 
  {
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("stream_id")]
    public string StreamId { get; set; }
    [JsonProperty("user_id")]
    public string UserId { get; set; }
    [JsonProperty("user_login")]
    public string UserLogin { get; set; }
    [JsonProperty("user_name")]
    public string UserName { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("thumbnail_url")]
    public string ThumbnailUrl { get; set; }
    [JsonProperty("viewable")]
    public string Viewable { get; set; }
    [JsonProperty("view_count")]
    public int ViewCount { get; set; }
    [JsonProperty("language")]
    public string Language { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("duration")]
    public string Duration { get; set; }
    [JsonProperty("muted_segments")]
    public List<MutedSegment> MutedSegments { get; set; } 

  }
  public class MutedSegment
  {
    [JsonProperty("duration")]
    public int Duration {get;set;}
    [JsonProperty("offset")]
    public int Offset {get;set;}
  }

  public class Pagination
  {
    [JsonProperty("cursor")]
    public string Cursor {get;set;}
  }

  public class GetVideosResponse
  {
    [JsonProperty("data")]
    public List<VideoDTO> Data {get;set;}
    [JsonProperty("pagination")]
    public Pagination Pagination {get;set;}
  }

  public class UserDTO 
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("broadcaster_type")]
    public string BroadcasterType { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("profile_image_url")]
    public string ProfileImageUrl { get; set; }

    [JsonProperty("offline_image_url")]
    public string OfflineImageUrl { get; set; }

    [JsonProperty("view_count")]
    public int ViewCount { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
  }

  public class GetUsersResponse
  {
    [JsonProperty("data")]
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
        var token = GetToken();
        if(token == null || token.Length == 0) {
          throw new Exception("No token ffs");
        }
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("Client-Id", client_id);
        var resp = client.GetAsync(url).Result;
        respText = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<GetVideosResponse>(respText);
        return respParsed.Data;
      }
    }

    private object MakeGetRequest(HttpClient client, string url) 
    {
      //TODO make generic method
      throw new NotImplementedException();
    }

    private string GetUserIDFromUsername(string username) 
    {
      var url = $"https://api.twitch.tv/helix/users?login={username}";
      var userId = "";
      using(var client = GetHttpClient())
      {
        var token = GetToken();
        if(token == null || token.Length == 0) {
          throw new Exception("No token ffs");
        }
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("Client-Id", client_id);
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
        _token = respParsed.AccessToken;
        _expiration = DateTime.Now.AddSeconds(respParsed.ExpiresIn);
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
