using Microsoft.EntityFrameworkCore.Diagnostics;
using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AnswersOnMyQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            StringBuilder sb = new StringBuilder();
            var answers = Db.Answers
                .OrderByDescending(a => a.Id)
                .Where(a => a.Question!.UserId == User.Id)
                .Skip(User.UserSettings!.PageOfAnswers * Config.CountMessagesOnPage)
                .Take(Config.CountMessagesOnPage / 2).ToList();
            foreach (var answer in answers)
            {
                if (answer.GoodAnswer)
                {
                    sb.Append("⭐ ");
                }
                sb.Append("Ответ #").Append(answer.Id);
                sb.Append("        Отправить жалобу /report_").AppendLine(answer.Id.ToString());
                sb.Append("Ваш вопрос: ").AppendLine(answer.Question!.Text);
                sb.Append("Ответ от ").Append(answer.User!.Name).Append(": ").AppendLine(answer.Text);
                sb.AppendLine();
            }
            sb.AppendLine("Напишите номер ответа, чтобы пометить ответ хорошим/снять метку хорошего ответа.");
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswersOnMyQuestionsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder(["Назад"]);
            if (User.UserSettings!.PageOfAnswers != 0)
            {
                keyboardBuilder.AddKeyboardButton("⬅");
            }
            int countSelectedQuestions = Db.Answers.Where(a=> a.Question!.UserId == User.Id).Count();
            int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
            if (User.UserSettings!.PageOfAnswers + 1 < countOfPages)
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
                if (User.UserSettings!.PageOfAnswers != 0)
                {
                    User.UserSettings!.PageOfAnswers--;
                    return new CommandResponse(this);
                }
            }
            if (command == "➡")
            {
                int countSelectedQuestions = Db.Answers.Where(a => a.Question!.UserId == User.Id).Count();
                int countOfPages = countSelectedQuestions / Config.CountMessagesOnPage + 1;
                if (User.UserSettings!.PageOfAnswers + 1 < countOfPages)
                {
                    User.UserSettings!.PageOfAnswers++;
                    return new CommandResponse(this);
                }
            }
            if (command.StartsWith("/report_"))
            {
                if (command.Split('_').Length > 0 && int.TryParse(command.Split('_')[1], out int idOfReport))
                {
                    Answer? answer = Db.Answers.Where((a) => a.Question!.UserId == User.Id && a.Id == idOfReport).FirstOrDefault();
                    if (answer == null)
                    {
                        return new CommandResponse(this) { ResultMessage = "Ответ не найден!" };
                    }
                    User.UserSettings!.CurrentAnswer = answer;
                    return new CommandResponse(new AnswerReportMenu(this));
                }
                return null;
            }
            if (int.TryParse(command, out int idOfAnswer))
            {
                Answer? answer = Db.Answers.Where((a) => a.Question!.UserId == User.Id && a.Id == idOfAnswer).FirstOrDefault();
                if (answer == null)
                {
                    return new CommandResponse(this) { ResultMessage = "Ответ не найден!"};
                }
                answer.GoodAnswer = !answer.GoodAnswer;
                Db.SaveChanges();
                return new CommandResponse(this) { ResultMessage = "Ответ #" + answer.Id + " оценен!" };
            }
            return null;
        }
    }
}
