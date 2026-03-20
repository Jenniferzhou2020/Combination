namespace MyFirstAngularNetApp.Server.Models;

public partial class Gdctdatum : GDCTEntityBase<int>
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int AttributeId { get; set; }

    public string? AttributeName { get; set; }

    public string? DataValue { get; set; }

    public string? Worksheet { get; set; }

    public int TemplateId { get; set; }

    public int OrgId { get; set; }
    public string? Comments { get; set; }
    public string? CellLocation { get; set; }
    public virtual Attribute GDCTAttribute { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Template Template { get; set; } = null!;

    public virtual Organization Organization { get; set; } = null!;
}
