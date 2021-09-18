
using System;
using Newtonsoft.Json;

namespace dotnet_ffmpeg_console
{
  public class Program
  {
    private static string exampleFilePath = "./../example/example.mkv";
    private static string outputPath = "./../output/output.mp4";


    static void Main(string[] args)
    {
//      var videoConverter = new VideoConverter(exampleFilePath, outputPath);
//      videoConverter.ConvertVideo();
        var twitchClient = new TwitchClient();
        var videos = twitchClient.GetVideosForUsername("theprimeagen");
        var parsedJson = JsonConvert.DeserializeObject(videos);

        Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));
    }
  }
}
