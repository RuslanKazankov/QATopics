using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
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
    public class AnswerTheQuestionMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            return "Напишите ответ:";
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswerTheQuestionMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
                return new CommandResponse(new MainMenu(this));
            if (User.CurrentQuestion != null)
            {
                using ApplicationContext db = new ApplicationContext();
                Answer answer = new Answer(User.CurrentQuestion.Id, command, User.Id);
                db.Answers.Add(answer);
                db.SaveChanges();

                MessageService?.SendMessageAsync(User.CurrentQuestion.UserId, "На ваш вопрос ответили!");
                return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Ваш ответ добавлен!" };
            }
            return new CommandResponse(new MainMenu(this)) { ResultMessage = "Вопрос не найден" };
        }
    }
}
