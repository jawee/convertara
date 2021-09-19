using System;
using System.Diagnostics;
using System.IO;

namespace Convertara.Core
{
  public class VideoConverter
  {
    private static string ffmpegPath = "./src/lib/ffmpeg";
    private string _inputPath;
    private string _outputPath;
    public VideoConverter(string inputPath, string outputPath) 
    {
      _inputPath = inputPath;
      _outputPath = outputPath;
    }

    public bool ConvertVideo()
    {
      if(File.Exists(_outputPath))
      {
        File.Delete(_outputPath);
      }

      if(!File.Exists(ffmpegPath))
      {
        throw new Exception("Cant find ffmpeg");
      }
      var argStr = $"-i {_inputPath} -vcodec libx265 -crf 28 -r 30 {_outputPath}";

      var psi = new ProcessStartInfo(ffmpegPath)
      {
        Arguments = argStr
      };

      using (var process = new Process())
      {
        var stopwatch = Stopwatch.StartNew();
        process.StartInfo = psi;
        process.Start();
        process.WaitForExit();
        stopwatch.Stop();

        var res = false;
        if(process.ExitCode == 0)
        {
          var elapsedTime = stopwatch.Elapsed;
          res = true;
          Console.WriteLine($"Video conversion complete {elapsedTime.ToString(@"hh\:mm\:ss\.fff")}");
        } 
        else 
        {
          Console.WriteLine("Error");
        }
        return res;
      }
    }
  }
}

