namespace MyFirstAngularNetApp.Server.Models;

public partial class User : GDCTEntityBase<int>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; } 

    public string? FullName { get; set; }
    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }    
    public string? Email { get; set; } 

    public int UserRoleId { get; set; }

    public int? OrgId { get; set; }

    public virtual ICollection<SubmissionApprover>? SubmissionApprovers { get; set; }

    public virtual ICollection<SubmissionSetting>? SubmissionSettings { get; set; } 

    public virtual ICollection<Template>? TemplateReviewers { get; set; } 
    
    public virtual ICollection<Template>? TemplateApprovers { get; set; } 
    
    public virtual ICollection<Template>? Requestors { get; set; } 
    
    public virtual ICollection<Submission>? SubmissionRequestors { get; set; } 

    public virtual UserRole? UserRole { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();


}
