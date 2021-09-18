using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

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
      Console.WriteLine($"Client_id: {client_id} Client_secret: {client_secret}");
    }

    public string GetUserIDFromUsername(string username) {

      Console.WriteLine(GetToken());
      return "";
    }

    private string GetToken() 
    {
      using(var client = GetPublicApiClient())
      {
        var authUrl = $"https://id.twitch.tv/oauth2/token?client_id={client_id}&client_secret={client_secret}&grant_type=client_credentials";
        var resp = client.PostAsync(authUrl, new StringContent("", Encoding.UTF8, "application/json")).Result;
        return resp.Content.ReadAsStringAsync().Result;
      }
    }
    
    private HttpClient GetPublicApiClient()
    {

      var hc = new HttpClient();
      return hc;
    }
  }
}
