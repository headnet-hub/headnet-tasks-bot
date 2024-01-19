using System.Diagnostics;
using HeadNet.Bot.Controllers.CallbackHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CallbackHandlers;

public class GetAdviceHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly ITopicService _topicService;

    public GetAdviceHandler(ITelegramBotClient botClient, IChatStateService stateService, ITopicService topicService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _topicService = topicService;
    }

    public async Task HandleCallbackAsync(CallbackQuery query)
    {
        var userId = query.From.Id;

        if (query.Data!.StartsWith("add_"))
        {
            var adviceId = int.Parse(query.Data.Split("_")[1]);
            await _topicService.AddAdviceToUser(userId, adviceId);
            _stateService.CleanState(userId);
            await _botClient.SendTextMessageAsync(userId, text: "Задача успешно добавлена в Google Tasks.");
            return;
        }
        
        if (query.Data.StartsWith("decline_"))
        {
            _stateService.CleanState(userId);
            await _botClient.SendTextMessageAsync(userId, text: "Задача отклонена.");
            return;
        }

        throw new UnreachableException();
    }
}