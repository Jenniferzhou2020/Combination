using MyFirstAngularNetApp.Server.Models;
using System.Linq.Expressions;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IOrganizationRepository : IGDCTRepository<Organization>
    {
        Task<int> CountOrganizationAsync(Expression<Func<Organization, bool>> predicate = null);
        Task<IEnumerable<Organization>> GetOrganizationBySectorIdAsync(int sectorId);
        Task<bool> IsFacilityNoUniqueAsync(int facilityNo, int orgId);
        Task<IEnumerable<Organization>> GetAllWithSectorAsync(int sectorId);
    }
}
