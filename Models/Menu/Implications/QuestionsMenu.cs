﻿using QATopics.Helpers;
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

        public override string GetMenuText()
        {
            Question? question = User.UserSettings!.CurrentQuestion;
            if (question == null)
            {
                int randomId = (int)random.NextInt64(Db.Questions.LongCount());
                question = Db.Questions.Skip(randomId).FirstOrDefault();
            }

            if (question == null)
                return "Вопросов пока нет.";
            
            User.UserSettings!.CurrentQuestion = question;
            return $"{question.User!.Name}:\n{question.Text}\n{Replicas.QuestionMenuText}";
        }

        public override string GetNameOfMenu()
        {
            return nameof(QuestionsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            if (User.UserSettings!.CurrentQuestion == null)
            {
                return new KeyboardBuilder("Назад").BuildKeyboard();
            }
            return new KeyboardBuilder(["💬", "👍", "➡️", "🚩", "Назад"]).BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (User.UserSettings!.CurrentQuestion == null)
                return new CommandResponse(new MainMenu(this));

            if (command == "💬" || command == "1")
            {
                User.UserSettings!.CurrentQuestion.LikeCount++;
                return new CommandResponse(new AnswerTheQuestionMenu(this));
            }
            if (command == "👍" || command == "2")
            {
                User.UserSettings!.CurrentQuestion.LikeCount++;
                User.UserSettings!.CurrentQuestion = null;
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "➡️" || command == "3")
            {
                User.UserSettings!.CurrentQuestion = null;
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (command == "🚩" || command == "4")
            {
                return new CommandResponse(new QuestionReportMenu(this));
            }
            if (command == "Назад")
            {
                User.UserSettings!.CurrentQuestion = null;
                return new CommandResponse(new MainMenu(this));
            }
            return null;
        }
    }
}
