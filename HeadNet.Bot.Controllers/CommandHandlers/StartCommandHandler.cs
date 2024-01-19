using HeadNet.Bot.Controllers.CommandHandlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CommandHandlers;

public class StartCommandHandler : ICommandHandler
{
    private readonly ITelegramBotClient _botClient;

    public StartCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(Message message)
    {
        const string response = "Добро пожаловать в HeadNet! Я - ваш персональный планировщик задач и целей. " +
                                "Перед началом работы необходимо провести интеграцию с Google Calendar " +
                                "(/connect_google) и выбрать предпочтительные категории (/set_topics).";

        await _botClient.SendTextMessageAsync(message.Chat.Id, response);
    }
}