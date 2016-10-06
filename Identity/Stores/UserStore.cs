using Data.Contexts;
using Identity.Entities;
using Identity.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Stores
{
    public class UserStore<TKey, TUser, TUserRole, TRoleClaim> :
        IUserStore<TUser,TKey>,
        IUserLoginStore<TUser,TKey>,
        IUserRoleStore<TUser,TKey>,
        IUserClaimStore<TUser,TKey>,
        IUserPasswordStore<TUser,TKey>,
        IUserSecurityStampStore<TUser,TKey>,
        IUserEmailStore<TUser,TKey>,
        IUserLockoutStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>
        //,
        //IUserAuthenticationTokenStore<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        private readonly UnitOfWorkFactory _uowFactory;
        private readonly UserRepository<TKey, TUser, TUserRole, TRoleClaim> _userRepository;

        public UserStore(UserRepository<TKey, TUser, TUserRole, TRoleClaim> userRepository,
            UnitOfWorkFactory uowFactory)
        {
            _userRepository = userRepository;
            _uowFactory = uowFactory;
        }

        public  IQueryable<TUser> Users
        {
            get
            {
                //Impossible to implement IQueryable with Dapper
                return  _userRepository.GetAll();
            }
        }


        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.InsertClaims(user.Id, claims);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

       public  Task AddClaimAsync(TUser user, Claim claim)
        {
            var claims = new List<Claim>();
            claims.Add(claim);
            return AddClaimsAsync(user, claims);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.InsertLoginInfo(user.Id, login);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.AddToRole(user.Id, roleName);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.Insert(user);

                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.Remove(user.Id);

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

        public async Task<TUser> FindByEmailAsync(string normalizedEmail)
        {
            if (string.IsNullOrEmpty(normalizedEmail))
                throw new ArgumentNullException(nameof(normalizedEmail));

            try
            {
                var result = await _userRepository.GetByEmail(normalizedEmail);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TUser> FindByIdAsync(TKey userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            try
            {
                var result = await _userRepository.GetById((TKey)Convert.ChangeType(userId, typeof(TKey)));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            try
            {
                var result = await _userRepository.GetByUserLogin(loginProvider, providerKey);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName)
        {
            if (string.IsNullOrEmpty(normalizedUserName))
                throw new ArgumentNullException(nameof(normalizedUserName));

            try
            {
                var result = await _userRepository.GetByUserName(normalizedUserName);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var result = await _userRepository.GetClaimsByUserId(user.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.LockoutEndDateUtc.Value);
           
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var result = await _userRepository.GetUserLoginInfoById(user.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<string> GetNormalizedEmailAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var result = await _userRepository.GetRolesByUserId(user.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<string> GetTokenAsync(TUser user, string loginProvider, string name)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim)
        {
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            try
            {
                var result = await _userRepository.GetUsersByClaim(claim);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            try
            {
                var result = await _userRepository.GetUsersInRole(roleName);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            try
            {
                var result = await _userRepository.IsInRole(user.Id, roleName);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            var claims = new List<Claim>();
            claims.Add(claim);
            return RemoveClaimsAsync(user, claims);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.RemoveClaims(user.Id, claims);

                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.RemoveFromRole(user.Id, roleName);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(loginProvider))
                throw new ArgumentNullException(nameof(loginProvider));

            if (string.IsNullOrEmpty(providerKey))
                throw new ArgumentNullException(nameof(providerKey));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.RemoveLogin(user.Id, loginProvider, providerKey);

                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task RemoveTokenAsync(TUser user, string loginProvider, string name)
        {
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(TUser user,UserLoginInfo loginInfo)
        {
            return Task.FromResult(0);
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            if (newClaim == null)
                throw new ArgumentNullException(nameof(newClaim));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.UpdateClaim(user.Id, claim, newClaim);
                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.LockoutEndDateUtc = lockoutEnd;

            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Email = normalizedEmail;

            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task SetTokenAsync(TUser user, string loginProvider, string name, string value)
        {
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(TUser user, string userName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.UserName = userName;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _userRepository.Update(user);

                    uow.SaveChanges();
                    return Task.FromResult<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException(nameof(login));
        }
    }
}