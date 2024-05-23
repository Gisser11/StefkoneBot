using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ValiBot.Services.Interfaces
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}