using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Models;
using MyFirstAngularNetApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class SubmissionRepository : GDCTRepository<Submission>, ISubmissionRepository
    {
        
        public SubmissionRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<Submission> logger) : base(dbcontextfactory, logger)
        {
        }

        public async Task<IEnumerable<Submission>> GetAllWithDetailsAsync()
        {
            var submissions = new List<Submission>();

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var conn = ctx.Database.GetDbConnection();
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "sp_GetSubmissions";
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var submission = new Submission
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    SubmissionName = reader["SubmissionName"] as string,
                                    SubmissionStatus = reader["SubmissionStatus"] != DBNull.Value ? Convert.ToInt32(reader["SubmissionStatus"]) : 1,
                                    SubmissionStatusName= reader["SubmissionStatusName"] as string,
                                    SubmissionEndDate = reader["SubmissionEndDate"] != DBNull.Value ? (DateOnly?)DateOnly.FromDateTime(Convert.ToDateTime(reader["SubmissionEndDate"])) : null,
                                    SubmissionLocation = reader["SubmissionLocation"] as string,
                                    TemplateId = reader["TemplateId"] != DBNull.Value ? Convert.ToInt32(reader["TemplateId"]) : 0,
                                    TemplateName = reader["TemplateName"] as string,
                                    RequestorId = reader["RequestorId"] != DBNull.Value ? Convert.ToInt32(reader["RequestorId"]) : 0,
                                    RequestorName = reader["RequestorName"] as string,
                                    Comments = reader["Comments"] as string,
                                    OrgId = reader["OrgId"] != DBNull.Value ? Convert.ToInt32(reader["OrgId"]) : 0,
                                    OrganizationName = reader["OrganizationName"] as string,
                                    DocumentId = reader["DocumentId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["DocumentId"]) : null,
                                    SubmissionSettingId = reader["SubmissionSettingId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SubmissionSettingId"]) : null,
                                    Created = reader["Created"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Created"]) : null,
                                    CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : string.Empty,
                                    Modified = reader["Modified"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Modified"]) : null,
                                    ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : string.Empty,
                                    Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 1,
                                    
                                };
                                submissions.Add(submission);
                            }
                        }
                    }
                }
                return submissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all submissions with details.");
                throw new RepositoryException("An unexpected error occurred while fetching all submissions with details.", ex);
            }
        }

        /// <summary>
        /// Retrieves submissions associated with a specific organization ID using a stored procedure.
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException"></exception>
        public async Task<IEnumerable<Submission>> GetSubmissionsByOrgIdAsync(int orgId)
        {
            var submissions = new List<Submission>();

            //try
            //{
            //    if (orgId > 0)
            //    {
            //        using (var ctx = _dbcontextfactory.CreateDbContext())
            //        {
            //            submissions = await ctx.Submissions
            //                .FromSqlInterpolated($"EXEC sp_GetSubmissionsByOrgId @OrgId={orgId}")
            //                .ToListAsync();
            //        }
            //    }

            //    return submissions;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occurred while fetching submissions by OrgId: {OrgId}", orgId);
            //    throw new RepositoryException($"An unexpected error occurred while fetching submissions by orgId for orgId {orgId}.", ex);
            //}
            try
            {
                if (orgId > 0)
                {
                    using (var ctx = _dbcontextfactory.CreateDbContext())
                    {
                        var conn = ctx.Database.GetDbConnection();
                        await conn.OpenAsync();

                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = "sp_GetSubmissionsByOrgId";
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@OrgId", orgId));

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var submission = new Submission
                                    {
                                        Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                        SubmissionName = reader["SubmissionName"] as string,
                                        SubmissionStatus = reader["SubmissionStatus"] != DBNull.Value ? Convert.ToInt32(reader["SubmissionStatus"]) : 1,
                                        SubmissionEndDate = reader["SubmissionEndDate"] != DBNull.Value ? (DateOnly?)DateOnly.FromDateTime(Convert.ToDateTime(reader["SubmissionEndDate"])) : null,
                                        SubmissionLocation = reader["SubmissionLocation"] as string,
                                        TemplateId = reader["TemplateId"] != DBNull.Value ? Convert.ToInt32(reader["TemplateId"]) : 0,
                                        TemplateName = reader["TemplateName"] as string,
                                        RequestorId = reader["RequestorId"] != DBNull.Value ? Convert.ToInt32(reader["RequestorId"]) : 0,
                                        RequestorName = reader["RequestorName"] as string,
                                        Comments = reader["Comments"] as string,
                                        OrgId = reader["OrgId"] != DBNull.Value ? Convert.ToInt32(reader["OrgId"]) : 0,
                                        OrganizationName = reader["OrganizationName"] as string,
                                        DocumentId = reader["DocumentId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["DocumentId"]) : null,
                                        SubmissionSettingId = reader["SubmissionSettingId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SubmissionSettingId"]) : null,
                                        Created = reader["Created"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Created"]) : null,
                                        CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : string.Empty,
                                        Modified = reader["Modified"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Modified"]) : null,
                                        ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? reader["ModifiedBy"].ToString() : string.Empty,
                                        Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 1,

                                    };
                                    submissions.Add(submission);
                                }
                            }
                        }
                    }
                }
                return submissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching submissions by OrgId: {OrgId}", orgId);
                throw new RepositoryException($"An unexpected error occurred while fetching submissions by orgId for orgId {orgId}.", ex);
            }
        }
    }
}
