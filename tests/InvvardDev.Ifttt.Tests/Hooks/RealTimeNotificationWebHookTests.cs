using System.Net;
using FluentAssertions;
using InvvardDev.Ifttt.Hooks;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.TestFactories.Triggers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace InvvardDev.Ifttt.Tests.Hooks;

public class RealTimeNotificationWebHookTests
{
    [Fact(DisplayName = "SendNotification should return 200 Ok")]
    public async Task SendNotification_HandleRequest_ShouldReturnOk()
    {
        // Arrange
        var capturedRequest = new List<HttpRequestMessage>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected().As<IMockHttpClient>()
                              .Setup(x => x.SendAsync(Capture.In(capturedRequest),
                                                      It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new HttpResponseMessage
                                            {
                                                StatusCode = HttpStatusCode.OK
                                            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
                         {
                             BaseAddress = new Uri("http://unknown-domain.com/v1/triggers/")
                         };
        var httpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(It.IsAny<string>()) == httpClient);
        var payload = RealTimeNotificationModelFactory.Instance.CreateTriggerIdentities(10);
        var sut = new RealTimeNotificationWebHook(httpClientFactory, Mock.Of<ILogger<RealTimeNotificationWebHook>>());

        // Act
        var response = await sut.SendNotification(payload);

        // Assert
        response.Should().Be(HttpStatusCode.OK);
        capturedRequest.Should().ContainSingle().Which.Content.Should().NotBeNull();
        var actualRequest = capturedRequest[0].Content!;
        var json = await actualRequest.ReadAsStringAsync();
        var actualContent = TopLevelMessageModel<List<RealTimeNotificationModel>>.Deserialize(json);
        actualContent.Should().NotBeNull();
        actualContent.Data.Should().BeEquivalentTo(payload);
    }

    // Advised way in order to capture on a ProtectedSetup:
    // https://github.com/devlooped/moq/issues/934#issuecomment-716029030
    private interface IMockHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}