using System.Collections.Generic;
using Convertara.Core.DTO;

namespace Convertara.Core
{
    public interface ITwitchService
    {
        ICollection<VideoDTO> GetVideosForUsername(string username);
        string GetUserIdFromUsername(string username);
        string GetToken();
    }
}
