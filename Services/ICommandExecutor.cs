using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ValiBot.Services
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}