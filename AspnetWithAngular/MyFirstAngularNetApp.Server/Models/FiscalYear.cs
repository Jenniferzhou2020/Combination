namespace MyFirstAngularNetApp.Server.Models;

public partial class FiscalYear: GDCTEntityBase<int>
{
    public string? FiscalYearName { get; set; } 

    public virtual ICollection<Template>? Templates { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
