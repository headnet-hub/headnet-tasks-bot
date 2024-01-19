using HeadNet.Bot.Controllers.StateHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.StateHandlers;

public class FeedbackStateHandler : IStateHandler
{
    // ReSharper disable once CollectionNeverQueried.Local
    private readonly List<(long chatId, string text)> _feedbacks = new();
    
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;

    public FeedbackStateHandler(ITelegramBotClient botClient, IChatStateService stateService)
    {
        _botClient = botClient;
        _stateService = stateService;
    }

    public async Task HandleStateAsync(Message message)
    {
        _feedbacks.Add((message.Chat.Id, message.Text));
        
        _stateService.CleanState(message.Chat.Id);
        await _botClient.SendTextMessageAsync(message.Chat.Id, "Спасибо за Ваш фидбэк!");
    }
}