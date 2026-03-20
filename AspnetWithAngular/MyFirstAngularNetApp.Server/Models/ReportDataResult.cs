
namespace MyFirstAngularNetApp.Server.Models;

public partial class ReportDataResult
{
    public string OrganizationName { get; set; } = "";
    public string RegionName { get; set; } = "";
    public string CategoryGroupName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string FiscalYearName { get; set; } = "";
    public string ReportingPeriodName { get; set; } = "";
    public string AttributeName { get; set; } = "";
    public decimal DataValue { get; set; }

}
