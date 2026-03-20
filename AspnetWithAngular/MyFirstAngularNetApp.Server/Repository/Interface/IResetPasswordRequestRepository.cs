using MyFirstAngularNetApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IResetPasswordRequestRepository :   IGDCTRepository<ResetPasswordRequest>
    {
        Task<IEnumerable<ResetPasswordRequest>> GetRequestListAsync(string email);

        Task<string> GetEmailFromRequestCode(string code);
    }
}
