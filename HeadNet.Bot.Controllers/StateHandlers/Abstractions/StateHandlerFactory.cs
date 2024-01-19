using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CallbackHandlers;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;

namespace HeadNet.Bot.Controllers.StateHandlers.Abstractions;

public class StateHandlerFactory
{
    private readonly Dictionary<ChatStates, IStateHandler> _stateHandlers;

    public StateHandlerFactory(ITelegramBotClient botClient, IGoogleIntegrationService googleService, IChatStateService stateService, ITopicService topicService)
    {
        _stateHandlers = new Dictionary<ChatStates, IStateHandler>
        {
            { ChatStates.AwaitingGoogleId, new GoogleIdStateHandler(botClient, googleService, stateService) },
            { ChatStates.AwaitingFeedback, new FeedbackStateHandler(botClient, stateService) }
        };
    }

    public IStateHandler GetHandler(ChatStates state)
    {
        return _stateHandlers.TryGetValue(state, out var handler) ? handler : null;
    }
}