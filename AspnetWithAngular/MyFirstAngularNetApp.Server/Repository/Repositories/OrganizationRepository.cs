using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Models;
using MyFirstAngularNetApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class OrganizationRepository : GDCTRepository<Organization>, IOrganizationRepository
    {

        public OrganizationRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<Organization> logger) : base(dbcontextfactory, logger)
        {
        }

        /// <summary>
        /// Counts the number of organizations based on the provided predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException"></exception>
        public async Task<int> CountOrganizationAsync(Expression<Func<Organization, bool>>? predicate = null)
        {
            int iCount = 0;

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    if (predicate is null)
                    {
                        iCount = await ctx.Organizations.CountAsync();
                    }
                    else
                    {
                        iCount = await ctx.Organizations.CountAsync(predicate);
                    }

                    return iCount;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while counting organizations.");
                throw new RepositoryException("An unexpected error occurred while counting organizations.", ex);
            }
        }

        /// <summary>
        /// Retrieves organizations associated with a specific sector ID using a stored procedure.
        /// </summary>
        /// <param name="sectorId"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException"></exception>
        public async Task<IEnumerable<Organization>> GetOrganizationBySectorIdAsync(int sectorId)
        {
            var organizations = new List<Organization>();

            try
            {
                if (sectorId > 0)
                {
                    using (var ctx = _dbcontextfactory.CreateDbContext())
                    {
                        organizations = await ctx.Organizations
                            .FromSqlInterpolated($"EXEC sp_GetOrganizationsBySectorId @SectorId={sectorId}")
                            .ToListAsync();
                    }
                }

                return organizations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching organizations by SectorId: {SectorId}", sectorId);
                throw new RepositoryException($"An unexpected error occurred while fetching organizations by SectorId for sector ID {sectorId}.", ex);
            }
        }

        public async Task<IEnumerable<Organization>> GetAllWithSectorAsync(int sectorId)
        {
            string logMethod = nameof(GetAllWithSectorAsync);

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    var organizations = await ctx.Organizations
                        .Where(o => o.SectorId == sectorId && o.Status == RecordStatus.Active)
                        .Select(t => new Organization
                        {
                            Id = t.Id,
                            OrganizationName = t.OrganizationName,
                            SectorId = sectorId,
                            Status = RecordStatus.Active // t.Status
                        })
                        .OrderBy(o => o.OrganizationName)
                        .AsNoTracking()
                        .ToListAsync();

                    if (organizations == null)
                    {
                        _logger.LogWarning("{Method}: No organizations found (result is null).", logMethod);
                        organizations = new List<Organization>();
                    }

                    _logger.LogInformation("{Method}: Successfully retrieved {Count} organizations.", logMethod, organizations.Count);
                    return organizations;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving templates with details on MyFirstAngularNetApp.Server.Repository.Repositories.OrganizationRepository.GetAllWithSectorAsync().");
                throw new RepositoryException("An unexpected error occurred on MyFirstAngularNetApp.Server.Repository.Repositories.OrganizationRepository.GetAllWithSectorAsync().", ex);
            }
        }

        /// <summary>
        /// Checks if a facility number is unique within the organization context.
        /// </summary>
        /// <param name="facilityNo"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        /// <exception cref="RepositoryException"></exception>
        public async Task<bool> IsFacilityNoUniqueAsync(int facilityNo, int orgId)
        {
            bool bIsUnique = false;
            var organizations = new List<Organization>();

            try
            {
                if (facilityNo > 0)
                {
                    using (var ctx = _dbcontextfactory.CreateDbContext())
                    {
                        organizations = await ctx.Organizations
                            .FromSqlInterpolated($"EXEC sp_GetOrganizationsByFacilityNoOrgId @FacilityNo={facilityNo}, @OrgId={orgId}")
                            .ToListAsync();
                    }
                }

                if ((organizations != null) && organizations.Any())
                {
                    // If the list is not empty, it means the facility number is not unique
                    bIsUnique = false;
                }
                else
                {
                    // If the list is empty, it means the facility number is unique
                    bIsUnique = true;
                }

                return bIsUnique;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching organizations by SectorId: {facilityNo}", facilityNo);
                throw new RepositoryException($"An unexpected error occurred while fetching organizations by facilityNo for facilityNo {facilityNo}.", ex);
            }

        }
    }
}
