using Data.Contexts;
using Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public class RoleRepository<TKey, TRole, TUserRole, TRoleClaim> : IRoleRepository<TKey, TRole, TUserRole, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        private readonly DbContext _context;

        public RoleRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<TRole> GetById(TKey id)
        {
            return await _context.QueryFirstOrDefaultAsync<TRole>("select * from IdentityRole where id=@id", new { id = id });
        }

        public async Task<TRole> GetByName(string roleName)
        {
            return await _context.QueryFirstOrDefaultAsync<TRole>("select * from IdentityRole where Name like '%@id%'", new { Name = roleName });
        }

        public void Insert(TRole role)
        {
            _context.Execute("insert into IdentityRole values (@Name)", role);
        }

        public void Remove(TKey id)
        {
            _context.Execute("delete from IdentityRole where id=@id", new { id = id });
        }

        public void Update(TRole role)
        {
            _context.Execute("update IdentityRole set name=@Name where id=@Id", role);
        }
    }
}