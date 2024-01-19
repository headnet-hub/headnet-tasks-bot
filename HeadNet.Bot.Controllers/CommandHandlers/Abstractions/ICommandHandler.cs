using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CommandHandlers.Abstractions;

public interface ICommandHandler
{
    Task ExecuteAsync(Message message);
}