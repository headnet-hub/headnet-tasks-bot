namespace HeadNet.Bot.Services.Abstractions;

public interface IMoodService
{
    short GetMoodForToday(long chatId);
    void SaveUserRate(long userId, short rating, DateTime date);
    double GetAverageMoodForWeek(long chatId);
}