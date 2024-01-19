using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CallbackHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Controllers.CallbackHandlers;

public class RateDayHandler : ICallbackHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly IMoodService _moodService;

    public RateDayHandler(ITelegramBotClient botClient, IChatStateService stateService, IMoodService moodService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _moodService = moodService;
    }

    public async Task HandleCallbackAsync(CallbackQuery query)
    {
        var chatState = _stateService.GetState(query.From.Id);
        
        if (chatState is ChatStates.RatingDay)
        {
            var rating = int.Parse(query.Data!.Split("_")[1]);
            await AskForDate(query.From.Id, rating);
        }
        else if (chatState is ChatStates.ChoosingRateDay)
        {
            var rating = short.Parse(query.Data!.Split("_")[1]);
            var date = DateTime.Parse(query.Data!.Split("_")[3]);

            _moodService.SaveUserRate(query.From.Id, rating, date);
            _stateService.CleanState(query.From.Id);

            await _botClient.SendTextMessageAsync(query.From.Id, "Ваша оценка сохранена!");
        }
    }
    
    private async Task AskForDate(long chatId, int rating)
    {
        var inlineKeyboard = BuildDateSelectionKeyboard(rating);
        
        _stateService.SetState(chatId, ChatStates.ChoosingRateDay);

        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите день",
            replyMarkup: inlineKeyboard
        );
    }
    
    private static InlineKeyboardMarkup BuildDateSelectionKeyboard(int rating)
    {
        var today = DateTime.UtcNow;
        var buttons = new List<InlineKeyboardButton[]>();
        
        var previousDaysButtons = Enumerable.Range(1, 6).Reverse()
            .Select(i => DateTime.UtcNow.AddDays(-i))
            .Select(date => InlineKeyboardButton.WithCallbackData(date.ToString("ddd"), $"rate_{rating}_date_{date}"))
            .ToArray();
        
        buttons.Add(previousDaysButtons);
        
        var todayButton = InlineKeyboardButton.WithCallbackData(today.ToString("Сегодня"), $"rate_{rating}_date_{today}");
        buttons.Add(new[] { todayButton });

        return new InlineKeyboardMarkup(buttons);
    }
}