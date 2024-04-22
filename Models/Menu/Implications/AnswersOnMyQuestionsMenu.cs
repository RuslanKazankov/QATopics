using Microsoft.EntityFrameworkCore.Diagnostics;
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
            using ApplicationContext db = new ApplicationContext();
            StringBuilder sb = new StringBuilder();
            foreach (var answer in db.Answers.Where(a => a.Question!.UserId == User.Id).TakeLast(20))
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
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("Назад")
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new MainMenu(this));
            }
            if (command.StartsWith("/report_"))
            {
                if (command.Split('_').Length > 0 && int.TryParse(command.Split('_')[1], out int idOfReport))
                {
                    using ApplicationContext db = new ApplicationContext();
                    Answer? answer = db.Answers.Where((a) => a.Question!.UserId == User.Id && a.Id == idOfReport).FirstOrDefault();
                    if (answer == null)
                    {
                        return new CommandResponse(this) { ResultMessage = "Ответ не найден!" };
                    }
                    User.CurrentAnswer = answer;
                    return new CommandResponse(new AnswerReportMenu(this));
                }
                return null;
            }
            if (int.TryParse(command, out int idOfAnswer))
            {
                using ApplicationContext db = new ApplicationContext();
                Answer? answer = db.Answers.Where((a) => a.Question!.UserId == User.Id && a.Id == idOfAnswer).FirstOrDefault();
                if (answer == null)
                {
                    return new CommandResponse(this) { ResultMessage = "Ответ не найден!"};
                }
                answer.GoodAnswer = !answer.GoodAnswer;
                db.SaveChanges();
                return new CommandResponse(this) { ResultMessage = "Ответ #" + answer.Id + " оценен!" };
            }
            return null;
        }
    }
}
