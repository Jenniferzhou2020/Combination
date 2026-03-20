namespace MyFirstAngularNetApp.Server.Models
{

    public partial class SubmissionSetting : GDCTEntityBase<int>
    {
        
        public string? SubmissionName { get; set; }

        public int TemplateId { get; set; }
        public Template Template { get; set; } = default!;

        public int SubmissionReviewerId { get; set; }

        public string? Comments { get; set; }

        public DateOnly? SubmissionEndDate { get; set; }

        public virtual ICollection<SubmissionApprover>? SubmissionApprovers { get; set; }

        public virtual User? SubmissionReviewer { get; set; }

        public virtual ICollection<Submission>? Submissions { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
