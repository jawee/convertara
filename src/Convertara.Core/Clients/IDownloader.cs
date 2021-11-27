using Convertara.Core.DTO;

namespace Convertara.Core.Clients;

public interface IDownloader
{
    bool Download(VideoDTO video);
}