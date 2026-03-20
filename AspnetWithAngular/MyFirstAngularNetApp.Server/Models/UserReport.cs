
namespace MyFirstAngularNetApp.Server.Models;

public partial class UserReport : GDCTEntityBase<int>
{
    public string ReportName { get; set; } = null!;
    public string ReportDescription { get; set; } = null!;
    public string ReportLayout { get; set; } = null!;
    public int CombineReportId { get; set; }
    public int UserId { get; set; }
    
}
