using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Models;
using Dapper;
using System.Data;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DapperContext _dapperContext;
        private IAppLogger<ReportRepository> _logger;

        public ReportRepository(DapperContext dapperContext, IAppLogger<ReportRepository> logger)
        {
            _dapperContext = dapperContext;
            _logger = logger;

        }


        public async Task<IEnumerable<Models.Attribute>> GetTemplateAttributeList(int templateId, string worksheet)
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {
                    return await dbConnection.QueryAsync<Models.Attribute>("sp_GetTemplateAttributeList",
                                            new { TemplateId = templateId,  Worksheet = worksheet},
                                            commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<IEnumerable<Category>> GetTemplateCategoryList(int templateId, string worksheet)
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {
                    return await dbConnection.QueryAsync<Category>("sp_GetTemplateCategoryList",
                                            new { TemplateId = templateId,  Worksheet = worksheet},
                                            commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<IEnumerable<string>> GetDistinctWorksheets(int templateId, int sectorId, int organizationId)
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {
                    return await dbConnection.QueryAsync<string>("sp_GetDistinctWorksheets",
                                            new { TemplateId = templateId,  SectorId = sectorId, OrganizationId = organizationId},
                                            commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<ReportDataResult>> GetReportDataAsync(int reportId, int sectorId, int organizationId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {
                    return await dbConnection.QueryAsync<ReportDataResult>("sp_GetCombineReport",
                                            new { ReportId = reportId, SectorID = sectorId, OrganizationId = organizationId, StartDate = startDate, EndDate = endDate },
                                            commandType: CommandType.StoredProcedure, commandTimeout:60);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
