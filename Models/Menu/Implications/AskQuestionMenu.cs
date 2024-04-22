using QATopics.Helpers;
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
    public class AskQuestionMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetNameOfMenu()
        {
            return nameof(AskQuestionMenu);
        }
        public override string GetMenuText()
        {
            return Replicas.AskQuestionText;
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            CommandResponse commandResponse = new CommandResponse(new MainMenu(this));
            if (command != "Назад")
            {
                if (User.Questions.Count >= 1000)
                {
                    commandResponse.ResultMessage = "У вас слишком много вопросов! Попробуйте удалить неактуальные вопросы.";
                    return commandResponse;
                }
                using ApplicationContext db = new ApplicationContext();
                Question question = new Question(User.Id, command);
                db.Questions.Add(question);
                db.SaveChanges();
                commandResponse.ResultMessage = "Ваш вопрос добавлен!";
            }
            return commandResponse;
        }
    }
}
