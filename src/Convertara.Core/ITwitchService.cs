using System.Collections.Generic;
using System.Threading.Tasks;
using Convertara.Core.Dto;

namespace Convertara.Core
{
    public interface ITwitchService
    {
        Task<ICollection<VideoDto>> GetVideosForUsername(string username);
        Task<string> GetUserIdFromUsername(string username);
        Task<string> GetToken();
    }
}
