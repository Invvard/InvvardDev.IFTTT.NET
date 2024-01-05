using System.Net;
using System.Net.Mime;
using System.Text;
using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using InvvardDev.Ifttt.Service.Api.Core.Models;
using InvvardDev.Ifttt.Service.Api.Trigger.Contracts;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Hooks;

public class RealTimeNotificationWebHook : ITriggerHook
{
    private readonly ILogger<RealTimeNotificationWebHook> logger;
    private readonly HttpClient httpClient;

    public RealTimeNotificationWebHook(IHttpClientFactory httpClientFactory,
                                       ILogger<RealTimeNotificationWebHook> logger)
    {
        this.logger = logger;
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        httpClient = httpClientFactory.CreateClient(IftttConstants.TriggerHttpClientName);
    }

    public async Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData)
    {
        var content = new StringContent(TopLevelMessageModel<List<RealTimeNotificationModel>>.Serialize(notificationData.ToList()),
                                        Encoding.UTF8,
                                        MediaTypeNames.Application.Json);
        var request = new HttpRequestMessage
                      {
                          Content = content,
                          Method = HttpMethod.Post
                      };
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Success! IFTTT response code is '{ResponseCode}'", response.StatusCode);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            logger.LogWarning("Failure... IFTTT response code is '{ResponseCode}' with message content '{ErrorMessage}'",
                              response.StatusCode,
                              errorMessage);
        }

        return response.StatusCode;
    }
}