namespace QATopics.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(long userId, string message);
    }
}
