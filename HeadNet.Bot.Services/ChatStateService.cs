using HeadNet.Bot.Common.Enums;
using HeadNet.Bot.Services.Abstractions;

namespace HeadNet.Bot.Services;

public class ChatStateService : IChatStateService
{
    private readonly Dictionary<long, ChatStates> _chatStates = new();

    public ChatStates GetState(long chatId)
    {
        return _chatStates.TryGetValue(chatId, out var state) ? state : ChatStates.None;
    }

    public void SetState(long chatId, ChatStates state)
    {
        _chatStates[chatId] = state;
    }
    
    public void CleanState(long chatId)
    {
        _chatStates[chatId] = ChatStates.None;
    }
}
