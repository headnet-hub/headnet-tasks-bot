using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;

namespace HeadNet.Bot.Controllers.CommandHandlers.Abstractions;

public class CommandHandlerFactory
{
    private readonly Dictionary<string, ICommandHandler> _commandHandlers;

    public CommandHandlerFactory(ITelegramBotClient botClient, IChatStateService stateService, ITopicService topicService, IMoodService moodService, ITaskService taskService)
    {
        _commandHandlers = new Dictionary<string, ICommandHandler>
        {
            { "/start", new StartCommandHandler(botClient) },
            { "/help", new HelpCommandHandler(botClient) },
            { "/connect_google", new ConnectGoogleCommandHandler(botClient, stateService) },
            { "/set_topics", new SetTopicsCommandHandler(botClient, stateService, topicService) },
            { "/view_day", new ViewDayCommandHandler(botClient, moodService, taskService) },
            { "/get_analytics", new GetAnalyticsCommandHandler(botClient, stateService, topicService, taskService, moodService) },
            { "/get_advice", new GetAdviceCommandHandler(botClient, stateService, topicService) },
            { "/rate_day", new RateDayCommandHandler(botClient, stateService) },
            { "/feedback", new FeedbackCommandHandler(botClient, stateService) },
        };
    }

    public ICommandHandler GetHandler(string command)
    {
        return _commandHandlers.TryGetValue(command ?? string.Empty, out var handler) ? handler : null;
    }
}