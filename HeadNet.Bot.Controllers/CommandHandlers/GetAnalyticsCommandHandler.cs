using System.Text;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class GetAnalyticsCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly ITopicService _topicService;
    private readonly ITaskService _taskService;
    private readonly IMoodService _moodService;

    public GetAnalyticsCommandHandler(ITelegramBotClient botClient, IChatStateService stateService, ITopicService topicService, ITaskService taskService, IMoodService moodService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _topicService = topicService;
        _taskService = taskService;
        _moodService = moodService;
    }

    public async Task ExecuteAsync(Message message)
    {
        var chatId = message.Chat.Id;
        var averageMood = _moodService.GetAverageMoodForWeek(chatId);
        var taskCategories = _taskService.GetUserCategoriesMock(chatId);

        var response = new StringBuilder();
        response.AppendLine(averageMood == 0
            ? "*Вы не оценили свое настроение на этой неделе*"
            : $"*Среднее настроение за неделю:* {averageMood}");

        response.AppendLine("\n*КАТЕГОРИИ ЗАДАЧ:*");
        foreach (var category in taskCategories)
        {
            response.AppendLine(category);
        }

        response.AppendLine("\n_\"Желтым обозначены категории задач, повышающие ваше настроение, а красным - понижающие\"_");

        await _botClient.SendTextMessageAsync(chatId, response.ToString(), parseMode: ParseMode.Markdown);
    }
}