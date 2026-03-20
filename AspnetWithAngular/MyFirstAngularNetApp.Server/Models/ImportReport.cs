namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ImportReport: GDCTEntityBase<int>
    {
        public string? ImportReportName { get; set; }
        public string? TableName { get; set; }
        public string? Summary { get; set; }
        public string? ImportLogLocation { get; set; }
    }
}
