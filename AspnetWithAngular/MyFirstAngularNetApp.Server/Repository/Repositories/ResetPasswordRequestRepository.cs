using Microsoft.EntityFrameworkCore;
using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class ResetPasswordRequestRepository : GDCTRepository<ResetPasswordRequest>, IResetPasswordRequestRepository
    {
        public ResetPasswordRequestRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<ResetPasswordRequest> logger) : base(dbcontextfactory, logger)
        {
        }

        public async Task<IEnumerable<ResetPasswordRequest>> GetRequestListAsync(string email)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                return await ctx.Set<ResetPasswordRequest>()
                .Where(x => x.Email == email && x.Created > DateTime.Now.AddMinutes(-200))
                .ToListAsync();
            }
        }

        public async Task<string> GetEmailFromRequestCode(string code)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                var request = await ctx.Set<ResetPasswordRequest>()
                .FirstOrDefaultAsync(x => x.RequestCode == code && x.Created > DateTime.Now.AddMinutes(-200));
                return request?.Email ?? string.Empty;
            }
        }
    }
}
