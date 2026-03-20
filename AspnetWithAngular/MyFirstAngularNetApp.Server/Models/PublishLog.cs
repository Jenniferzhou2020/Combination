namespace MyFirstAngularNetApp.Server.Models
{
    public partial class PublishLog: GDCTEntityBase<int>
    {
        public int TemplateId { get; set; }

        public int OrgId { get; set; }

        public DateTime PublishDate { get; set; }

        public string? PublishedBy { get; set; }

        public int SubmissionId { get; set; }
        public virtual Organization? Org { get; set; } 
        public virtual Template? Template { get; set; } 
    }
}
