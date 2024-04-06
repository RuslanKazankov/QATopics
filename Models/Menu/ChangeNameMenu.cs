﻿using QATopics.Models.MenuCommands;
using QATopics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public class ChangeNameMenu(BotUser botUser) : BaseMenu(botUser)
    {
        public override string GetMenuText()
        {
            return Replicas.ChangeNameText;
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
                CurrentUser.Name = command;
                commandResponse.ResultMessage = "Ваш имя обновлено!";
            }
            return commandResponse;
        }
    }
}
