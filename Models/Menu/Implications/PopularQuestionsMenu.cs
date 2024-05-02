using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class PopularQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Всего вопросов: {Db.Questions.Where(q => q.AskDate >= DateTime.Today.AddDays(-Config.DaysOfLiveQuestion)).Count()}");
            var questions = Db.Questions
                .Where(q => q.AskDate >= DateTime.Today.AddDays(-Config.DaysOfLiveQuestion))
                .OrderByDescending(q => q.LikeCount)
                .Skip(User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu * Config.CountMessagesOnPage)
                .Take(Config.CountMessagesOnPage).ToList();
            sb.AppendLine("Топ самых популярных вопросов:");
            foreach (Question question in questions)
            {
                sb.Append("Вопрос #").Append(question.Id)
                    .Append(" задан ").Append(question.User!.Name).Append("#").Append(question.UserId)
                    .Append(" Лайков: ").AppendLine(question.LikeCount.ToString());
                sb.Append("Вопрос: ").AppendLine(question.Text);
                sb.Append("Подробнее: /selectquestion_").AppendLine(question.Id.ToString());
                Answer? answer = question.Answers.Where(a => a.GoodAnswer).FirstOrDefault();
                if (answer != null)
                {
                    sb.Append("        Ответ от ").Append(answer.User!.Name).Append("#").AppendLine(answer.UserId.ToString())
                        .AppendLine("        " + answer.Text[..Math.Min(answer.Text.Length, 50)]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(PopularQuestionsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder(["Назад"]);
            if (User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu != 0)
            {
                keyboardBuilder.AddKeyboardButton("⬅");
            }
            int countSelectedQuestions = Db.Questions.Where(q => q.AskDate >= DateTime.Today.AddDays(-Config.DaysOfLiveQuestion)).Count();
            int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
            if (User.Admin!.AdminSettings.PageOfPopularQuestionsMenu + 1 < countOfPages)
            {
                keyboardBuilder.AddKeyboardButton("➡");
            }
            return keyboardBuilder.BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new AdminMenu(this));
            }
            if (command == "⬅")
            {
                if (User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu > 0)
                {
                    User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu--;
                }
                return new CommandResponse(this);
            }
            if (command == "➡")
            {
                int countSelectedQuestions = Db.Questions.Where(q => q.AskDate >= DateTime.Today.AddDays(-Config.DaysOfLiveQuestion)).Count();
                int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
                if (User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu + 1 < countOfPages)
                {
                    User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu++;
                }
                return new CommandResponse(this);
            }
            if (command.StartsWith("/selectquestion_"))
            {
                if (int.TryParse(command.Split('_')[1], out int questionId))
                {
                    User.UserSettings!.CurrentQuestion = Db.Questions.Where(q => q.Id == questionId).FirstOrDefault();
                    return new CommandResponse(new AnswersOfQuestionAdminMenu(this));
                }
                return new CommandResponse(this) { ResultMessage = "Вопрос не найден"};
            }
            return null;
        }
    }
}
