
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
        //twitchClient.GetUserIDFromUsername("jawee15");
    }
  }
}
