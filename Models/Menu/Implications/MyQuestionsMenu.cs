using QATopics.Helpers;
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Напишите номер вопроса чтобы удалить его.");
            var questions = Db.Questions
                .OrderByDescending(q => q.Id)
                .Where(q => q.UserId == User.Id)
                .Skip(User.UserSettings!.PageOfMyQuestions * Config.CountMessagesOnPage)
                .Take(Config.CountMessagesOnPage);
            foreach(Question question in questions)
            {
                sb.Append("Вопрос #").AppendLine(question.Id.ToString());
                sb.AppendLine(question.Text);
                sb.Append("Лайков: ").AppendLine(question.LikeCount.ToString());
            }
            return sb.ToString();
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder(["Назад"]);
            if (User.UserSettings!.PageOfMyQuestions != 0)
            {
                keyboardBuilder.AddKeyboardButton("⬅");
            }
            int countSelectedQuestions = User.Questions.Count;
            int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
            if (User.UserSettings!.PageOfMyQuestions + 1 < countOfPages)
            {
                keyboardBuilder.AddKeyboardButton("➡");
            }
            return keyboardBuilder.BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new MainMenu(this));
            }
            if (command == "⬅")
            {
                if (User.UserSettings!.PageOfMyQuestions != 0)
                {
                    User.UserSettings!.PageOfMyQuestions--;
                    return new CommandResponse(this);
                }
            }
            if (command == "➡")
            {
                int countSelectedQuestions = User.Questions.Count;
                int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
                if (User.UserSettings!.PageOfMyQuestions + 1 < countOfPages)
                {
                    User.UserSettings!.PageOfMyQuestions++;
                    return new CommandResponse(this);
                }
            }
            int idOfQuestion = -1;
            if (int.TryParse(command, out idOfQuestion))
            {
                Question? question = Db.Questions.Where((q) =>  q.UserId == User.Id && q.Id == idOfQuestion).FirstOrDefault();
                if (question == null)
                {
                    return new CommandResponse(this) { ResultMessage = "Вопрос не найден" };
                }
                else
                {
                    Db.Questions.Remove(question);
                    Db.SaveChanges();
                    return new CommandResponse(this) { ResultMessage = $"Вопрос #{question.Id} удалён" };
                }
            }
            return null;
        }
    }
}
