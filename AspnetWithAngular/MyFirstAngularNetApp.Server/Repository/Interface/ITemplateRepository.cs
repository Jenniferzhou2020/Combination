using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface ITemplateRepository : IGDCTRepository<Template>
    {
        Task<IEnumerable<Template>> GetAllWithDetailsAsync();
        Task<string?> GetJsonSchemaAsync(int templateId);
        Task TemplatePublish(int templateId, int sectorId, string userEmail, string SubmissionUrl);
        Task<IEnumerable<Template>> GetAllWithSectorAsync(int sectorId);
        Task<IEnumerable<Template>> GetAllWithoutDetailAsync();
    }
}
