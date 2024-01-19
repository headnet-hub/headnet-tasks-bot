using HeadNet.Bot.Controllers;
using HeadNet.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Application;

public class Bot
{
    private const string TelegramBotToken = "6617885941:AAEJbQ_l-nscdSS5caGm9hHPNXlfRL4JO7w";
    
    private readonly ITelegramBotClient _botClient;
    private readonly GlobalHandler _globalHandler;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public Bot(ITaskService taskService, 
        IGoogleIntegrationService googleService,
        IChatStateService stateService,
        ITopicService topicService,
        IMoodService moodService)
    {
        _botClient = new TelegramBotClient(TelegramBotToken);
        _globalHandler = new GlobalHandler(_botClient, googleService, stateService, topicService, moodService, taskService);
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    public void Start()
    {
        SetCommandsAsync().Wait();
        
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // Оставить пустым, если нужно получать все обновления
        };
        
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );

        Console.WriteLine("Bot is running...");
        
        Console.ReadLine();
        _cancellationTokenSource.Cancel();
    }

    private async Task SetCommandsAsync()
    {
        var commands = new List<BotCommand>
        {
            new() { Command = "start", Description = "Get information about the app" },
            new() { Command = "help", Description = "Get descriptions of commands" },
            new() { Command = "connect_google", Description = "Integrate with Google Calendar and Google Tasks" },
            new() { Command = "set_topics", Description = "Choose from 10 topics for recommendations" },
            new() { Command = "view_day", Description = "View mood rating for the day, calendar events, and tasks" },
            new() { Command = "get_analytics", Description = "Get information on how task categories affect you" },
            new() { Command = "get_advice", Description = "Get advice to improve mood" },
            new() { Command = "rate_day", Description = "Rate your mood for the day" },
            new() { Command = "feedback", Description = "Send feedback" },
        };

        await _botClient.SetMyCommandsAsync(commands, BotCommandScope.Default(), cancellationToken: _cancellationTokenSource.Token);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message != null)
            await _globalHandler.HandleMessage(update.Message);
        else if (update.CallbackQuery != null)
            await _globalHandler.HandleQuery(update.CallbackQuery);
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"An error occurred: {exception.Message}");
        return Task.CompletedTask;
    }
}