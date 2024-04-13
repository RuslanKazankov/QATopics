using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public class MainMenu(User user) : BaseMenu(user)
    {
        public override string GetNameOfMenu()
        {
            return nameof(MainMenu);
        }
        public override string GetMenuText()
        {
            return Replicas.MainMenuText;
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new (new KeyboardButton[] {
                new KeyboardButton("1"),
                new KeyboardButton("2"),
                new KeyboardButton("3"), 
                new KeyboardButton("4"),
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            CommandResponse? commandResponse = null;
            if (command == "1") //Отвечать на вопросы
            {
                commandResponse = new CommandResponse(new QuestionsMenu(CurrentUser));
            }
            else if (command == "2") //Задать свой вопрос
            {
                commandResponse = new CommandResponse(new AskQuestion(CurrentUser));
            }
            else if (command == "3") //Изменить имя
            {
                commandResponse = new CommandResponse(new ChangeNameMenu(CurrentUser));
            }
            else if (command == "4") //Мои вопросы (Актуальные)
            {
                commandResponse = new CommandResponse(new MainMenu(CurrentUser));
            }

            return commandResponse;
        }
    }
}
