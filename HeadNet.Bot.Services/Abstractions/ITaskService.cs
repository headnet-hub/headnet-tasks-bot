namespace HeadNet.Bot.Services.Abstractions;

public interface ITaskService
{
    List<string> GetUserTasksMock(long userId);
    void AddUserTask(long userId, string task);
    List<string> GetUserEventsMock(long userId);
    List<string> GetUserCategoriesMock(long userId);
}