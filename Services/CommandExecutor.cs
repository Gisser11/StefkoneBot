using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Commands;
using ValiBot.Entities;
using ValiBot.Repository.Interfaces;
using ValiBot.Services.Interfaces;

namespace ValiBot.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<BaseCommand> _commands;
        private BaseCommand _lastCommand;
        private readonly IFillFormService _fillFormService;
        private readonly IBaseRepository<AppUser> _userRepository;
        
        public CommandExecutor(IServiceProvider serviceProvider, IFillFormService fillFormService, IBaseRepository<AppUser> userRepository)
        {
            _fillFormService = fillFormService;
            _userRepository = userRepository;
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        
        public async Task Execute(Update update)
        {
            var currentUser = _userRepository.GetAll().FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);

            //TODO
            if (currentUser == null)
            {
                return;
            }
            
            var questionIndex = currentUser.QuestionIndex;

            var lastCommand = currentUser.LastCommand;
            
            if(update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message?.Text)
                {
                    case "Создать операцию":
                        await ExecuteCommand(CommandNames.AddOperationCommand, update);
                        return;
                    case "Получить операции":
                        await ExecuteCommand(CommandNames.GetOperationsCommand, update);
                        return;
                    case "i wanna get offer":
                        await ExecuteCommand(CommandNames.FillFormCommand, update);
                        return;
                }
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("/start"))
                {
                    await ExecuteCommand(CommandNames.StartCommand, update);
                    return;
                }
            }

            
            
            switch (lastCommand)
            {
                case CommandNames.FillFormCommand:
                {
                    questionIndex++;
                    await _fillFormService.FillForm(update, CancelKeyboard(), questionIndex);
                    break;
                }
                case CommandNames.SelectCategoryCommand:
                {
                    await ExecuteCommand(CommandNames.FinishOperationCommand, update);
                    break;
                }
                case null:
                {
                    await ExecuteCommand(CommandNames.StartCommand, update);
                    break;
                }
            }
        }
        
        private async Task ExecuteCommand(string commandName, Update update)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            
            await _lastCommand.ExecuteAsync(update, CancelKeyboard());
        }

        private static InlineKeyboardMarkup CancelKeyboard()
        {
            var startOverButton = InlineKeyboardButton.WithCallbackData("Чтобы вернуться в меню, нажми кнопку ниже", "/start");

            return new InlineKeyboardMarkup(new[]
            {
                new[] { startOverButton }
            });
        }
    }
}