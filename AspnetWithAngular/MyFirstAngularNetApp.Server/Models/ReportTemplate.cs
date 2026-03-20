
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportTemplate: GDCTEntityBase<int>
    {
        public int CombineReportId { get; set; }

        public int TemplateId { get; set; }

        public string WorkSheet { get; set; } = null!;

        public int OrderNumber { get; set; }

    }
}
