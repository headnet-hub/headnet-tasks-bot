using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class HelpCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public HelpCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(Message message)
    {
        const string helpText = "Commands:\n" +
                                "/start - Get information about the app\n" +
                                "/help - Get descriptions of commands\n" +
                                "/connect_google <google_id> - Integrate with Google Calendar and Google Tasks\n" +
                                "/set_topics - Choose from 10 topics for recommendations\n" +
                                "/view_day - View mood rating for the day, calendar events, and tasks\n" +
                                "/get_analytics - Get information on how task categories affect you\n" +
                                "/get_advice - Get advice to improve mood (with option to accept and add to tasks or change)\n" +
                                "/rate_day - Rate your mood for the day (1-5), with an option to specify the date\n" +
                                "/feedback - Send feedback";
        
        await _botClient.SendTextMessageAsync(message.Chat.Id, helpText);
    }
}