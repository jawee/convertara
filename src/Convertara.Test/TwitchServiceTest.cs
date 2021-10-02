using NUnit.Framework;
using Convertara.Core;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Convertara.Test
{
    public class TwitchServiceTest
    {
      private ITwitchService _twitchService;
        [SetUp]
        public void Setup()
        {
          var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
          handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
              ).ReturnsAsync(new HttpResponseMessage() 
                {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent("{'access_token': 'asdfasdfa'}"),
                  }).Verifiable();

          var httpClient = new HttpClient(handlerMock.Object);
          _twitchService = new TwitchService(httpClient);
        }

        [Test]
        public void TwitchService_GetToken_Returns_Token()
        {
            Assert.AreEqual(_twitchService.GetToken(), "asdfasdfa");
        }
    }
}
