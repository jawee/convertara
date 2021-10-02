using NUnit.Framework;
using Convertara.Core;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Convertara.Core.DTO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Convertara.Core.Clients;

namespace Convertara.Test
{
  public class TwitchServiceTest
  {
    private ITwitchService _twitchService;

    [SetUp]
    public void Setup()
    {
      var tokenResponse = GetAccessTokenResponse();
      var mockTwitchClient = new Mock<ITwitchClient>();
      mockTwitchClient.Setup(twitchClient => 
          twitchClient.GetToken(It.IsAny<string>(), It.IsAny<string>()))
          .Returns(() => GetAccessTokenResponse());
      mockTwitchClient.Setup(twitchClient => 
          twitchClient.GetUserIdFromUsername(It.IsAny<string>(), It.IsAny<string>()))
          .Returns(() => GetUsersResponse());
        
      _twitchService = new TwitchService(mockTwitchClient.Object);
    }

    [Test]
    public void TwitchService_GetToken_Returns_Token()
    {
      Assert.AreEqual(_twitchService.GetToken(), "asdfasdfa");
    }

    [Test]
    public void TwitchService_GetIdForUsername_Returns_123()
    {
      Assert.AreEqual(_twitchService.GetUserIdFromUsername("testusername"), "123");
    }

    private GetAccessTokenResponse GetAccessTokenResponse()
    {
      var tokenResponse = new GetAccessTokenResponse();
      tokenResponse.AccessToken = "asdfasdfa";
      tokenResponse.ExpiresIn = 300;
      tokenResponse.RefreshToken = "asdfasf";
      tokenResponse.Scope = new List<string>{"scope1", "scope2"};
      tokenResponse.TokenType = "client";
      return tokenResponse;
    }

    private GetUsersResponse GetUsersResponse()
    {
      var usersResponse = new GetUsersResponse();
      usersResponse.Data = new List<UserDTO>();
      var userDto = new UserDTO();
      userDto.Id = "123";
      usersResponse.Data.Add(userDto);
      return usersResponse;
    }
  }
}
