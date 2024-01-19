using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class ConnectGoogleCommandHandler: ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _chatStateService;

    public ConnectGoogleCommandHandler(ITelegramBotClient botClient, IChatStateService chatStateService)
    {
        _botClient = botClient;
        _chatStateService = chatStateService;
    }

    public async Task ExecuteAsync(Message message)
    {
        const string response = "Пожалуйста, введите ваш Google API Key. " + 
                                "Убедитесь, что он имеет права на Google Calendar и Google Tasks.";

        _chatStateService.SetState(message.Chat.Id, ChatStates.AwaitingGoogleId);
        await _botClient.SendTextMessageAsync(message.Chat.Id, response);
    }
}