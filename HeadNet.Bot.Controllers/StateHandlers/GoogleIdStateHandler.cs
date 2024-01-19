using System.Text.RegularExpressions;
using HeadNet.Bot.Controllers.StateHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.StateHandlers;

public class GoogleIdStateHandler : IStateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IGoogleIntegrationService _googleService;
    private readonly IChatStateService _stateService;

    public GoogleIdStateHandler(ITelegramBotClient botClient, IGoogleIntegrationService googleService, IChatStateService stateService)
    {
        _botClient = botClient;
        _googleService = googleService;
        _stateService = stateService;
    }

    public async Task HandleStateAsync(Message message)
    {
        const string apiKeyPattern = "^[A-Za-z0-9_-]{35,45}$";
        
        var googleId = message.Text;

        if (googleId == null || Regex.IsMatch(googleId, apiKeyPattern) == false)
        {
            await _botClient.SendTextMessageAsync(message.Chat.Id, "Ключ не соответствует формату Google API Key. Попробуйте еще раз.");
            return;
        }

        await _botClient.SendTextMessageAsync(message.Chat.Id, "Google API Key успешно добавлен!");
        await _googleService.Connect(googleId);
        _stateService.CleanState(message.Chat.Id);
    }
}