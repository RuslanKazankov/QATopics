﻿using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using QATopics.Services.Implications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AnswerTheQuestionMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            return "Напишите ответ:";
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswerTheQuestionMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
                return new CommandResponse(new MainMenu(this));

            Answer answer = new Answer() { Id = PseudoDB.Answers.LastOrDefault()?.Id + 1 ?? 0, Text = command, Responder = User, ResponderId = User.Id, Question = User.CurrentQuestion, QuestionId = User.CurrentQuestion.Id };
            User.CurrentQuestion.Answers.Add(answer);
            PseudoDB.Answers.Add(answer);
            MessageService?.SendMessageAsync(User.CurrentQuestion.UserId, "На ваш вопрос ответили!");

            //TODO: db AnswerTheQuestion
            return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Ваш ответ добавлен!" };
        }
    }
}
