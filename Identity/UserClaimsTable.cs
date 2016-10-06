using Data.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace Identity
{
    /// <summary>
    /// Class that represents the UserClaims table in the Database
    /// </summary>
    public class UserClaimsTable
    {
        private readonly DbContext _context;

        /// <summary>
        /// Constructor that takes a DbManager instance
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(int memberId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();

            foreach (var c in _context.Query<dynamic>("Select * from MemberClaim where MemberId=@memberId", new { memberId = memberId }))
            {
                claims.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int memberId)
        {
            _context.Execute(@"Delete from MemberClaim where UserId = @memberId", new { memberId = memberId });
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="MemberClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public void Insert(Claim MemberClaim, int memberId)
        {
            _context.Execute(@"Insert into MemberClaim (ClaimValue, ClaimType, MemberId)
                values (@value, @type, @userId)",
                    new
                    {
                        value = MemberClaim.Value,
                        type = MemberClaim.Type,
                        userId = memberId
                    });
        }

        /// <summary>
        /// Deletes a claim from a user
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityMember member, Claim claim)
        {
            _context.Execute(@"Delete from MemberClaim
            where UserId = @memberId and @ClaimValue = @value and ClaimType = @type",
                new
                {
                    memberId = member.Id,
                    ClaimValue = claim.Value,
                    type = claim.Type
                });
        }
    }
}