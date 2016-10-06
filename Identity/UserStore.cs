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
    /// Class that implements the key ASP.NET Identity user store iterfaces
    /// </summary>
    public class UserStore :
        Microsoft.AspNet.Identity.IUserStore<IdentityMember, int>
    //where TUser : IdentityMember
    {
        private readonly UserTable<IdentityMember> _userTable;
        private readonly RoleTable _roleTable;
        private readonly UserRolesTable _userRolesTable;
        private readonly UserClaimsTable _userClaimsTable;
        private readonly UserLoginsTable _userLoginsTable;
        private readonly UnitOfWorkFactory _uowFactory;

        public IQueryable<IdentityMember> Users
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
        public UserStore()
        {
            //new UserStore<TUser>(new DbManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
        }

        /// <summary>
        /// Constructor that takes a dbmanager as argument
        /// </summary>
        /// <param name="database"></param>
        public UserStore(UserTable<IdentityMember> userTable, RoleTable roleTable, UserRolesTable userRolesTable, UserClaimsTable userClaimsTable, UserLoginsTable userLoginsTable, UnitOfWorkFactory uowFactory)
        {
            _userTable = userTable;
            _roleTable = roleTable;
            _userRolesTable = userRolesTable;
            _userClaimsTable = userClaimsTable;
            _userLoginsTable = userLoginsTable;
            _uowFactory = uowFactory;
        }

        /// <summary>
        /// Insert a new TUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(IdentityMember user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userTable.Insert(user);
                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns an TUser instance based on a userId query
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<IdentityMember> FindByIdAsync(int userId)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    throw new ArgumentException("Null or empty argument: userId");
            //}

            IdentityMember result = _userTable.GetUserById(userId) as IdentityMember;
            if (result != null)
            {
                return Task.FromResult<IdentityMember>(result);
            }

            return Task.FromResult<IdentityMember>(null);
        }

        /// <summary>
        /// Returns an TUser instance based on a userName query
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<IdentityMember> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            List<IdentityMember> result = _userTable.GetUserByName(userName) as List<IdentityMember>;

            // Should I throw if > 1 user?
            if (result != null && result.Count == 1)
            {
                return Task.FromResult<IdentityMember>(result[0]);
            }

            return Task.FromResult<IdentityMember>(null);
        }

        /// <summary>
        /// Updates the UsersTable with the TUser instance values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(IdentityMember user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userTable.Update(user);

                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts a claim to the UserClaimsTable for the given user
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="claim">Claim to be added</param>
        /// <returns></returns>
        public Task AddClaimAsync(IdentityMember user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userClaimsTable.Insert(claim, user.Id);

                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns all claims for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(IdentityMember user)
        {
            ClaimsIdentity identity = _userClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        }

        /// <summary>
        /// Removes a claim froma user
        /// </summary>
        /// <param name="user">User to have claim removed</param>
        /// <param name="claim">Claim to be removed</param>
        /// <returns></returns>
        public Task RemoveClaimAsync(IdentityMember user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userClaimsTable.Delete(user, claim);
                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts a Login in the UserLoginsTable for a given User
        /// </summary>
        /// <param name="user">User to have login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public Task AddLoginAsync(IdentityMember user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userLoginsTable.Insert(user, login);
                    uow.SaveChanges();
                }
                return Task.FromResult<object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns an TUser based on the Login info
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<IdentityMember> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = _userLoginsTable.FindUserIdByLogin(login);
            if (userId > 0)
            {
                IdentityMember user = _userTable.GetUserById(userId) as IdentityMember;
                if (user != null)
                {
                    return Task.FromResult<IdentityMember>(user);
                }
            }

            return Task.FromResult<IdentityMember>(null);
        }

        /// <summary>
        /// Returns list of UserLoginInfo for a given IdentityMember
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityMember user)
        {
            List<UserLoginInfo> userLogins = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<UserLoginInfo> logins = _userLoginsTable.FindByUserId(user.Id);
            if (logins != null)
            {
                return Task.FromResult<IList<UserLoginInfo>>(logins);
            }

            return Task.FromResult<IList<UserLoginInfo>>(null);
        }

        /// <summary>
        /// Deletes a login from UserLoginsTable for a given IdentityMember
        /// </summary>
        /// <param name="user">User to have login removed</param>
        /// <param name="login">Login to be removed</param>
        /// <returns></returns>
        public Task RemoveLoginAsync(IdentityMember user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userLoginsTable.Delete(user, login);
                    uow.SaveChanges();
                }
                return Task.FromResult<Object>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts a entry in the UserRoles table
        /// </summary>
        /// <param name="user">User to have role added</param>
        /// <param name="roleName">Name of the role to be added to user</param>
        /// <returns></returns>
        public Task AddToRoleAsync(IdentityMember user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            int roleId = _roleTable.GetRoleId(roleName);
            if (roleId > 0)
            {
                try
                {
                    using (var uow = _uowFactory.Create())
                    {
                        _userRolesTable.Insert(user, roleId);
                        uow.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            //if (!string.IsNullOrEmpty(roleId))
            //{
            //    userRolesTable.Insert(user, roleId);
            //}

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns the roles for a given IdentityMember
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(IdentityMember user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<string> roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null)
                {
                    return Task.FromResult<IList<string>>(roles);
                }
            }

            return Task.FromResult<IList<string>>(null);
        }

        /// <summary>
        /// Verifies if a user is in a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<bool> IsInRoleAsync(IdentityMember user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            List<string> roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null && roles.Contains(role))
                {
                    return Task.FromResult<bool>(true);
                }
            }

            return Task.FromResult<bool>(false);
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(IdentityMember user, string role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityMember user)
        {
            if (user != null)
            {
                try
                {
                    using (var uow = _uowFactory.Create())
                    {
                        _userTable.Delete(user);
                        uow.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given IdentityMember
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(IdentityMember user)
        {
            string passwordHash = _userTable.GetPasswordHash(user.Id);

            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(IdentityMember user)
        {
            var hasPassword = !string.IsNullOrEmpty(_userTable.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(Boolean.Parse(hasPassword.ToString()));
        }

        /// <summary>
        /// Sets the password hash for a given IdentityMember
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(IdentityMember user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        ///  Set security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(IdentityMember user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(IdentityMember user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Set email on user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task SetEmailAsync(IdentityMember user, string email)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.Email = email;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get email from user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(IdentityMember user)
        {
            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Get if user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(IdentityMember user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Set when user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(IdentityMember user, bool confirmed)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.EmailConfirmed = confirmed;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<IdentityMember> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            IdentityMember result = _userTable.GetUserByEmail(email).FirstOrDefault();
            if (result != null)
            {
                return Task.FromResult<IdentityMember>(result);
            }

            return Task.FromResult<IdentityMember>(null);
        }

        /// <summary>
        /// Set user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(IdentityMember user, string phoneNumber)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.PhoneNumber = phoneNumber;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(IdentityMember user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Get if user phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityMember user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Set phone number if confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(IdentityMember user, bool confirmed)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.PhoneNumberConfirmed = confirmed;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Set two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(IdentityMember user, bool enabled)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.TwoFactorEnabled = enabled;
                    _userTable.Update(user);

                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get if two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(IdentityMember user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Get user lock out end date
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityMember user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        /// <summary>
        /// Set user lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(IdentityMember user, DateTimeOffset lockoutEnd)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Increment failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(IdentityMember user)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.AccessFailedCount++;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(user.AccessFailedCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Reset failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(IdentityMember user)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.AccessFailedCount = 0;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(IdentityMember user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get if lockout is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(IdentityMember user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Set lockout enabled for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(IdentityMember user, bool enabled)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    user.LockoutEnabled = enabled;
                    _userTable.Update(user);
                    uow.SaveChanges();
                }
                return Task.FromResult(0);
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