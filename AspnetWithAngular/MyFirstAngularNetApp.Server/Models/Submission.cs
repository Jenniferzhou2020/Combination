using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstAngularNetApp.Server.Models;

public partial class Submission : GDCTEntityBase<int>
{
    public string? SubmissionName { get; set; }

    public DateOnly? SubmissionEndDate { get; set; }

    public int SubmissionStatus { get; set; }

    public string ? SubmissionStatusName { get; set; }

    public string? SubmissionLocation { get; set; }
    public int TemplateId { get; set; }
    [NotMapped]
    public string? TemplateName { get; set; }

    public int RequestorId { get; set; }
    [NotMapped]
    public string? RequestorName { get; set; }

    public string? Comments { get; set; }

    public int OrgId { get; set; }

    [NotMapped]
    public string? OrganizationName { get; set; } 

    public int? DocumentId { get; set; }

    public int? SubmissionSettingId { get; set; }

    public virtual Organization? Org { get; set; }

    public virtual User? SubmissionRequestor { get; set; } 

    public virtual SubmissionSetting? SubmissionSetting { get; set; }

    public virtual Template? Template { get; set; } 
    public DateTime? SubmittedDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public virtual SubmissionStatus? SubmissionStatusNavigation { get; set; }
}

