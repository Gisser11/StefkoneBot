using System.Threading.Tasks;
using Telegram.Bot.Types;
using ValiBot.Entities;

namespace ValiBot.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetOrCreate(Update update);
    }
}