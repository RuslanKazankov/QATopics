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
        public override string GetNameOfMenu()
        {
            return nameof(QuestionsMenu);
        }

        public override string GetMenuText()
        {
            Question? question = PseudoDB.RandomlyQuestion(User);
            if (question == null)
                return "Вопросиков пока нет(((";
            
            User.CurrentQuestion = question;
            return question.Text + "\n" + Replicas.QuestionMenuText ?? "Походу тут ошибка произошла, сорян, листай дальше =)";
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("1"), //Ответить
                new KeyboardButton("2"), //Хороший вопрос
                new KeyboardButton("3"), //Следующий вопрос
                new KeyboardButton("4"), //Пожаловаться
                new KeyboardButton("Назад"),
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (User.CurrentQuestion == null)
                return new CommandResponse(new MainMenu(this));

            if (command == "1")
            {
                User.CurrentQuestion.LikeCount++;
                return new CommandResponse(new AnswerTheQuestionMenu(this));
            }
            if (command == "2")
            {
                User.CurrentQuestion.LikeCount++;
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "3")
            {
                return new CommandResponse(new QuestionsMenu(this));
            }

            return new CommandResponse(new MainMenu(this));
        }
    }
}
