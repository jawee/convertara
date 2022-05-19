using System;
using System.Threading.Tasks;
using Convertara.Core;
using Convertara.Infrastructure.Clients;
using Microsoft.Extensions.Configuration;

namespace Convertara.ConsoleApp
{
    public class Program
    {
        private static string exampleFilePath = "./example/example.mkv";
        private static string outputPath = "./output/output.mp4";


        static async Task Main(string[] args)
        {
            if(args.Length == 0) {
                Console.WriteLine("No argument ffs. Setting to theprimeagen");
                args = new[] {"theprimeagen"};
            }

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // var convertVideoResult = new FfmpegVideoConverter().ConvertVideo(exampleFilePath, outputPath);
            var twitchClient = new TwitchClient();
            var twitchService = new TwitchService(twitchClient, config);
            var videos = await twitchService.GetVideosForUsername(args[0]);

            foreach(var video in videos) 
            {
                Console.WriteLine($"Title: '{video.Title}' Broadcasted: '{video.CreatedAt}'");
            }
        }
    }
}
