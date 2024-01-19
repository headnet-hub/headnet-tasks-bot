using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.StateHandlers.Abstractions;

public interface IStateHandler
{
    Task HandleStateAsync(Message message);
}