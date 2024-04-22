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
    public class QuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        private Random random = new Random();
        public override string GetNameOfMenu()
        {
            return nameof(QuestionsMenu);
        }

        public override string GetMenuText()
        {
            using ApplicationContext db = new ApplicationContext();
            long randomId = random.NextInt64(db.Questions.LongCount());
            Question? question = db.Questions.Where(q => q.Id == randomId).FirstOrDefault();
            if (question == null)
                return "Вопросов пока нет.";
            
            User.CurrentQuestion = question;
            return "❔: " + question.Text + "\n" + Replicas.QuestionMenuText ?? "Походу тут ошибка произошла, сорян, листай дальше =)";
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            if (User.CurrentQuestion == null)
            {
                return new KeyboardBuilder("Назад").BuildKeyboard();
            }
            return new KeyboardBuilder(["💬", "👍", "➡️", "🚩", "Назад"]).BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (User.CurrentQuestion == null)
                return new CommandResponse(new MainMenu(this));

            if (command == "💬" || command == "1")
            {
                User.CurrentQuestion.LikeCount++;
                return new CommandResponse(new AnswerTheQuestionMenu(this));
            }
            if (command == "👍" || command == "2")
            {
                User.CurrentQuestion.LikeCount++;
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "➡️" || command == "3")
            {
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "🚩" || command == "4")
            {
                return new CommandResponse(new QuestionReportMenu(this));
            }
            if (command == "Назад")
            {
                return new CommandResponse(new MainMenu(this));
            }
            return null;
        }
    }
}
