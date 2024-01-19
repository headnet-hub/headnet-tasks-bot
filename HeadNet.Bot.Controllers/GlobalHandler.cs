using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Controllers.CallbackHandlers;
using HeadNet.Bot.Controllers.CallbackHandlers.Abstractions;
using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using HeadNet.Bot.Controllers.StateHandlers.Abstractions;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers;

public class GlobalHandler
{
    private readonly CommandHandlerFactory _handlerFactory;
    private readonly StateHandlerFactory _stateFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly IChatStateService _stateService;
    private readonly ICallbackHandler _topicSelectionHandler;
    private readonly GetAdviceHandler _adviceHandler;
    private readonly RateDayHandler _rateDayHandler;

    public GlobalHandler(ITelegramBotClient botClient,
        IGoogleIntegrationService googleService, 
        IChatStateService stateService, 
        ITopicService topicService,
        IMoodService moodService,
        ITaskService taskService)
    {
        _botClient = botClient;
        _stateService = stateService;
        _handlerFactory = new CommandHandlerFactory(botClient, stateService, topicService, moodService, taskService);
        _stateFactory = new StateHandlerFactory(botClient, googleService, stateService, topicService);
        _topicSelectionHandler = new TopicSelectionHandler(botClient, stateService, topicService);
        _adviceHandler = new GetAdviceHandler(botClient, stateService, topicService);
        _rateDayHandler = new RateDayHandler(botClient, stateService, moodService);
    }
    
    public async Task HandleMessage(Message message)
    {
        var handler = _handlerFactory.GetHandler(message.Text);
        
        if (handler != null)
        {
            _stateService.CleanState(message.Chat.Id);
            await handler.ExecuteAsync(message);
        }
        else
        {
            var chatState = _stateService.GetState(message.Chat.Id);
            
            if (chatState == ChatStates.None)
                await SendDefaultMessage(message);
            else
                await HandleStateDependentMessage(message, chatState);
        }
    }

    public async Task HandleQuery(CallbackQuery query)
    {
        var chatState = _stateService.GetState(query.From.Id);

        if (chatState == ChatStates.TopicSelection)
        {
            await _topicSelectionHandler.HandleCallbackAsync(query);
        }
        else if (chatState == ChatStates.GettingAdvice)
        {
            await _adviceHandler.HandleCallbackAsync(query);
        }
        else if (chatState is ChatStates.RatingDay or ChatStates.ChoosingRateDay)
        {
            await _rateDayHandler.HandleCallbackAsync(query);
        }
    }
    
    private async Task HandleStateDependentMessage(Message message, ChatStates state)
    {
        var handler = _stateFactory.GetHandler(state);
        
        if (handler == null)
            await SendDefaultMessage(message);
        else 
            await handler.HandleStateAsync(message);
    }
    
    private async Task SendDefaultMessage(Message message)
    {
        await _botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Sorry, I didn't understand that command.");
    }
}