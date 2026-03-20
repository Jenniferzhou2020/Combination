namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportAttributeMapping: GDCTEntityBase<int>
    {
        public string MappingName { get; set; } = null!;

        public string? Description { get; set; }

        public int? DisplayAttributeId { get; set; }

        public string? DisplayAttributeName { get; set; }
               
        public virtual ICollection<ReportAttributeMappingDetail> ReportAttributeMappingDetails { get; set; } = new List<ReportAttributeMappingDetail>();
    }
}
