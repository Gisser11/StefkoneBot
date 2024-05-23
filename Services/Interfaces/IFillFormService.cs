using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ValiBot.Services.Interfaces
{
    public interface IFillFormService
    {
        Task FillForm(Update upd, InlineKeyboardMarkup inlineKeyboardMarkup, int questionIndex);

    }
}