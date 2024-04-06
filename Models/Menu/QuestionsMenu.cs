using QATopics.Models.MenuCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public class QuestionsMenu(BotUser botUser) : BaseMenu(botUser)
    {
        public override string GetMenuText()
        {
            return "Вопросиков пока нет(((";
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("1"),
                new KeyboardButton("2"), 
                new KeyboardButton("3"),  
                new KeyboardButton("4"),  
                new KeyboardButton("Назад"), 
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            return new CommandResponse(new MainMenu(CurrentUser));
        }
    }
}
