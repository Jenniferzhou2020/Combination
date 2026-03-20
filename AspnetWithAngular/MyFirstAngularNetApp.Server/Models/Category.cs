namespace MyFirstAngularNetApp.Server.Models;

public partial class Category: GDCTEntityBase<int>
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? CategoryGroupId { get; set; }

    public int? ParentCategoryId { get; set; }

    public virtual Category? ParentCategory { get; set; }

    public int? UnitofMeasureId { get; set; }

    public virtual CategoryGroup? CategoryGroup { get; set; }

    public virtual ICollection<Gdctdatum>? Gdctdata { get; set; }
    public virtual ICollection<GdctdataCalculation> GdctdataCalculations { get; set; } = new List<GdctdataCalculation>();
    public virtual UnitofMeasure? UnitofMeasure { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public bool? SumFlag { get; set; }
    public bool? UseAttributeMeasure { get; set; }
    public bool? EditFlag { get; set; }
}
