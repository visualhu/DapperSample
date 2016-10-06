using Identity.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public interface IRoleRepository<TKey, TRole, TUserRole, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        void Insert(TRole role);

        void Remove(TKey id);

        void Update(TRole role);

        Task<TRole> GetById(TKey id);

        Task<TRole> GetByName(string roleName);
    }
}