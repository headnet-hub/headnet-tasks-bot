using System.Text;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class ViewDayCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMoodService _moodService;
    private readonly ITaskService _taskService;

    public ViewDayCommandHandler(ITelegramBotClient botClient, IMoodService moodService, ITaskService taskService)
    {
        _botClient = botClient;
        _moodService = moodService;
        _taskService = taskService;
    }

    public async Task ExecuteAsync(Message message)
    {
        var chatId = message.Chat.Id;
        var mood = _moodService.GetMoodForToday(chatId);
        var moodText = mood != 0 ? $"*Оценка настроения:* {mood}" : "*Вы не оценили свое настроение сегодня*";

        var tasks = _taskService.GetUserTasksMock(chatId); 
        var events = _taskService.GetUserEventsMock(chatId);

        var response = new StringBuilder();
        response.AppendLine(moodText);
        response.AppendLine("\n*ЗАДАЧИ (невыполненные):*");
        foreach (var task in tasks)
        {
            response.AppendLine($"📝 {task}");
        }

        response.AppendLine("\n*СОБЫТИЯ (оставшиеся):*");
        foreach (var ev in events)
        {
            response.AppendLine($"🗓️ {ev}");
        }

        await _botClient.SendTextMessageAsync(chatId, response.ToString(), parseMode: ParseMode.Markdown);
    }
}