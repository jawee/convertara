using System;
using Convertara.Core;
using Convertara.Core.Clients;
using Microsoft.Extensions.Configuration;

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

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var clientId = config["twitch_client_id"];
            var clientSecret = config["twitch_client_secret"];

            var videoConverter = new VideoConverter(exampleFilePath, outputPath);
            videoConverter.ConvertVideo();
            var twitchClient = new TwitchClient();
            var twitchService = new TwitchService(twitchClient, clientId, clientSecret);
            var videos = twitchService.GetVideosForUsername(args[0]);

            foreach(var video in videos) 
            {
                Console.WriteLine($"Title: '{video.Title}' Broadcasted: '{video.CreatedAt}'");
            }
        }
    }
}
