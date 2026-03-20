namespace MyFirstAngularNetApp.Server.Models
{
    public partial class SubmissionStatus: GDCTEntityBase<int>
    {
        public string? StatusName { get; set; }
        public virtual ICollection<Submission>? Submissions { get; set; }
       
    }
}
