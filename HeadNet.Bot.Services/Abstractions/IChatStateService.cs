using HeadNet.Bot.Common.Enums;

namespace HeadNet.Bot.Services.Abstractions;

public interface IChatStateService
{
    ChatStates GetState(long chatId);
    void SetState(long chatId, ChatStates state);
    void CleanState(long chatId);
}