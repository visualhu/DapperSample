using Data.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Identity
{
    /// <summary>
    /// Class that represents the UserRoles table in the Database
    /// </summary>
    public class UserRolesTable
    {
        private readonly DbContext _context;

        /// <summary>
        /// Constructor that takes a DbManager instance
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(int memberId)
        {
            return _context.Query<string>("Select Role.Name from MemberRole, Role where MemberRole.MemberId=@MemberId and MemberRole.RoleId = Role.Id", new { MemberId = memberId })
                .ToList();
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int memberId)
        {
            _context.Execute(@"Delete from MemberRole where Id = @MemberId", new { MemberId = memberId });
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public void Insert(IdentityMember member, int roleId)
        {
            _context.Execute(@"Insert into AspNetUserRoles (UserId, RoleId) values (@userId, @roleId",
                new { userId = member.Id, roleId = roleId });
        }
    }
}