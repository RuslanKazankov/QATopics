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
        private const int countQuestionsOnOnePage = 20;
        public override string GetMenuText()
        {
            StringBuilder sb = new StringBuilder("Топ самых популярных вопросов:\n");
            List<Question> questions = Db.Questions.OrderByDescending(q => q.Id).Take(User.Admin!.AdminSettings!.CountOfSelectedQuestions)
                .OrderByDescending(q => q.LikeCount)
                .Skip(User.Admin.AdminSettings.PageOfPopularQuestionsMenu * countQuestionsOnOnePage)
                .Take(countQuestionsOnOnePage).ToList();
            foreach (Question question in questions)
            {
                sb.Append("Вопрос #").Append(question.Id)
                    .Append(" задан ").Append(question.User!.Name).Append("#").Append(question.UserId)
                    .Append(" Лайков: ").AppendLine(question.LikeCount.ToString());
                sb.Append("Вопрос: ").AppendLine(question.Text);
                sb.Append("Подробнее: /selectquestion_").AppendLine(question.Id.ToString());
                var answers = question.Answers.Where(a => a.GoodAnswer).Take(5);
                foreach (Answer answer in answers)
                {
                    sb.Append("        Ответ от ").Append(answer.User!.Name).Append("#").AppendLine(answer.UserId.ToString())
                        .AppendLine("        " + answer.Text);
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
            if (User.Admin!.AdminSettings.CountOfSelectedQuestions / countQuestionsOnOnePage != User.Admin.AdminSettings.PageOfPopularQuestionsMenu + 1)
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
                User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu--;
                return new CommandResponse(this);
            }
            if (command == "➡")
            {
                User.Admin!.AdminSettings!.PageOfPopularQuestionsMenu++;
                return new CommandResponse(this);
            }
            if (command.StartsWith("/selectquestion_"))
            {
                if (int.TryParse(command.Split('_')[1], out int questionId))
                {
                    User.CurrentQuestion = Db.Questions.Where(q => q.Id == questionId).FirstOrDefault();
                    return new CommandResponse(new AnswersOfQuestionAdminMenu(this));
                }
                return new CommandResponse(this) { ResultMessage = "Вопрос не найден"};
            }
            return null;
        }
    }
}
