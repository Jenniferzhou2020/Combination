using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface ISubmissionRepository : IGDCTRepository<Submission>
    {
        Task<IEnumerable<Submission>> GetAllWithDetailsAsync();

        Task<IEnumerable<Submission>> GetSubmissionsByOrgIdAsync(int orgId);
    }
}
