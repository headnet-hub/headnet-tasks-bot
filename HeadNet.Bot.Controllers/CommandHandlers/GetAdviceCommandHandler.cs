using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class GetAdviceCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly ITopicService _topicService;

    public GetAdviceCommandHandler(ITelegramBotClient botClient, IChatStateService stateService, ITopicService topicService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _topicService = topicService;
    }

    public async Task ExecuteAsync(Message message)
    {
        var advice = await _topicService.GetAdvice(message.Chat.Id);

        if (advice == null)
        {
            await _botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "На текущий момент для Вас нет советов.");
            return;
        }
        
        _stateService.SetState(message.Chat.Id, ChatStates.GettingAdvice);

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Добавить в задачи", $"add_{advice.Id}"),
            InlineKeyboardButton.WithCallbackData("Отклонить", $"decline_{advice.Id}")
        });

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Мы подобрали для вас новую задачу:\n'{advice.Name}'",
            replyMarkup: inlineKeyboard
        );
    }
}