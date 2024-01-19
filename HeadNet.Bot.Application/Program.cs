using HeadNet.Bot.Services;

namespace HeadNet.Bot.Application
{
    public abstract class Program
    {
        private static void Main()
        {
            var taskService = new TaskService();
            
            var bot = new Bot(
                taskService,
                new GoogleIntegrationService(),
                new ChatStateService(),
                new TopicService(taskService),
                new MoodService());
            
            bot.Start();
        }
    }
}