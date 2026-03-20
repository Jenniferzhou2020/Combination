namespace MyFirstAngularNetApp.Server.Models
{
    public partial class MessageUser : MessageBase
    {
        public int MessageUserId { get; set; }
        public int ToUserId { get; set; }  //be current context identity user
        public string ToUserEmail { get; set; } = "";
        public string fromUserName { get; set; } = "";  
        public bool IsRead { get; set; }
        public DateTime? Modified { get; set; }
        
    }
}
