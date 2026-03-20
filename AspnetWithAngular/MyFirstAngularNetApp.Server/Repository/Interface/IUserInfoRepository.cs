using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IUserInfoRepository : IGDCTRepository<User>
    {
        Task<User> GetUserInfoByEmailAsync(string userEmail);
        Task<User?> GetUserInfoByUserNameAsync(string userName);

        Task<IEnumerable<User>> GetUserListByOrgIdAsync(int orgId);
        Task<bool> IsEmailUniqueAsync(string userEmail, int id);
    }
}
