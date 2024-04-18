using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public StartCommand(IUserService userService, TelegramBot telegramBot)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update, InlineKeyboardMarkup inlineKeyboardMarkup)
        {
            var user = await _userService.GetOrCreate(update);
            var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton
                    {
                        Text = "Создать операцию"
                    },
                    new KeyboardButton
                    {
                        Text = "Получить операции"
                    },
                    new KeyboardButton
                    {
                        Text = "i wanna get offer"
                    }
                }
            }, 
                resizeKeyboard: true);

            await _botClient.SendTextMessageAsync(user.ChatId, "Привет! Я помогу тебе разобраться в процессинге. ", 
                ParseMode.Markdown, replyMarkup:inlineKeyboard);
        }
    }
}