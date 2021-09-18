using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace dotnet_ffmpeg_console 
{

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
        var respParsed = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(respContent);

        userId = respParsed["data"][0]["id"];
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
        var respParsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(respContent);
        _token = respParsed["access_token"];
        _expiration = DateTime.Now.AddSeconds(int.Parse(respParsed["expires_in"]));
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
