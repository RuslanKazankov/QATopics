using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class MainMenu : BaseMenu
    {
        public MainMenu(IMenuParams menuParams) : base(menuParams) { }

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
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("1"),
                new KeyboardButton("2"),
                new KeyboardButton("3"),
                new KeyboardButton("4"),
                new KeyboardButton("5"),
                new KeyboardButton("6"),
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            CommandResponse? commandResponse = null;
            if (command == "1") //Отвечать на вопросы
            {
                commandResponse = new CommandResponse(new QuestionsMenu(this));
            }
            else if (command == "2") //Задать свой вопрос
            {
                commandResponse = new CommandResponse(new AskQuestionMenu(this));
            }
            else if (command == "3") //Изменить имя
            {
                commandResponse = new CommandResponse(new ChangeNameMenu(this));
            }
            else if (command == "4") //Мои вопросы (Актуальные)
            {
                commandResponse = new CommandResponse(new MainMenu(this));
            }
            else if (command == "5") //Статистика (В будущем)
            {
                commandResponse = new CommandResponse(new MainMenu(this));
            }
            else if (command == "6") //Ответы
            {
                commandResponse = new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }

            return commandResponse;
        }
    }
}
