using Microsoft.EntityFrameworkCore;
using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MessageTemplate = MyFirstAngularNetApp.Server.Models.MessageTemplate;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class MessageTemplateRepository : GDCTRepository<MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<MessageTemplate> logger) : base(dbcontextfactory, logger)
        {
        }

        public async Task<MessageTemplate> GetTemplateByKeyAsync(string key)
        {
            using (var ctx = _dbcontextfactory.CreateDbContext())
            {
                return await ctx.Set<MessageTemplate>().Where(x => x.MessageTemplateKey == key).FirstOrDefaultAsync();
            }
        }
    }
}
