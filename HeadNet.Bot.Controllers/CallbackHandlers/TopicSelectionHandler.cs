using HeadNet.Bot.Controllers.CallbackHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CallbackHandlers;

public class TopicSelectionHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly ITopicService _topicService;

    public TopicSelectionHandler(ITelegramBotClient botClient, IChatStateService stateService, ITopicService topicService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _topicService = topicService;
    }

    public async Task HandleCallbackAsync(CallbackQuery query)
    {
        var userId = query.From.Id;
        var selectedTopics = _topicService.GetTopics(userId);

        if (query.Data!.StartsWith("category_"))
        {
            var topic = query.Data[9..];
            
            if (selectedTopics.Contains(topic))
                _topicService.RemoveTopic(userId, topic);
            else
                _topicService.AddTopic(userId,topic);
        }
        else if (query.Data == "finish_selection")
        {
            _stateService.CleanState(userId);
            await _botClient.SendTextMessageAsync(
                chatId: userId,
                text: "Ваш выбор сохранен."
            );
            
            return;
        }
        
        var updatedKeyboard = _topicService.BuildTopicsKeyboard(userId);
        await _botClient.EditMessageReplyMarkupAsync(
            chatId: userId,
            messageId: query.Message!.MessageId,
            replyMarkup: updatedKeyboard
        );
    }
}