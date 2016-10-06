using Data.Contexts;
using Identity.Entities;
using Identity.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Stores
{
    public class RoleStore<TKey, TRole, TUserRole, TRoleClaim>
        : IRoleStore<TRole, TKey>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        private readonly RoleRepository<TKey, TRole, TUserRole, TRoleClaim> _roleRepository;
        private readonly UnitOfWorkFactory _uowFactory;

        public RoleStore(RoleRepository<TKey, TRole, TUserRole, TRoleClaim> roleRepository,
            UnitOfWorkFactory uowFactory)
        {
            _roleRepository = roleRepository;
            _uowFactory = uowFactory;
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleRepository.Insert(role);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleRepository.Remove(role.Id);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public void Dispose()
        {
        }

        public async Task<TRole> FindByIdAsync(TKey roleId)
        {
            if (roleId == null)
                throw new ArgumentNullException(nameof(roleId));

            try
            {
                var result = await _roleRepository.GetById((TKey)Convert.ChangeType(roleId, typeof(TKey)));

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName)
        {
            if (string.IsNullOrEmpty(normalizedRoleName))
                throw new ArgumentNullException(nameof(normalizedRoleName));

            try
            {
                var result = await _roleRepository.GetByName(normalizedRoleName);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (role.Id.Equals(default(TKey)))
                return null;

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (role.Id.Equals(default(TKey)))
                return null;

            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(TRole role, string roleName)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            role.Name = roleName;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleRepository.Update(role);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
    }
}