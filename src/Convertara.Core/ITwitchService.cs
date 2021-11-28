using System.Collections.Generic;
using System.Threading.Tasks;
using Convertara.Core.DTO;

namespace Convertara.Core
{
    public interface ITwitchService
    {
        Task<ICollection<VideoDTO>> GetVideosForUsername(string username);
        Task<string> GetUserIdFromUsername(string username);
        Task<string> GetToken();
    }
}
