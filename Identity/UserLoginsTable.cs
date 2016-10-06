using Data.Contexts;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Identity
{
    /// <summary>
    /// Class that represents the UserLogins table in the Database
    /// </summary>
    public class UserLoginsTable
    {
        private readonly DbContext _context;

        /// <summary>
        /// Constructor that takes a DbManager instance
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTable(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityMember member, UserLoginInfo login)
        {
            _context.Execute(@"Delete from MemberLogin
                    where UserId = @userId
                    and LoginProvider = @loginProvider
                    and ProviderKey = @providerKey",
                new
                {
                    userId = member.Id,
                    loginProvider = login.LoginProvider,
                    providerKey = login.ProviderKey
                });
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int userId)
        {
            _context.Execute(@"Delete from MemberLogin
                    where UserId = @userId", new { userId = userId });
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public void Insert(IdentityMember member, UserLoginInfo login)
        {
            _context.Execute(@"Insert into MemberLogin
                (LoginProvider, ProviderKey, UserId)
                values (@loginProvider, @providerKey, @userId)",
                    new
                    {
                        loginProvider = login.LoginProvider,
                        providerKey = login.ProviderKey,
                        userId = member.Id
                    });
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="MemberLogin">The user's login info</param>
        /// <returns></returns>
        public int FindUserIdByLogin(UserLoginInfo MemberLogin)
        {
            return _context.ExecuteScalar<int>(@"Select UserId from MemberLogin
                where LoginProvider = @loginProvider and ProviderKey = @providerKey",
                        new
                        {
                            loginProvider = MemberLogin.LoginProvider,
                            providerKey = MemberLogin.ProviderKey
                        });
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(int memberId)
        {
            return _context.Query<UserLoginInfo>("Select * from MemberLogin where MemberId = @memberId", new { memberId = memberId })
                .ToList();
        }
    }
}