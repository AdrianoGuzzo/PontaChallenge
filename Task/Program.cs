
using ApiTask;
namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = Config.CreateBuilder(args);

            Config.ConfigApp(app);
        }
    }
}
