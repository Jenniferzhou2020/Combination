
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportExcludeAttribute: GDCTEntityBase<int>
    {
        public int? AttributeId { get; set; }
        public string? Comments { get; set; }

    }
}
