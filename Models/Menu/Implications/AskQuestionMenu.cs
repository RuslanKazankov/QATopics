﻿using QATopics.Models.Database;
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
    public class AskQuestionMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetNameOfMenu()
        {
            return nameof(AskQuestionMenu);
        }
        public override string GetMenuText()
        {
            return Replicas.AskQuestionText;
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
                Question question = new Question() { User = User, Text = command, UserId = User.Id };
                User.Questions.Add(question);
                PseudoDB.Questions.Add(question);
                //TODO: db asqQuestion
                commandResponse.ResultMessage = "Ваш вопрос добавлен!";
            }
            return commandResponse;
        }
    }
}