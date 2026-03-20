namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportAttributeMappingDetail: GDCTEntityBase<int>
    {
        public int ReportAttributeMappingId { get; set; }

        public int CombineReportId { get; set; }

        public int? TemplateId { get; set; }

        public int? AttributeId { get; set; }

        public virtual Attribute? Attribute { get; set; }

        public virtual CombineReport CombineReport { get; set; } = null!;

        public virtual ReportAttributeMapping ReportAttributeMapping { get; set; } = null!;

        public virtual Template? Template { get; set; }
    }
}
