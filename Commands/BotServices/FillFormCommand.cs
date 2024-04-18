using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Services;

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
            
            await _telegramBotClient.SendTextMessageAsync(user.ChatId, "Мы крайне рады, что вы решили присоединиться к нам! ");
            
            await _telegramBotClient.SendTextMessageAsync(user.ChatId, "Для начала необходимо заполнить форму", replyMarkup: inlineKeyboardMarkup);
            
            var poll1 = await _telegramBotClient.SendPollAsync(
                chatId: user.ChatId,
                question: "Какую книгу вы читаете сейчас? Опишите впечатления.",
                options: new[] { "Ответ 1", "Ответ 2", "Ответ 3" }
            );
            
            
        }
        
        private Task<Update> GetNextUpdate(Update update)
        {
            return Task.FromResult(update);
        }
    }
}