using QATopics.Models.Menu;
using QATopics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.MenuCommands
{
    public class CommandResponse
    {
        public BaseMenu NextMenu { get; set; }
        public ReplyKeyboardMarkup? ReplyKeyboard { get; private set; }
        public string? ResultMessage;
        public CommandResponse(BaseMenu nextMenu)
        {
            NextMenu = nextMenu;
            ReplyKeyboard = nextMenu.GetRelplyKeyboard();
        }
    }
}
