using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public class AnswerTheQuestionMenu(User user) : BaseMenu(user)
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
            CommandResponse commandResponse = new CommandResponse(new MainMenu(CurrentUser));
            if (command != "Назад")
            {
                Answer answer = new Answer() { Text = command };
                PseudoDB.Answers.Add(answer);
                TelegramMessageService.GetInstance().SendMessageToUser(CurrentUser.CurrentQuestion.UserId, answer.Text);

                //TODO: db AnswerTheQuestion
                commandResponse.ResultMessage = "Ваш ответ добавлен!";
            }
            return commandResponse;
        }
    }
}
