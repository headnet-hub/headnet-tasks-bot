using HeadNet.Bot.Services.Abstractions;

namespace HeadNet.Bot.Services;

//TODO: интеграция с Google
public class TaskService : ITaskService
{
    private readonly Dictionary<long, List<string>> _customUserTasks = new();
    public List<string> GetUserTasksMock(long userId)
    {
        if (!_customUserTasks.TryGetValue(userId, out var customUserTasks))
            customUserTasks = new List<string>();

        return new List<string>
            {
                "Закончить отчет по проекту",
                "Сходить за продуктами"
            }
            .Concat(customUserTasks)
            .ToList();
    }

    public void AddUserTask(long userId, string task)
    {
        if (!_customUserTasks.ContainsKey(userId))
            _customUserTasks.Add(userId, new List<string>());
        
        _customUserTasks[userId].Add(task);
    }

    public List<string> GetUserEventsMock(long userId)
    {
        return new List<string> 
        { 
            "День рождения Маши (весь день)", 
            "Встреча с клиентом (17:00-18:00)", 
            "Поход в кино (18:30-20:00)", 
            "Семейный ужин (20:30-21:00)" 
        };
    }

    public List<string> GetUserCategoriesMock(long userId)
    {
        return new List<string>
        {
            "\ud83d\udfe1  Просмотр фильмов и сериалов",
            "\ud83d\udfe1  Проектная работа",
            "\ud83d\udd34  Домашние задания",
            "\ud83d\udd34  Уборка",
            "\ud83d\udd34  Силовой спорт",
            "\ud83d\udfe1 Длительность событий",
            "\ud83d\udd34  Количество задач",
        };
    }
}