using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Data;
using MyFirstAngularNetApp.Server.Models;
using Dapper;
using System.Data;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DapperContext _dapperContext;
        private IAppLogger<NotificationRepository> _logger;

        public NotificationRepository(DapperContext dapperContext, IAppLogger<NotificationRepository> logger)
        {
            _dapperContext = dapperContext;
            _logger = logger;

        }
        //sp_GetUnreadMessageListforAllUser
        public async Task<IEnumerable<MessageUser>> GetAllUserUnreadMessagesAsync()
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {
                    return await dbConnection.QueryAsync<MessageUser>("sp_GetUnreadMessageListforAllUser",
                                            commandType: CommandType.StoredProcedure,commandTimeout:300);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        //sp: sp_CreateMessage
        public async Task<Guid> CreateNewMessage(Messages message)
        {
            try
            {
                using (var dbConnection = _dapperContext.CreateConnection())
                {


                  return await dbConnection.QuerySingleAsync<Guid>("sp_CreateMessage", new
                    {
                        Subject = message.Subject,
                        Body = message.Body,
                        FromUserId = message.FromUserId,
                        MessageTypeId= message.MessageTypeId,
                        ChannelId= message.ChannelId,
                        ToUserIdList=message.ToUserIds()
                    }, commandType: CommandType.StoredProcedure);

                  
                }
            }
            catch(Exception ex) {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        //sp: sp_GetMessageUserData
        public async Task<IEnumerable<MessageUser>> GetCurrentUserMessages(int userId)
        {
            IEnumerable<MessageUser> result = new List<MessageUser>();
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<MessageUser>("sp_GetMessageUserData",
                                              new { userId = userId },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;

        }

        //sp: sp_GetUnreadMessageUserData
        public async Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessages(int userId)
        {
            IEnumerable<MessageUser> result = new List<MessageUser>();
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<MessageUser>("sp_GetUnreadMessageUserData",
                                              new { userId = userId },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }
        //sp: sp_GetUnreadMessageListByEmail

        public async Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessagesByEmail(string email)
        {
            IEnumerable<MessageUser> result = new List<MessageUser>();
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<MessageUser>("sp_GetUnreadMessageListByEmail",
                                              new { Email = email },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }

        public async Task<IEnumerable<MessageUser>> GetCurrentUserUnreadMessagesByUserName(string userName)
        {             
            IEnumerable<MessageUser> result = new List<MessageUser>();
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryAsync<MessageUser>("sp_GetUnreadMessageListByUserName",
                                              new { UserName = userName },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return result;
        }

        //sp: sp_MarkMessageRead, parameter: UserId/ MessageId
        public async Task MarkMessagesAsRead(Guid messageId, int userId)
        {
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    await dbConnection.ExecuteAsync("sp_MarkMessageRead",
                                              new { UserId = userId, MessageId = messageId },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        
        //sp: sp_CountUnreadMessageUser
        public async Task<int> CountCurrentUserUnreadMessages(int userId)
        {
            int result = 0;
            try
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    result = await dbConnection.QueryFirstAsync<int>("sp_CountUnreadMessageUser",
                                              new { userId = userId },
                                              commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }

    }
}
