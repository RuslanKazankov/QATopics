using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class MyQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetNameOfMenu()
        {
            return nameof(MyQuestionsMenu);
        }
        public override string GetMenuText()
        {
            return "Напишите номер вопроса чтобы удалить его.\nВаши вопросы:";
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("Назад")
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            CommandResponse commandResponse;
            if (command == "Назад")
            {
                User.Name = command;
                commandResponse = new CommandResponse(new MainMenu(this));
            }
            else
            {
                commandResponse = new CommandResponse(new MyQuestionsMenu(this));
                commandResponse.ResultMessage = "Вопросов пока нет";
            }
            return commandResponse;
        }
    }
}
