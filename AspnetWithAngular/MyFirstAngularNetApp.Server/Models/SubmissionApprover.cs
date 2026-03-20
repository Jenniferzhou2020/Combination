namespace MyFirstAngularNetApp.Server.Models;

public partial class SubmissionApprover : GDCTEntityBase<int>
{
    
    public int UserId { get; set; }

    public int SubmissionSettingId { get; set; }

    public virtual SubmissionSetting? SubmissionSetting { get; set; }
    public virtual User? User { get; set; }
}
