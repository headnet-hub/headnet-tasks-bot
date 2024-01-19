using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class SetTopicsCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _chatStateService;
    private readonly ITopicService _topicService;

    public SetTopicsCommandHandler(ITelegramBotClient botClient, IChatStateService chatStateService, ITopicService topicService)
    {
        _botClient = botClient;
        _chatStateService = chatStateService;
        _topicService = topicService;
    }

    public async Task ExecuteAsync(Message message)
    {
        var inlineKeyboard = BuildTopicsKeyboard(message.Chat.Id);

        _chatStateService.SetState(message.Chat.Id, ChatStates.TopicSelection);
        
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите интересующие вас категории:",
            replyMarkup: inlineKeyboard
        );
    }
    
    private InlineKeyboardMarkup BuildTopicsKeyboard(long chatId)
    {
        return _topicService.BuildTopicsKeyboard(chatId);
    }
}