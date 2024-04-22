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
            using ApplicationContext db = new ApplicationContext();
            StringBuilder sb = new StringBuilder();
            foreach(Question question in db.Questions.Where(q => q.UserId == User.Id))
            {
                sb.Append("Вопрос #").AppendLine(question.Id.ToString());
                sb.AppendLine(question.Text);
                sb.Append("Лайков: ").AppendLine(question.LikeCount.ToString());
            }
            sb.AppendLine("Напишите номер вопроса чтобы удалить его.");
            return sb.ToString();
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
            CommandResponse? commandResponse = null;
            int idOfQuestion = -1;
            if (int.TryParse(command, out idOfQuestion))
            {
                using ApplicationContext db = new ApplicationContext();
                Question? question = db.Questions.Where((q) =>  q.UserId == User.Id && q.Id == idOfQuestion).FirstOrDefault();
                commandResponse = new CommandResponse(new MyQuestionsMenu(this));
                if (question == null)
                {
                    commandResponse.ResultMessage = "Вопрос не найден";
                }
                else
                {
                    db.Questions.Remove(question);
                    db.SaveChanges();
                    commandResponse.ResultMessage = "Вопрос #" + question.Id + " удалён!";
                }
            }
            if (command == "Назад")
            {
                commandResponse = new CommandResponse(new MainMenu(this));
            }
            return commandResponse;
        }
    }
}
