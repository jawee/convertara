using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace dotnet_ffmpeg_console 
{
  public class TwitchClient
  {
    private string client_id; 
    private string client_secret;

    public TwitchClient()
    {
      var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      client_id = config["twitch_client_id"];
      client_secret = config["twitch_client_secret"];
    }

    public string GetVideosForUsername(string username) 
    {
      var userId = GetUserIDFromUsername(username);
      var url = $"https://api.twitch.tv/helix/videos?user_id={userId}";
      var token = GetToken();
      var respText = "";
      using(var client = GetPublicApiClient())
      {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("Client-Id", client_id);
        var resp = client.GetAsync(url).Result;
        respText = resp.Content.ReadAsStringAsync().Result;
      }
      return respText;
    }


    private string GetUserIDFromUsername(string username) 
    {

      var token = GetToken();
      var url = $"https://api.twitch.tv/helix/users?login={username}";
      var userId = "";
      using(var client = GetPublicApiClient())
      {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("Client-Id", client_id);
        var resp = client.GetAsync(url).Result;
        var respContent = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(respContent);

        userId = respParsed["data"][0]["id"];
      }
      return userId;
    }

    private string GetToken() 
    {
      using(var client = GetPublicApiClient())
      {
        var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={client_id}&client_secret={client_secret}&grant_type=client_credentials";
        var resp = client.PostAsync(authUrl, new StringContent("", Encoding.UTF8, "application/json")).Result;
        var respContent = resp.Content.ReadAsStringAsync().Result;
        var respParsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(respContent);

        return respParsed["access_token"];
      }
    }

    private HttpClient GetPublicApiClient()
    {

      var hc = new HttpClient();
      return hc;
    }
  }
}
