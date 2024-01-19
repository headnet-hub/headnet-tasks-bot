using Telegram.Bot.Types;

namespace HeadNet.Bot.Controllers.CallbackHandlers.Abstractions;

public interface ICallbackHandler
{
    Task HandleCallbackAsync(CallbackQuery query);
}