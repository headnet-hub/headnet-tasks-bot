using HeadNet.Bot.Common.Models;

namespace HeadNet.Bot.Services.Helpers;

public static class AdviceGenerator
{
    private static readonly List<Advice> Advices = new()
    {
        new Advice { Id = 1, Name = "Вечерняя медитация", Topic = "health" },
        new Advice { Id = 2, Name = "Утренняя пробежка", Topic = "sport" },
        new Advice { Id = 3, Name = "Разгадывание кроссвордов", Topic = "hobbies" },
        new Advice { Id = 4, Name = "Чтение мотивационной литературы", Topic = "rest" },
        new Advice { Id = 5, Name = "Игра в настольные игры с семьей", Topic = "communication" },
        new Advice { Id = 6, Name = "Посещение художественных выставок", Topic = "travel" },
        new Advice { Id = 7, Name = "Кулинарный эксперимент", Topic = "routine" },
        new Advice { Id = 8, Name = "Занятие йогой", Topic = "sport" },
        new Advice { Id = 9, Name = "Создание дневника благодарности", Topic = "hobbies" },
        new Advice { Id = 10, Name = "Создание собственного музыкального проекта", Topic = "hobbies" },
        new Advice { Id = 11, Name = "Организация семейного киносеанса", Topic = "communication" },
        new Advice { Id = 12, Name = "Посещение фитнес-центра", Topic = "sport" },
        new Advice { Id = 13, Name = "Вязание", Topic = "hobbies" },
        new Advice { Id = 14, Name = "Организация вечеринки", Topic = "communication" },
        new Advice { Id = 15, Name = "Утренние упражнения", Topic = "health" },
        new Advice { Id = 16, Name = "Занятие саморазвитием", Topic = "spirit" },
        new Advice { Id = 17, Name = "Занятие танцами", Topic = "hobbies" },
        new Advice { Id = 18, Name = "Планирование путешествия", Topic = "travel" },
        new Advice { Id = 19, Name = "Поход в музей", Topic = "travel" }
    };

    public static List<Advice> GetAdvicesByTopics(List<string> topics)
    {
        return Advices.Where(a => topics.Contains(a.Topic)).ToList();
    }
    
    public static List<Advice> GetAllAdvices()
    {
        return Advices;
    }
    
    public static Advice GetAdviceById(int id)
    {
        return Advices.FirstOrDefault(a => a.Id == id);
    }
}