using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using ValiBot.Commands.BotServices;
using ValiBot.Services;
using ValiBot.Services.Interfaces;

namespace ValiBot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IFillFormService _fillFormService;


        public TelegramBotController(ICommandExecutor commandExecutor, IFillFormService fillFormService)
        {
            _commandExecutor = commandExecutor;
            _fillFormService = fillFormService;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]Update upd)
        {
            Console.WriteLine("");

            if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
            {
                return Ok();
            }

            try
            {
                await _commandExecutor.Execute(upd);
            }
            catch (Exception e)
            {
                return Ok();
            }
            
            return Ok();
        }
        
       
    }
}