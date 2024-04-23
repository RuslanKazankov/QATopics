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
    public class AnswersOfQuestionAdminMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            if (User.CurrentQuestion != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Вопрос #").AppendLine(User.CurrentQuestion.Id.ToString());
                sb.Append("Вопрос от ").Append(User.CurrentQuestion.User!.Name).Append("#").AppendLine(User.CurrentQuestion.User.Id.ToString());
                sb.Append("Вопрос: ").AppendLine(User.CurrentQuestion.Text.Substring(0, Math.Min(User.CurrentQuestion.Text.Length, 50)));
                sb.AppendLine($"Всего ответов: {User.CurrentQuestion.Answers.Count}");
                IEnumerable<Answer> answers = User.CurrentQuestion.Answers
                    .OrderByDescending(a => a.Id)
                    .Skip(User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions * Config.CountMessagesOnPage)
                    .Take(Config.CountMessagesOnPage);
                foreach (Answer answer in answers)
                {
                    if (answer.GoodAnswer)
                    {
                        sb.Append("⭐ ");
                    }
                    sb.Append("Ответ #").Append(answer.Id).Append(" от ").AppendLine(answer.User!.Name);
                    sb.AppendLine(answer.Text);
                }
                return sb.ToString();
            }
            return "Вопрос не найден";
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswersOfQuestionAdminMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder(["Назад"]);
            if (User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions != 0)
            {
                keyboardBuilder.AddKeyboardButton("⬅");
            }
            int countSelectedQuestions = User.CurrentQuestion!.Answers.Count;
            int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
            if (User.Admin!.AdminSettings.PageOfAnswersOnPopularQuestions + 1 < countOfPages)
            {
                keyboardBuilder.AddKeyboardButton("➡");
            }
            return keyboardBuilder.BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                User.CurrentQuestion = null;
                return new CommandResponse(new PopularQuestionsMenu(this));
            }
            if (command == "⬅")
            {
                if (User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions != 0)
                {
                    User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions--;
                    return new CommandResponse(this);
                }
            }
            if (command == "➡")
            {
                int countSelectedQuestions = User.CurrentQuestion!.Answers.Count;
                int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
                if (User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions + 1 < countOfPages)
                {
                    User.Admin!.AdminSettings!.PageOfAnswersOnPopularQuestions++;
                    return new CommandResponse(this);
                }
            }
            return null;
        }
    }
}
