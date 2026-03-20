
namespace MyFirstAngularNetApp.Server.Models;

public partial class Attribute: GDCTEntityBase<int>
{
    public int AttributeId { get; set; }

    public string AttributeName { get; set; } = null!;

    public virtual ICollection<Gdctdatum>? Gdctdata { get; set; }
    public virtual ICollection<GdctdataCalculation> GdctdataCalculations { get; set; }=new List<GdctdataCalculation>();
    public int? UnitofMeasureId { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool? SumFlag { get; set; }
    public bool? EditFlag { get; set; }
}
