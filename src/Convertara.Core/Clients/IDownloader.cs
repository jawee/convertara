using System.Threading.Tasks;
using Convertara.Core.Dto;

namespace Convertara.Core.Clients;

public interface IDownloader
{
    Task<bool> Download(VideoDto video);
}