

namespace MyFirstAngularNetApp.Server.Models
{
    public class MessageBase  
    {
        public Guid MessageId { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public required int FromUserId { get; set; }
        public MessageType MessageTypeId { get; set; }
        public MessageChannel ChannelId { get; set; }
        public DateTime? Created { get; set; }
    }

    public enum MessageType
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Success = 4
    }

    public enum MessageStatus
    {
        Inactive = 0,
        Active = 1
    }
    public enum MessageChannel
    {
        Email = 1,
        InApp = 2,
        EmailInApp = 3
    }
}
