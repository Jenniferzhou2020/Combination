namespace MyFirstAngularNetApp.Server.Models;

public partial class UnitofMeasure : GDCTEntityBase<int>
{
    
    public string? UnitofMeasureName { get; set; }

    public string? DataType { get; set; }

    public string? ValidationRule { get; set; }

    public virtual ICollection<Category>? Categories { get; set; } 

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
