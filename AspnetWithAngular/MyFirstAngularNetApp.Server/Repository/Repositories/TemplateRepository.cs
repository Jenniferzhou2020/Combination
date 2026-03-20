using MyFirstAngularNetApp.Server.Models;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class TemplateRepository : GDCTRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<Template> logger) : base(dbcontextfactory, logger)
        {
        }

        /// <summary>
        /// Retrieves all templates with their details, excluding the Jsonschema field.
        /// </summary>
        /// <returns>List of templates</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Template>> GetAllWithDetailsAsync()
        {
            string logMethod = nameof(GetAllWithDetailsAsync);

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var templates = await ctx.Templates
                    .Select(t => new Template
                    {
                        Id = t.Id,
                        TemplateName = t.TemplateName,
                        FiscalYearId = t.FiscalYearId,
                        ReportingPeriodId = t.ReportingPeriodId,
                        TemplateLocation = t.TemplateLocation,
                        TemplateReviewerId = t.TemplateReviewerId,
                        TemplateApproverId = t.TemplateApproverId,
                        TemplateStatus = t.TemplateStatus,
                        RequestorId = t.RequestorId,
                        SectorId = t.SectorId,
                        DocumentId = t.DocumentId,
                        Jsonschema = null, // Exclude Jsonschema for this query
                        CreatedBy = t.CreatedBy,
                        Created = t.Created,
                        ModifiedBy = t.ModifiedBy,
                        Modified = t.Modified,
                        ReviewerComments = t.ReviewerComments,
                        ApproverComments = t.ApproverComments,
                        TemplateSubmittedDate = t.TemplateSubmittedDate,
                        TemplateApprovalDate = t.TemplateApprovalDate,
                        Status = t.Status
                    })
                    .ToListAsync();

                    if (templates == null)
                    {
                        _logger.LogWarning("{Method}: No templates found (result is null).", logMethod);
                        templates = new List<Template>();
                    }

                    _logger.LogInformation("{Method}: Successfully retrieved {Count} templates.", logMethod, templates.Count);
                    return templates;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates with details on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllWithDetailsAsync().");
                throw new RepositoryException("An unexpected error occurred on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllWithDetailsAsync().", ex);
            }
        }


        public async Task<IEnumerable<Template>> GetAllWithoutDetailAsync()
        {

            string logMethod = nameof(GetAllWithoutDetailAsync);

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var templates = await ctx.Templates
                        .Where(t => t.Status == RecordStatus.Active)
                        .Select(t => new Template
                        {
                            Id = t.Id,
                            TemplateName = t.TemplateName,
                            Status = RecordStatus.Active // t.Status
                        })
                        .OrderBy(t => t.TemplateName)
                        .AsNoTracking()
                        .ToListAsync();

                    if (templates == null)
                    {
                        _logger.LogWarning("{Method}: No templates found (result is null).", logMethod);
                        templates = new List<Template>();
                    }

                    _logger.LogInformation("{Method}: Successfully retrieved {Count} templates.", logMethod, templates.Count);
                    return templates;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates with details on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllNameOnlyWithSectorAsync().");
                throw new RepositoryException("An unexpected error occurred on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllNameOnlyWithSectorAsync().", ex);
            }
        }
        public async Task<IEnumerable<Template>> GetAllWithSectorAsync(int sectorId)
        {
            string logMethod = nameof(GetAllWithSectorAsync);

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var templates = await ctx.Templates
                        .Where(t => t.SectorId == sectorId && t.Status == RecordStatus.Active)
                        .Select(t => new Template
                        {
                            Id = t.Id,
                            TemplateName = t.TemplateName,
                            SectorId = sectorId,
                            Status = RecordStatus.Active // t.Status
                        })
                        .OrderBy(t => t.TemplateName)
                        .AsNoTracking()
                        .ToListAsync();

                    if (templates == null)
                    {
                        _logger.LogWarning("{Method}: No templates found (result is null).", logMethod);
                        templates = new List<Template>();
                    }

                    _logger.LogInformation("{Method}: Successfully retrieved {Count} templates.", logMethod, templates.Count);
                    return templates;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates with details on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllNameOnlyWithSectorAsync().");
                throw new RepositoryException("An unexpected error occurred on MyFirstAngularNetApp.Server.Repository.Repositories.GetAllNameOnlyWithSectorAsync().", ex);
            }
        }

        /// <summary>  
        /// Retrieves the JSON schema for a specific template by its ID.  
        /// </summary>  
        /// <param name="templateId"></param>  
        /// <returns>string of JSON schema</returns>  
        /// <exception cref="Exception"></exception>  
        public async Task<string?> GetJsonSchemaAsync(int templateId)
        {
            string logMethod = nameof(GetJsonSchemaAsync);
            string? jsonSchema = null; // Use nullable type  

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    jsonSchema = await ctx.Set<Template>()
                             .Where(t => t.Id == templateId)
                             .Select(t => t.Jsonschema) // Jsonschema may be null in DB  
                             .FirstOrDefaultAsync();

                    if (jsonSchema == null)
                    {
                        _logger.LogWarning("{Method}: No Jsonschema found for TemplateId={TemplateId}.", logMethod, templateId);
                    }
                    else
                    {
                        _logger.LogInformation("{Method}: Successfully retrieved Jsonschema for TemplateId={TemplateId}.", logMethod, templateId);
                    }
                }

                return jsonSchema; // Nullable type ensures no CS8603 warning  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving JSON schema for template with ID {TemplateId} on MyFirstAngularNetApp.Server.Repository.Repositories.GetJsonSchemaAsync().", templateId);
                throw new RepositoryException($"An unexpected error occurred while retrieving the JSON schema for template ID {templateId}.", ex);
            }
        }

        /// <summary>
        /// Publishes a GDCT Template and send out message
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="sectorId"></param>
        /// <param name="userEmail"></param>
        /// <param name="SubmissionUrl"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task TemplatePublish(int templateId, int sectorId, string userEmail, string submissionUrl)
        {
            string logMethod = nameof(GetAllWithDetailsAsync);
            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var conn = ctx.Database.GetDbConnection();
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "sp_PublishTemplate";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@TemplateId", templateId));
                        command.Parameters.Add(new SqlParameter("@SectorID", sectorId));
                        command.Parameters.Add(new SqlParameter("@UserEmail", userEmail));
                        command.Parameters.Add(new SqlParameter("@SubmissionUrl", submissionUrl));
                        await command.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogInformation("{Method}: Successfully published template with TemplateId={TemplateId}.", logMethod, templateId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing template with ID {TemplateId} on {logMethod}.", templateId, logMethod);
                throw new RepositoryException($"An unexpected error occurred while publishing template with template ID {templateId}.", ex);
            }
        }
    }
}
