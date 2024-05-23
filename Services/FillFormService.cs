using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Entities;
using ValiBot.Services.Interfaces;

namespace ValiBot.Services
{
    public class FillFormService : IFillFormService
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly IUserService _userService;
        private readonly DataContext _context;
        
        public FillFormService(TelegramBot telegramBotClient, IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
            _telegramBotClient = telegramBotClient.GetBot().Result;
        }

        public async Task FillForm(Update upd, InlineKeyboardMarkup inlineKeyboardMarkup, int questionIndex)
        {
            var user = await _userService.GetOrCreate(upd);
            
            await _telegramBotClient.SendTextMessageAsync(user.ChatId, questionIndex.ToString());

            RegisterForm registerForm = new RegisterForm()
            {
                AppUser = user,
                NeededPlatform = upd.Message.Text
            };
            await _context.RegisterForms.AddAsync(registerForm);
        }
    }
}