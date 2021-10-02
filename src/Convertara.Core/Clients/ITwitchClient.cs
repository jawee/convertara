using Convertara.Core.DTO;

namespace Convertara.Core.Clients
{
    public interface ITwitchClient
    {
        GetVideosResponse GetVideosForUserId(string userId, string token);
        GetUsersResponse GetUserIdFromUsername(string username, string token);
        GetAccessTokenResponse GetToken(string clientId, string clientSecret);
    }
}
