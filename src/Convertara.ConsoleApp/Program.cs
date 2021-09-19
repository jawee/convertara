
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Convertara.ConsoleApp
{
  public class Program
  {
    private static string exampleFilePath = "./example/example.mkv";
    private static string outputPath = "./output/output.mp4";


    static void Main(string[] args)
    {
      if(args.Length == 0) {
        Console.WriteLine("No argument ffs");
        return;
      }
      var videoConverter = new VideoConverter(exampleFilePath, outputPath);
      videoConverter.ConvertVideo();
      var twitchClient = new TwitchClient();
      var videos = twitchClient.GetVideosForUsername(args[0]);

      foreach(var video in videos) 
      {
        Console.WriteLine($"Title: '{video.Title}' Broadcasted: '{video.CreatedAt}'");
      }
    }
  }
}
