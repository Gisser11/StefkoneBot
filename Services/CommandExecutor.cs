﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Commands;

namespace ValiBot.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<BaseCommand> _commands;
        private BaseCommand _lastCommand;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        
        public async Task Execute(Update update)
        {
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
            

            // AddOperation => SelectCategory => FinishOperation
            // 
            switch (_lastCommand?.Name)
            {
                case CommandNames.AddOperationCommand:
                {
                    await ExecuteCommand(CommandNames.SelectCategoryCommand, update);
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
            
            var startOverButton = InlineKeyboardButton.WithCallbackData("Вернуться в меню", "/start");

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { startOverButton }
            });
            
            await _lastCommand.ExecuteAsync(update, keyboard);
        }
    }
}