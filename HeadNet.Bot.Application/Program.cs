using HeadNet.Bot.Services;

namespace HeadNet.Bot.Application
{
    public abstract class Program
    {
        private static void Main(string telegramBotToken)
        {
            var taskService = new TaskService();
            
            var bot = new Bot(
                telegramBotToken,
                taskService,
                new GoogleIntegrationService(),
                new ChatStateService(),
                new TopicService(taskService),
                new MoodService());
            
            bot.Start();
        }
    }
}