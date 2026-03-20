using Dapper;
using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Models;
using System.Data;
using System.Security.Cryptography;
using static Dapper.SqlMapper;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    /***************************************************************
     * ClassName    : GDCTDataRepository
     * Created By   : Bill Chen 
     * Description  : GDCTData data access class
     * -------------------------------------------------------------
     * History      : 2024-11-26 Initial created - Bill Chen 
     * History      : 2025-02-21 implement new functions - Derrick Yiu
     *                2025-05-28 modify for coding standard - Jet Su
     ***************************************************************/
    public class GDCTDataRepository : IGDCTDataRepository
    {
        private readonly DapperContext _DapperContext;
        private IAppLogger<GDCTDataRepository> _Logger;
        private string _LoggerString="";
        public GDCTDataRepository(DapperContext dapperContext, IAppLogger<GDCTDataRepository> logger)
        {
            _DapperContext = dapperContext;
            _Logger = logger;
        }

        /***************************************************************
          * Function Name: GetGDCTDataAsync
          * Created By   : Derrick Yiu  
          * Description  : Get GDCT data by templateid and orgId
          * parameters   : templateId - Template Id
          *                orgId - Organization Id 
          * -------------------------------------------------------------
          * History      : 2025-03-05 Initial created - Derrick Yiu   
          ***************************************************************/
        public async Task<IEnumerable<TemplateSubmission>> GetGDCTDataAsync(int orgId, int templateId)
        {
            _LoggerString = string.Format("GetGDCTDataAsync({0},{1})", orgId.ToString(), templateId.ToString());
            _Logger.LogInformation(_LoggerString + StringValue.Started); 
            IEnumerable<TemplateSubmission> result = Enumerable.Empty<TemplateSubmission> ();

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<TemplateSubmission>("sp_GetGDCTDataByOrgId",
                                              new { orgId = orgId, templateId = templateId },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
            }
            
            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
          * Function Name: UpdateGDCTDataAsync
          * Created By   : Derrick Yiu  
          * Description  : Get GDCT data by templateid and orgId
          * parameters   : templateId - Template Id
          *                orgId - Organization Id 
          *                gdctJSONString - template Json format string
          * -------------------------------------------------------------
          * History      : 2025-03-05 Initial created - Derrick Yiu   
          ***************************************************************/
        public async Task<string> UpdateGDCTDataAsync(int orgId, int templateId, string gdctJSONString)
        {
            _LoggerString = string.Format("GetGDCTDataAsync({0},{1},JsonString)", orgId.ToString(), templateId.ToString());
            _Logger.LogInformation(_LoggerString +StringValue.Started);
            string result = StringValue.Succeed;

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateGDCT",
                                               new { orgId = orgId, templateId = templateId, gdctJSONString = gdctJSONString },
                                               commandType: CommandType.StoredProcedure, commandTimeout: 0);
                }
            }
            catch (Exception ex)
            {
                result=StringValue.Failed;
                _Logger.LogError(_LoggerString.Replace("Jsonstring", gdctJSONString) + " : " + ex.Message);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: UpdateGDCTTemplateStatusAsync
        * Created By   : Derrick Yiu  
        * Description  : Update Template status by Id
        * parameters   : templateId - tempate Id
        *                templateStatus - template Status 
        *                templateModifier - Update user info
        *                comments - update related info
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<string> UpdateGDCTTemplateStatusAsync(int? templateId, string templateStatus, string templateModifier, string comments)
        {
            _LoggerString = string.Format("UpdateGDCTTemplateStatusAsync({0},{1},{2},{3})", templateId.ToString(),templateStatus,templateModifier,comments);
            _Logger.LogInformation(_LoggerString + StringValue.Started);

            string result = StringValue.Succeed;

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateGDCTTemplateStatus",
                                                new { templateId = templateId, templateStatus = templateStatus, templateModifier = templateModifier, comments = comments },
                                                commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = StringValue.Failed;
                _Logger.LogError(ex.Message);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: UpdateGDCTSubmissionStatusAsync
        * Created By   : Derrick Yiu  
        * Description  : Update Submission status by Id
        * parameters   : submissionId - Submission  Id
        *                submissionStatus - Submission Status 
        *                submissionModifier - Update user info
        *                comments - update related info
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<string> UpdateGDCTSubmissionStatusAsync(int? submissionId, string submissionStatus, string submissionModifier, string comments)
        {
            _LoggerString = string.Format("UpdateGDCTSubmissionStatusAsync({0},{1},{2},{3})", submissionId.ToString(), submissionStatus, submissionModifier, comments);
            _Logger.LogInformation(_LoggerString + StringValue.Started);

            string result = StringValue.Succeed;
            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateGDCTSubmissionStatus",
                                               new { submissionId = submissionId, submissionStatus = submissionStatus, submissionModifier = submissionModifier, comments = comments },
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);

            return result;
        }

        /***************************************************************
        * Function Name: GetConsolidateDataAsync
        * Created By   : Derrick Yiu  
        * Description  : Update Submission status by Id
        * parameters   : templateId - Template  Id
        *                sectorId - Sector Id
        *                orgId -    organization Id
        *                worksheet - Template worksheet name
        *                startDate - Start Date
        *                endDate -   End date
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<IEnumerable<dynamic>> GetConsolidateDataAsync(int templateId, int sectorId, int orgId, string worksheet, DateTime? startDate = null, DateTime? endDate = null)
        {
            _LoggerString = string.Format("GetConsolidateData({0},{1},{2},{3},{4},{5})", templateId.ToString(), sectorId.ToString(), orgId.ToString(), worksheet, startDate.ToString(), endDate.ToString());
            _Logger.LogInformation(_LoggerString + StringValue.Started);

            IEnumerable<dynamic> result = Enumerable.Empty<dynamic>();

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@TemplateId", templateId, DbType.Int32);
                    parameters.Add("@SectorId", sectorId, DbType.Int32);
                    parameters.Add("@OrganizationID", orgId, DbType.Int32);
                    parameters.Add("@Worksheet", worksheet, DbType.String);
                    parameters.Add("@StartDate", startDate, DbType.DateTime);
                    parameters.Add("@EndDate", endDate, DbType.DateTime);

                    // Execute the stored procedure
                    result = await dbConnection.QueryAsync<dynamic>("sp_ExtractGDCTReportData", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: GetWorksheetListAsync
        * Created By   : Derrick Yiu  
        * Description  : Get template worksheet list by Id
        * parameters   : templateId - Template  Id
        *                sectorId - Sector Id
        *                orgId -    organization Id
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<IEnumerable<string>> GetWorksheetListAsync(int templateId, int sectorId, int orgId)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();

            try
            {
                _LoggerString = string.Format("GetWorksheetListAsync({0},{1},{2})", templateId.ToString(), sectorId.ToString(), orgId.ToString());
                _Logger.LogInformation(_LoggerString + StringValue.Started);

                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<string>("sp_GetDistinctWorksheets",
                                          new { templateId = templateId, sectorId = sectorId, organizationID = orgId },
                                          commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while calling GetWorksheetListAsync templateId={templateId}, orgId={orgId}, sectorId={sectorId}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: GetGDCTDataByTemplateAsync
        * Created By   : Derrick Yiu  
        * Description  : Get template worksheet list by Id
        * parameters   : templateId - Template  Id
        *                orgId -    organization Id
        *                worksheet - Sector Id
        *                submitted -  template  submitted date
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<IEnumerable<Gdctdatum>> GetGDCTDataByTemplateAsync(int templateId, int orgId, string worksheet, DateTime? submittedDate = null)
        {
            IEnumerable<Gdctdatum> result = Enumerable.Empty<Gdctdatum>();

            try
            {
                _LoggerString = string.Format("GetGDCTDataByTemplateAsync({0},{1},{2},{3})", templateId.ToString(), orgId.ToString(), worksheet, submittedDate.ToString());
                _Logger.LogInformation(_LoggerString + StringValue.Started);

                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<Gdctdatum>("sp_GetTemplateGDCTData",
                                               new { TemplateId = templateId, OrganizationId = orgId, Worksheet = worksheet, SubmittedDate = submittedDate },
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while GetGDCTDataByTemplateAsync templateId={templateId}, orgId={orgId}, worksheet={worksheet}, submittedDate={submittedDate}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: GetGDCTDataCalculationByTemplateAsync
        * Created By   : Derrick Yiu  
        * Description  : Get template worksheet list by Id
        * parameters   : templateId - Template  Id
        *                orgId -    organization Id
        *                worksheet - Template worksheet name
        *                submittedDate -  submitted date
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<IEnumerable<GdctdataCalculation>> GetGDCTDataCalculationByTemplateAsync(int templateId, int orgId, string worksheet, DateTime? submittedDate = null)
        {
            IEnumerable<GdctdataCalculation> result = Enumerable.Empty<GdctdataCalculation>();

            try
            {
                _LoggerString = string.Format("GetGDCTDataCalculationByTemplateAsync({0}, {1}, {2}, {3})", templateId.ToString(), orgId.ToString(), worksheet, submittedDate.ToString());
                _Logger.LogInformation(_LoggerString + StringValue.Started);

                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<GdctdataCalculation>("sp_GetTemplateGDCTCaculationData",
                                               new { TemplateId = templateId, OrganizationId = orgId, Worksheet = worksheet, SubmittedDate = submittedDate },
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while calling GetGDCTDataCalculationByTemplateAsync templateId={templateId}, orgId={orgId}, worksheet={worksheet}, submittedDate={submittedDate}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
       * Function Name: UpdateGDCTStatusAsync
       * Created By   : Derrick Yiu  
       * Description  : Update GDCTData record status
       * parameters   : id - GDCTData  Id
       *                status -    Status
       *                updatedBy -  Update user info
       * -------------------------------------------------------------
       * History      : 2025-03-05 Initial created - Derrick Yiu   
       ***************************************************************/
        public async Task<string> UpdateGDCTStatusAsync(int id, int status, string updatedBy)
        {
            
            string result = StringValue.Succeed;

            try
            {
                _LoggerString = string.Format("UpdateGDCTStatusAsync({0},{1},{2})", id.ToString(), status, updatedBy);
                _Logger.LogInformation(_LoggerString + StringValue.Started);

                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateGDCTDatastatus",
                                               new { Id = id, Datastatus = status, UpdateBy = updatedBy },
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = StringValue.Failed;
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while calling UpdateGDCTStatusAsync Id={id}, status={status}, updatedBy={updatedBy}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
          * Function Name: UpdateGDCTValueAsync
          * Created By   : Derrick Yiu  
          * Description  : Get template worksheet list by Id
          * parameters   : Id - Template  Id
          *                value -    organization Id
          *                UpdatedBy - Template worksheet name
          * -------------------------------------------------------------
          * History      : 2025-03-05 Initial created - Derrick Yiu   
          ***************************************************************/
        public async Task<string> UpdateGDCTValueAsync(int id, string value, string comments, string updatedBy)
        {
            _LoggerString = string.Format("UpdateGDCTValueAsync({0},{1},{2})", id.ToString(), value, updatedBy);
            _Logger.LogInformation(_LoggerString + StringValue.Started);
            string result = StringValue.Succeed;

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateGDCTDataValue",
                                               new { Id = id, DataValue = value, Comments = comments, UpdateBy = updatedBy },
                                               commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                result = StringValue.Failed;
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while calling UpdateGDCTValueAsync Id={id}, value={value}, comments={comments}, updatedBy ={updatedBy}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed );
            return result;
        }

        /***************************************************************
        * Function Name: GetTemplateAttributeList
        * Created By   : Derrick Yiu  
        * Description  : Get template worksheet list by Id
        * parameters   : templateId - Template  Id
        *                orgId -    organization Id
        *                worksheet - Template worksheet name
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<IEnumerable<dynamic>> GetTemplateAttributeList(int templateId, int orgId, string worksheet)
        {
            _LoggerString = string.Format("GetTemplateAttributeList({0},{1},{2})", templateId.ToString(),  orgId.ToString(), worksheet);
            _Logger.LogInformation(_LoggerString + StringValue.Started);
            IEnumerable<dynamic> result = Enumerable.Empty<dynamic>();

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<dynamic>("sp_GetTemplateAttributeList",
                                              new { TemplateId = templateId, Worksheet = worksheet },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while GetTemplateAttributeList templateId={templateId}, orgId={orgId}, worksheet={worksheet}", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }

        /***************************************************************
        * Function Name: UpdateTemplateDataValue
        * Created By   : Derrick Yiu  
        * Description  : Get template worksheet list by Id
        * parameters   : categoryId - CategoryId 
        *                templateId - Template  Id
        *                orgId -    organization Id
        *                attributeName - Attribute name
        *                dataValue -  Updated data value
        *                comments -  Comments
        *                updatedBy -  updated user info
        * -------------------------------------------------------------
        * History      : 2025-03-05 Initial created - Derrick Yiu   
        ***************************************************************/
        public async Task<string> UpdateTemplateDataValue(int categoryId, int templateId, int orgId, string attributeName, string dataValue, string comments, string updatedBy)
        {
            _LoggerString = string.Format("UpdateTemplateDataValue({0},{1},{2},{3},{4},{5},{6})", categoryId.ToString(), templateId.ToString(),orgId.ToString(), attributeName, dataValue, comments, updatedBy);
            _Logger.LogInformation(_LoggerString + StringValue.Started);
            string result = StringValue.Succeed;

            try
            {
                using (IDbConnection dbConnection = _DapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_UpdateTemplateDataValue",
                                               new { CategoryId = categoryId, TemplateId = templateId, OrgId = orgId, AttributeName = attributeName, DataValue = dataValue, Comments = comments, UpdateBy = updatedBy },
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = StringValue.Failed;
                _Logger.LogError(ex.Message);
                throw new RepositoryException($"An unexpected error occurred while UpdateTemplateDataValue categoryId={categoryId}, templateId ={templateId}, orgId={orgId}, attributeName={attributeName}, dataValue={dataValue},comments={comments}, updatedBy={updatedBy} ", ex);
            }

            _Logger.LogInformation(_LoggerString + StringValue.Completed);
            return result;
        }
    }

}
