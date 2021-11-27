namespace Convertara.Core.Clients;

public interface IVideoConverter
{
    bool ConvertVideo(string inputPath, string outputPath);
}