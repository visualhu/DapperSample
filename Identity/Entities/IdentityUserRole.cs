using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Entities
{
    public class IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey UserId { get; set; }
        public virtual TKey RoleId { get; set; }
    }
}