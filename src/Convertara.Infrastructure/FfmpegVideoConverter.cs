using System.Diagnostics;
using Convertara.Core.Clients;

namespace Convertara.Core
{
    public class FfmpegVideoConverter : IVideoConverter
    {
        private static string ffmpegPath = "./src/lib/ffmpeg";

        public bool ConvertVideo(string inputPath, string outputPath)
        {
            if(File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            if(!File.Exists(ffmpegPath))
            {
                throw new FileNotFoundException("Cant find ffmpeg");
            }
            var argStr = $"-i {inputPath} -vcodec libx265 -crf 28 -r 30 {outputPath}";

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

