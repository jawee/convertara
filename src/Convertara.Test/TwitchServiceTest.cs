using NUnit.Framework;
using Convertara.Core;
using Moq;
using Convertara.Core.Dto;
using System.Collections.Generic;
using Convertara.Core.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            mockTwitchClient.Setup(twitchClient => 
                    twitchClient.GetVideosForUserId(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => GetVideosResponse());

            _twitchService = new TwitchService(mockTwitchClient.Object, "clientid", "clientsecret");
        }

        [Test]
        public async Task TwitchService_GetToken_Returns_Token()
        {
            var token = await _twitchService.GetToken();
            Assert.AreEqual("asdfasdfa", token);
        }

        [Test]
        public async Task TwitchService_GetIdForUsername_Returns_123()
        {
            var userId = await _twitchService.GetUserIdFromUsername("testusername");
            Assert.AreEqual("123", userId);
        }

        [Test]
        public async Task TwitchService_GetVideosForUsername_Returns_Something()
        {
            var res = await _twitchService.GetVideosForUsername("testusername");
            Assert.AreEqual("testusername", res.First().UserName);
        }

        private static Task<GetVideosResponse> GetVideosResponse()
        {
            var videosResponse = new GetVideosResponse();
            videosResponse.Data = new List<VideoDto>();
            var videoDto = new VideoDto();
            videoDto.Id = "1128218457";
            videoDto.StreamId = "43405788957";
            videoDto.UserId = "167160215";
            videoDto.UserLogin = "testusername";
            videoDto.UserName = "testusername";
            videoDto.Title = "SomeTitle";
            videoDto.Description = "";
            videoDto.CreatedAt = new DateTime(2021, 8, 24, 18,15,40);
            videoDto.PublishedAt = new DateTime(2021, 8, 24, 18,15,40);
            videoDto.Url = "https://www.twitch.tv/videos/1128218457";
            videoDto.ThumbnailUrl = "https://static-cdn.jtvnw.net/cf_vods/d1m7jfoe9zdc1j/095dd28aa5024e2c850e_testusername_43405788957_1629828931//thumb/thumb0-%{width}x%{height}.jpg";
            videoDto.Viewable = "public";
            videoDto.ViewCount = 2801;
            videoDto.Language = "en";
            videoDto.Type = "archive";
            videoDto.Duration = "1h45m44s";
            videoDto.MutedSegments = null;
            videosResponse.Data.Add(videoDto);
            return Task.FromResult(videosResponse);
        }

        private static Task<GetAccessTokenResponse> GetAccessTokenResponse()
        {
            var tokenResponse = new GetAccessTokenResponse();
            tokenResponse.AccessToken = "asdfasdfa";
            tokenResponse.ExpiresIn = 300;
            tokenResponse.RefreshToken = "asdfasf";
            tokenResponse.Scope = new List<string>{"scope1", "scope2"};
            tokenResponse.TokenType = "client";
            return Task.FromResult(tokenResponse);
        }

        private static Task<GetUsersResponse> GetUsersResponse()
        {
            var usersResponse = new GetUsersResponse();
            usersResponse.Data = new List<UserDto>();
            var userDto = new UserDto();
            userDto.Id = "123";
            usersResponse.Data.Add(userDto);
            return Task.FromResult(usersResponse);
        }
    }
}
