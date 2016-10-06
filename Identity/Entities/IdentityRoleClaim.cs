using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Entities
{
    public class IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {
        public virtual int Id { get; set; }
        public virtual TKey RoleId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }

        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}