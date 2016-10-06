using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace Identity.Entities
{
    public class IdentityUser : IdentityUser<int>
    {
        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }

    public class IdentityUser<TKey> : IdentityUser<TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityLogin<TKey>>
        where TKey : IEquatable<TKey>
    {
        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }

    public class IdentityUser<TKey, TUserClaim, TUserRole, TUserLogin>:IUser<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityLogin<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public IdentityUser()
        {
            // Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTimeOffset? LockoutEndDateUtc { get; set; }

        public virtual ICollection<TUserLogin> Logins { get; } = new List<TUserLogin>();

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password
        /// changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        public string UserName { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}