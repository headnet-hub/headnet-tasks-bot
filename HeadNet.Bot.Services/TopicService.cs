using HeadNet.Bot.Common.Models;
using HeadNet.Bot.Services.Abstractions;
using HeadNet.Bot.Services.Helpers;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadNet.Bot.Services;

public class TopicService : ITopicService
{
    private readonly ITaskService _taskService;
    private readonly Dictionary<long, List<string>> _chatTopics = new();
    private readonly List<(string name, string code)> _categories = new()
    {
        ("Спорт", "sport"),
        ("Работа", "work"),
        ("Здоровье", "health"),
        ("Учеба", "studies"),
        ("Общение", "communication"),
        ("Отдых", "rest"),
        ("Путешествия и новые места", "travel"),
        ("Рутина", "routine"),
        ("Духовное развитие", "spirit"),
        ("Творчество и хобби", "hobbies"),
    };
    
    private readonly Dictionary<long, List<int>> _userAdvices = new();

    public TopicService(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public List<string> GetTopics(long chatId)
    {
        return _chatTopics.TryGetValue(chatId, out var topics) ? topics : new List<string>();
    }

    public void AddTopic(long chatId, string topic)
    {
        if (!_chatTopics.ContainsKey(chatId))
            _chatTopics.Add(chatId, new List<string>());
        
        if (!_chatTopics[chatId].Contains(topic))
            _chatTopics[chatId].Add(topic);
    }
    
    public void RemoveTopic(long chatId, string topic)
    {
        if (!_chatTopics.ContainsKey(chatId))
            _chatTopics.Add(chatId, new List<string>());
        
        if (_chatTopics[chatId].Contains(topic))
            _chatTopics[chatId].Remove(topic);
    }

    public InlineKeyboardMarkup BuildTopicsKeyboard(long chatId)
    {
        var keyboardInline = new List<InlineKeyboardButton[]>();
        
        if (!_chatTopics.TryGetValue(chatId, out var selectedTopics))
            selectedTopics = new List<string>();
        
        foreach (var (name, code) in _categories)
        {
            var buttonText = selectedTopics.Contains(code) ? $"{name}  \u2705" : name;
            keyboardInline.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonText, $"category_{code}") });
        }

        keyboardInline.Add(new[] { InlineKeyboardButton.WithCallbackData("Завершить выбор", "finish_selection") });

        return new InlineKeyboardMarkup(keyboardInline);
    }

    public Task<Advice> GetAdvice(long chatId)
    {
        var advices = _chatTopics.TryGetValue(chatId, out var selectedTopics) && selectedTopics.Count > 0
            ? AdviceGenerator.GetAdvicesByTopics(selectedTopics)
            : AdviceGenerator.GetAllAdvices();

        advices = advices.Where(a => !_userAdvices.ContainsKey(a.Id)).ToList();

        return Task.FromResult(GetRandom(advices));
    }
    
    public Task AddAdviceToUser(long chatId, int adviceId)
    {
        if (!_userAdvices.ContainsKey(chatId))
            _userAdvices.Add(chatId, new List<int>());
        
        _userAdvices[chatId].Add(adviceId);

        var advice = AdviceGenerator.GetAdviceById(adviceId);
        _taskService.AddUserTask(chatId, advice.Name);
        
        return Task.CompletedTask;
    }

    private static Advice GetRandom(IReadOnlyList<Advice> advices)
    {
        var random = new Random();
        var index = random.Next(advices.Count);
        return advices[index];
    }
}