using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class FeedbackCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _chatStateService;

    public FeedbackCommandHandler(ITelegramBotClient botClient, IChatStateService chatStateService)
    {
        _botClient = botClient;
        _chatStateService = chatStateService;
    }

    public async Task ExecuteAsync(Message message)
    {
        _chatStateService.SetState(message.Chat.Id, ChatStates.AwaitingFeedback);
        const string response = "Пожалуйста, введите обратную связь.";
        await _botClient.SendTextMessageAsync(message.Chat.Id, response);
    }
}