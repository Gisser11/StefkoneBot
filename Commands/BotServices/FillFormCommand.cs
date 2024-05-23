using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Services;
using ValiBot.Services.Interfaces;

namespace ValiBot.Commands.BotServices
{
    public class FillFormCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _telegramBotClient;
        public FillFormCommand(IUserService userService, TelegramBot telegramBotClient)
        {
            _userService = userService;
            _telegramBotClient = telegramBotClient.GetBot().Result;
        }

        public override string Name => CommandNames.FillFormCommand;
        
        public override async Task ExecuteAsync(Update update, InlineKeyboardMarkup inlineKeyboardMarkup)
        {
            
            var user = await _userService.GetOrCreate(update);

            await _telegramBotClient.SendTextMessageAsync(user.ChatId, "Расскажите немного о себе", replyMarkup: inlineKeyboardMarkup);
        }
    }
}