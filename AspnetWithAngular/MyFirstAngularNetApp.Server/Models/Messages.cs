
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class Messages: MessageBase
    {
        
        public List<User> ToUserList { get; set; } = new List<User>();

        public string ToUserIds()
        {
            string result = string.Empty;
            ToUserList.ForEach(x => result = result + x.Id.ToString() + ",");
            return result;
        }
    }

}
