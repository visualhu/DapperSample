using Identity.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public interface IUserRepository<TKey, TUser, TUserRole, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        void AddToRole(TKey id, string roleName);

        IQueryable<TUser> GetAll();

        Task<TUser> GetByEmail(string email);

        Task<TUser> GetById(TKey id);

        Task<TUser> GetByUserLogin(string loginProvider, string providerKey);

        Task<TUser> GetByUserName(string userName);

        Task<IList<Claim>> GetClaimsByUserId(TKey id);

        Task<IList<string>> GetRolesByUserId(TKey id);

        Task<IList<UserLoginInfo>> GetUserLoginInfoById(TKey id);

        Task<IList<TUser>> GetUsersByClaim(Claim claim);

        Task<IList<TUser>> GetUsersInRole(string roleName);

        void Insert(TUser user);

        void InsertClaims(TKey id, IEnumerable<Claim> claims);

        void InsertLoginInfo(TKey id, UserLoginInfo loginInfo);

        Task<bool> IsInRole(TKey id, string roleName);

        void Remove(TKey id);

        void RemoveClaims(TKey id, IEnumerable<Claim> claims);

        void RemoveFromRole(TKey id, string roleName);

        void RemoveLogin(TKey id, string loginProvider, string providerKey);

        void Update(TUser user);

        void UpdateClaim(TKey id, Claim oldClaim, Claim newClaim);
    }
}