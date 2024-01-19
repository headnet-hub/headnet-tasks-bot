using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class RateDayCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;

    public RateDayCommandHandler(ITelegramBotClient botClient, IChatStateService stateService)
    {
        _botClient = botClient;
        _stateService = stateService;
    }

    public async Task ExecuteAsync(Message message)
    {
        var inlineKeyboard = BuildTopicsKeyboard();
        
        _stateService.SetState(message.Chat.Id, ChatStates.RatingDay);
        
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Как вы оцениваете свое настроение? Выберите от 1 до 5.",
            replyMarkup: inlineKeyboard
        );
    }

    private static InlineKeyboardMarkup BuildTopicsKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1", "rate_1"),
                InlineKeyboardButton.WithCallbackData("2", "rate_2"),
                InlineKeyboardButton.WithCallbackData("3", "rate_3"),
                InlineKeyboardButton.WithCallbackData("4", "rate_4"),
                InlineKeyboardButton.WithCallbackData("5", "rate_5")
            }
        });
    }
}