
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ResetPasswordRequest :  GDCTEntityBase<int>
    {
        public string Email { get;set; } = "";
        public string RequestCode { get;set; } = "";
    }
}
