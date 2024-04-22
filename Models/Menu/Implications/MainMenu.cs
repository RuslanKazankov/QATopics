using QATopics.Helpers;
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
            StringBuilder sb = new StringBuilder();
            sb.Append("Привет, ").Append(User.Name).AppendLine(" 🖐\n").Append(Replicas.MainMenuText);
            if (User.Admin != null)
                sb.Append("\n/adminpanel - для админ-меню");
            return sb.ToString();
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder(["1🔎", "2❓", "3", "4", "5"]).BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "1🔎" || command == "1") //Отвечать на вопросы
            {
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "2❓" || command == "2") //Задать свой вопрос
            {
                return new CommandResponse(new AskQuestionMenu(this));
            }
            if (command == "3") //Изменить имя
            {
                return new CommandResponse(new ChangeNameMenu(this));
            }
            if (command == "4") //Мои вопросы
            {
                return new CommandResponse(new MyQuestionsMenu(this));
            }
            if (command == "5") //Ответы
            {
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }
            if (command == "/adminpanel" && User.Admin != null)
            {
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = Replicas.WelcomeAdminText };
            }
            return null;
        }
    }
}
