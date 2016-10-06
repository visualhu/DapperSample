using Data.Contexts;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole, int>
        where TRole : IdentityRole
    {
        private readonly RoleTable _roleTable;
        private readonly UnitOfWorkFactory _uowFactory;

        public IQueryable<TRole> Roles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Default constructor that initializes a new database
        /// instance using the Default Connection string
        /// </summary>
        //public RoleStore()
        //{
        //    new RoleStore<TRole>(new DbManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
        //}

        /// <summary>
        /// Constructor that takes a dbmanager as argument
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(RoleTable roleTable, UnitOfWorkFactory uowFactory)
        {
            _roleTable = roleTable;
            _uowFactory = uowFactory;
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleTable.Insert(role);
                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleTable.Delete(role.Id);
                    uow.SaveChanges();
                }
                return Task.FromResult<Object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<TRole> FindByIdAsync(int roleId)
        {
            TRole result = _roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = _roleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _roleTable.Update(role);
                    uow.SaveChanges();
                }
                return Task.FromResult<Object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
        }
    }
}