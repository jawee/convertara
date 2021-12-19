using System.Threading.Tasks;
using Convertara.Core.Dto;

namespace Convertara.Core.Clients
{
    public interface ITwitchClient
    {
        Task<GetVideosResponse> GetVideosForUserId(string userId, string token);
        Task<GetUsersResponse> GetUserIdFromUsername(string username, string token);
        Task<GetAccessTokenResponse> GetToken(string clientId, string clientSecret);
    }
}
