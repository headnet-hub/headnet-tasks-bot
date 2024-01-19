using HeadNet.Bot.Common.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Services.Abstractions;

public interface ITopicService
{
    List<string> GetTopics(long chatId);
    void AddTopic(long chatId, string topic);
    void RemoveTopic(long chatId, string topic);
    InlineKeyboardMarkup BuildTopicsKeyboard(long chatId);
    Task<Advice> GetAdvice(long chatId);
    Task AddAdviceToUser(long chatId, int adviceId);
}