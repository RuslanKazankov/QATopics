using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using QATopics.Services;
using QATopics.Services.Implications;
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
            string menuText = Replicas.MainMenuText;
            if (RoleService.IsAdmin(User.Id))
                menuText += "\n/adminpanel - для админ-меню";
            return menuText;
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
            if (command == "1") //Отвечать на вопросы
            {
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "2") //Задать свой вопрос
            {
                return new CommandResponse(new AskQuestionMenu(this));
            }
            if (command == "3") //Изменить имя
            {
                return new CommandResponse(new ChangeNameMenu(this));
            }
            if (command == "4") //Мои вопросы (Актуальные)
            {
                return new CommandResponse(new MyQuestionsMenu(this));
            }
            if (command == "5") //Статистика (В будущем)
            {
                return new CommandResponse(new MainMenu(this));
            }
            if (command == "6") //Ответы
            {
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }
            if (command == "/adminpanel" && RoleService.IsAdmin(User.Id))
            {
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = Replicas.WelcomeAdminText };
            }
            return null;
        }
    }
}
