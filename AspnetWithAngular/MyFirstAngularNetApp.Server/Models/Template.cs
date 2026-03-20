namespace MyFirstAngularNetApp.Server.Models;

//Removed Id as base class contains
public partial class Template : GDCTEntityBase<int>
{
    
    public string? TemplateName { get; set; }

    public string? Jsonschema { get; set; }

    public int FiscalYearId { get; set; }

    public int ReportingPeriodId { get; set; }

    public string? TemplateLocation { get; set; }

    public int? TemplateReviewerId { get; set; }

    public int? TemplateApproverId { get; set; }

    public int TemplateStatus { get; set; }
    public int? RequestorId { get; set; }

    public int? SectorId { get; set; }

    public int? DocumentId { get; set; }

    public virtual FiscalYear? FiscalYear { get; set; }
    
    public virtual  ReportingPeriod? ReportingPeriod { get; set; }

    public virtual User? TemplateReviewer { get; set; } 

    public virtual User? TemplateApprover { get; set; } 

    public virtual User? Requestor { get; set; }

    public virtual Sector? Sector { get; set; }
    
    public virtual ICollection<Gdctdatum>? Gdctdata { get; set; }
    public virtual ICollection<GdctdataCalculation> GdctdataCalculations { get; set; } = new List<GdctdataCalculation>();
    public virtual ICollection<SubmissionSetting>? SubmissionSettings { get; set; } 

    public virtual ICollection<Submission>? Submissions { get; set; } 

    public virtual ICollection<LookupMapping>? LookupMappings { get; set; } 
    public virtual ICollection<PublishLog>? PublishLogs { get; set; } 
    public DateTime? TemplateSubmittedDate { get; set; }
    public DateTime? TemplateApprovalDate { get; set; }
    public string? ReviewerComments { get; set; }
    public string? ApproverComments { get; set; }

    public virtual TemplateStatus? TemplateStatusNavigation { get; set; }
}
