using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace dotnet_ffmpeg_console
{
  class Program
  {
    private static string ffmpegPath = "./lib/ffmpeg";
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      Console.WriteLine(ffmpegPath);
      var list = new List<string>();
      var psi = new ProcessStartInfo(ffmpegPath);
      Process.Start(psi);
    }
  }
}
