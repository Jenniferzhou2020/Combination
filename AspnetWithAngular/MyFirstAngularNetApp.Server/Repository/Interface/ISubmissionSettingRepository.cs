
using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface ISubmissionSettingRepository :   IGDCTRepository<SubmissionSetting>
    {
        Task<IEnumerable<SubmissionSetting>> GetAllSubmissionSettingAsync();
    }
}
