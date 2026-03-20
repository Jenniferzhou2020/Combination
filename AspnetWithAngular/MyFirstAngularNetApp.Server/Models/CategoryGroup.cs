namespace MyFirstAngularNetApp.Server.Models;

public partial class CategoryGroup : GDCTEntityBase<int>
{
    public string? CategoryGroupName { get; set; }

    public int? ParentGroupId { get; set; }

    public virtual ICollection<Category>? Categories { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public bool? SubtotalFlag { get; set; }
}
