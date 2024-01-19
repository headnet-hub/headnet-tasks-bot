namespace HeadNet.Bot.Services.Abstractions;

public interface IGoogleIntegrationService
{
    Task Connect(string googleApiKey);
}