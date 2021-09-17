using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace dotnet_ffmpeg_console
{
  class Program
  {
    private static string ffmpegPath = "./lib/ffmpeg";
    private static string exampleFilePath = "./example-file/example.mkv";
    private static string outputDir = "./output/";
      
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      Console.WriteLine(ffmpegPath);
      File.Delete(outputDir + "output.mp4");
      var argStr = $"-i {exampleFilePath} -vcodec libx265 -crf 28 -r 30 {outputDir}output.mp4";

      var psi = new ProcessStartInfo(ffmpegPath)
      {
        Arguments = argStr
      };
      Process.Start(psi);
    }
  }
}
