using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IReportRepository
    {
        Task<IEnumerable<Models.Attribute>> GetTemplateAttributeList(int templateId, string worksheet);
        Task<IEnumerable<Category>> GetTemplateCategoryList(int templateId, string worksheet);
        Task<IEnumerable<string>> GetDistinctWorksheets(int templateId, int sectorId, int organizationId);

        Task<IEnumerable<ReportDataResult>?> GetReportDataAsync(int reportId,int sectorId, int organizationId, DateTime? startDate, DateTime? endDate);

    }

}