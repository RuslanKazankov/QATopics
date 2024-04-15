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
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("Назад")
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            CommandResponse commandResponse = new CommandResponse(new MainMenu(this));
            if (command != "Назад")
            {
                Answer answer = new Answer() { Id = PseudoDB.Answers.LastOrDefault()?.Id + 1 ?? 0, Text = command, ResponderId = User.Id, Question = User.CurrentQuestion, QuestionId = User.CurrentQuestion.Id };
                PseudoDB.Answers.Add(answer);
                MessageService?.SendMessageAsync(User.CurrentQuestion.UserId, "На ваш вопрос ответили!");

                //TODO: db AnswerTheQuestion
                commandResponse.ResultMessage = "Ваш ответ добавлен!";
            }
            return commandResponse;
        }
    }
}
