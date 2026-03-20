using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface INotificationRepository
    {
        Task<Guid> CreateNewMessage(Messages message);
        Task<IEnumerable<MessageUser>> GetCurrentUserMessages(int userId);
        Task<IEnumerable<MessageUser>> GetAllUserUnreadMessagesAsync();
        Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessages(int userId);
        Task MarkMessagesAsRead(Guid messageId, int userId);
        Task<int> CountCurrentUserUnreadMessages(int userId);
        Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessagesByEmail(string email);
        Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessagesByUserName(string userName);
    }

}
