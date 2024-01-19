using HeadNet.Bot.Services.Abstractions;

namespace HeadNet.Bot.Services;

public class MoodService : IMoodService
{
    private readonly Dictionary<long, List<(DateOnly, short)>> _userMoods = new();
    
    public short GetMoodForToday(long chatId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return _userMoods.TryGetValue(chatId, out var moods)
            ? moods.FirstOrDefault(m => m.Item1 == today).Item2
            : (short)0;
    }
    
    public void SaveUserRate(long userId, short rating, DateTime date)
    {
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        
        if (!_userMoods.TryGetValue(userId, out _))
            _userMoods.Add(userId, new List<(DateOnly, short)>());

        _userMoods[userId].RemoveAll((moods) => moods.Item1 == dateOnly);
        _userMoods[userId].Add((dateOnly, rating));
    }

    public double GetAverageMoodForWeek(long chatId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (!_userMoods.TryGetValue(chatId, out var moods))
            moods = new List<(DateOnly, short)>();

        moods = moods.Where(m => m.Item1 >= today.AddDays(-6)).ToList();
        return moods.Any() ? moods.Average(m => m.Item2) : 0;
    }
}