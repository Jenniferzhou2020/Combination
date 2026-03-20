namespace MyFirstAngularNetApp.Server.Models
{
    public partial class GdctdataCalculation: GDCTEntityBase<int>
    {
        public int CategoryId { get; set; }

        public int AttributeId { get; set; }

        public string? CellLocation { get; set; }

        public string? CellFormula { get; set; }

        public string? DataValue { get; set; }

        public string? Worksheet { get; set; }

        public int TemplateId { get; set; }

        public int OrgId { get; set; }

        public string? Comments { get; set; }

        public virtual Attribute Attribute { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual Organization Org { get; set; } = null!;

        public virtual Template Template { get; set; } = null!;
    }
}
