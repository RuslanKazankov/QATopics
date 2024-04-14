﻿using QATopics.Models.Database;
using QATopics.Models.Menu;
using QATopics.Models.Menu.Implications;

namespace QATopics.Services.Implications
{
    public class MenuService
    {
        public static BaseMenu GetMenuOfUser(User user, IMessageService? messageService)
        {
            IMenuParams menuParams = new MenuParams(user, messageService);
            return user.CurrentMenu switch
            {
                nameof(MainMenu) => new MainMenu(menuParams),
                nameof(AskQuestionMenu) => new AskQuestionMenu(menuParams),
                nameof(ChangeNameMenu) => new ChangeNameMenu(menuParams),
                nameof(MyQuestionsMenu) => new MyQuestionsMenu(menuParams),
                nameof(QuestionsMenu) => new QuestionsMenu(menuParams),
                nameof(AnswerTheQuestionMenu) => new AnswerTheQuestionMenu(menuParams),
                _ => new MainMenu(menuParams),
            };
        }
        public static BaseMenu GetMenuOfUser(IMenuParams menuParams)
        {
            return menuParams.User.CurrentMenu switch
            {
                nameof(MainMenu) => new MainMenu(menuParams),
                nameof(AskQuestionMenu) => new AskQuestionMenu(menuParams),
                nameof(ChangeNameMenu) => new ChangeNameMenu(menuParams),
                nameof(MyQuestionsMenu) => new MyQuestionsMenu(menuParams),
                nameof(QuestionsMenu) => new QuestionsMenu(menuParams),
                nameof(AnswerTheQuestionMenu) => new AnswerTheQuestionMenu(menuParams),
                _ => new MainMenu(menuParams),
            };
        }
    }
}