using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.Menu;
using QATopics.Models.Menu.Implications;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using QATopics.Services;
using QATopics.Services.Implications;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics
{
    public class Program
    {
        private static IMessageService? messageService;
        private static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(Config.TelegramBotToken);
            messageService = new TelegramMessageService(botClient);

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
            if (registration = user == null)
            {
                user = new Models.Database.User() { Id = chatId, CurrentMenu = nameof(MainMenu), Name = "Аноним" };
                if (chatId == Config.AdminChatId)
                {
                    RoleService.DoAdmin(chatId);
                }
                PseudoDB.Users.Add(user);
                //TODO: db.SaveChanges();
                await botClient.SendTextMessageAsync(chatId: chatId, text: Replicas.WelcomeText,
                            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
            }

            //Use bot
            BaseMenu currentMenu = MenuService.GetMenuOfUser(user, messageService);

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
