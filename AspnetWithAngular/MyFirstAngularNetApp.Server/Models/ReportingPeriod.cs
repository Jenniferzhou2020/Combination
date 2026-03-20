namespace MyFirstAngularNetApp.Server.Models;

public partial class ReportingPeriod : GDCTEntityBase<int>
{
    public string ReportingPeriodName { get; set; } = null!;

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
