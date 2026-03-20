
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class MessageTemplate: GDCTEntityBase<int>
    {
        public string MessageTemplateKey { get; set; } = "";

        public string Subject { get; set; } = "";

        public string Body { get; set; } = "";

        public string? Comment { get; set; }

        public MessageChannel ChannelId { get; set; }
    }
}
