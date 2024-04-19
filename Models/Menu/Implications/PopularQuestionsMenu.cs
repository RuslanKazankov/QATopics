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
            List<Question> questions = PseudoDB.Questions.OrderBy(q => q.LikeCount).TakeLast(10).ToList();
            StringBuilder sb = new StringBuilder("Топ самых популярных вопросов:\n");
            foreach (Question question in questions)
            {
                sb.Append("Вопрос #").Append(question.Id)
                    .Append(" задан ").Append(question.User.Name).Append("#").Append(question.UserId)
                    .Append(" Лайков: ").AppendLine(question.LikeCount.ToString());
                sb.Append("Вопрос: ").AppendLine(question.Text);
                foreach (Answer answer in question.Answers.Where(a => a.GoodAnswer).TakeLast(10))
                {
                    sb.Append("    Ответ от ").Append(answer.Responder.Name).Append("#").AppendLine(answer.ResponderId.ToString())
                        .AppendLine("    " + answer.Text);
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
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            return new CommandResponse(new MainMenu(this));
        }
    }
}
