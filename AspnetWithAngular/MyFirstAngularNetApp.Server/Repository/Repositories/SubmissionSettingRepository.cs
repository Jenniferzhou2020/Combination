using Microsoft.EntityFrameworkCore;
using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class SubmissionSettingRepository : GDCTRepository<SubmissionSetting>,  ISubmissionSettingRepository
    {   
        public SubmissionSettingRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<SubmissionSetting> logger) : base(dbcontextfactory, logger)
        {
        }
        public async Task<IEnumerable<SubmissionSetting>> GetAllSubmissionSettingAsync()
        {
            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var result= await ctx.SubmissionSettings.AsNoTracking()
                        .Select(static o => new SubmissionSetting() {
                            SubmissionName =o.SubmissionName,
                            SubmissionReviewerId= o.SubmissionReviewerId,
                            Comments= o.Comments,
                            SubmissionEndDate= o.SubmissionEndDate,
                        Created = o.Created,
                        CreatedBy = o.CreatedBy,
                        
                        EffectiveDate = o.EffectiveDate,
                        ExpiryDate = o.ExpiryDate,
                        Id = o.Id,
                        TemplateId = o.TemplateId,
                            Template =new Template() {
                            Id= o.Template.Id,
                            TemplateName = o.Template.TemplateName ?? string.Empty
                            },
                            SubmissionReviewer = new User() { Id = o.SubmissionReviewer.Id, FullName = o.SubmissionReviewer.FullName },
                        SubmissionApprovers = o.SubmissionApprovers== null ? null : o.SubmissionApprovers.Select(a => new SubmissionApprover() { Id = a.Id, UserId = a.UserId, User=a.User }).ToList(),
                        Status=o.Status
                        }).ToListAsync();
                    return result; 

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving submission settings.");
                throw;
            }
        }

    }
}
