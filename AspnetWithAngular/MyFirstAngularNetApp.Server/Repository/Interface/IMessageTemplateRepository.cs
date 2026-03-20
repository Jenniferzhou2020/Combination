using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstAngularNetApp.Server.Models;
using MessageTemplate = MyFirstAngularNetApp.Server.Models.MessageTemplate;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface IMessageTemplateRepository : IGDCTRepository<MessageTemplate>
    {
        Task<MessageTemplate> GetTemplateByKeyAsync (string key);
    }
}
