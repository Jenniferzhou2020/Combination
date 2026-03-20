using MyFirstAngularNetApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IGDCTDataRepository
    {
        Task<IEnumerable<TemplateSubmission>> GetGDCTDataAsync(int orgId, int templateId);
        Task<string> UpdateGDCTDataAsync(int orgId, int templateId, string gdctJSONString);

        Task<string> UpdateGDCTTemplateStatusAsync(int? templateId, string templateStatus, string templateModifier, string comments);

        Task<string> UpdateGDCTSubmissionStatusAsync(int? submissionId, string submissionStatus, string submissionModifier, string comments);
        Task<IEnumerable<dynamic>> GetConsolidateDataAsync(int templateId, int sectorId, int orgId, string worksheet, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<string>> GetWorksheetListAsync(int templateId, int sectorId, int orgId);
        Task<IEnumerable<Gdctdatum>> GetGDCTDataByTemplateAsync(int templateId, int orgId, string worksheet, DateTime? submitedDate = null);
        Task<IEnumerable<GdctdataCalculation>> GetGDCTDataCalculationByTemplateAsync(int templateId, int orgId, string worksheet, DateTime? submitedDate = null);
        Task<string> UpdateGDCTStatusAsync(int id, int status, string updatedBy);
        Task<string> UpdateGDCTValueAsync(int id, string value,string comments, string updatedBy);
        Task<IEnumerable<dynamic>> GetTemplateAttributeList(int templateId, int orgId, string worksheet);
        Task<string> UpdateTemplateDataValue(int categoryId, int templateId, int orgId, string attributeName, string dataValue, string comments, string updatedBy);

    }
}
