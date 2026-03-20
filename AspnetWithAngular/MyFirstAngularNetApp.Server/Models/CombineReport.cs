
namespace MyFirstAngularNetApp.Server.Models
{
    public partial class CombineReport: GDCTEntityBase<int>
    {
        public string ReportName { get; set; } = "";

        public int? SectorId { get; set; }

    }
}
