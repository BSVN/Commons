using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace BSN.Commons.Users
{
	public enum Roles
	{
		Guest,
		Admin,
		Doctor,
		Secretary
	}

    public class Role : IdentityRole<string, UserRole>
    {
        private static readonly List<Role> Roles = new List<Role>();

        public Role() { }

        private Role(Roles role)
        {
            Name = role.ToString();
        }

        /// <summary>
        /// Factory Method for Creating of a Role Of type x
        /// </summary>
        /// <param name="roles">Requesting Role</param>
        /// <returns>Generated Role</returns>
        public static Role Of(Roles roles)
        {
            foreach (var role in Roles.Where(role => role.Name.Equals(roles.ToString())))
                return role;
            var newRole = new Role(roles);
            Roles.Add(newRole);
            return newRole;
        }
    }
}
