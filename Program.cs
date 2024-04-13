﻿using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using QATopics.Helpers;
using QATopics.Resources;
using QATopics.Models;
using QATopics.Models.MenuCommands;
using Telegram.Bot.Types.ReplyMarkups;
using QATopics.Models.Menu;
using QATopics.Services;
using QATopics.Models.Database;

namespace QATopics
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(Config.TelegramBotToken);

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            TelegramMessageService.GetInstance().Init(botClient);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            Message? message = update.Message;
            if (message == null) return;

            // Only process text messages
            var messageText = message.Text;
            if (messageText == null) return;

            var chatId = message.Chat.Id;
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            //Registration
            Models.Database.User? user = PseudoDB.Users.Where((user) => user.Id == chatId).FirstOrDefault();
            bool registration = false;
            if (user == null)
            {
                registration = true;
                user = new Models.Database.User() { Id = chatId, CurrentMenu = nameof(MainMenu), Name = "Аноним" };
                PseudoDB.Users.Add(user);
                //TODO: db.SaveChanges();
                await botClient.SendTextMessageAsync(chatId: chatId, text: Replicas.WelcomeText,
                            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
            }

            //Use bot
            BaseMenu currentMenu = MenuService.GetMenuOfUser(user);

            if (!registration)
            {
                CommandResponse? commandResponse = currentMenu.SendCommand(messageText);
                if (commandResponse == null)
                {
                    await botClient.SendTextMessageAsync(chatId: chatId, text: Replicas.ErrorCommandText,
                            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
                }
                else
                {
                    if (commandResponse.ResultMessage != null)
                    {
                        await botClient.SendTextMessageAsync(chatId: chatId, text: commandResponse.ResultMessage,
                            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
                    }
                    currentMenu = commandResponse.NextMenu;
                    user.CurrentMenu = currentMenu.GetNameOfMenu();
                }
            }
            await botClient.SendTextMessageAsync(chatId: chatId, text: currentMenu.GetMenuText(), 
                replyMarkup: currentMenu.GetRelplyKeyboard(), cancellationToken: cancellationToken);
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
