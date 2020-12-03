using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSN.Commons.Users
{
	public class User : IdentityUser<string, UserLogin, UserRole, UserClaim>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Gender Gender { get; set; }
		public string FullName => FirstName + " " + LastName;


		public User()
		{
			Id = Guid.NewGuid().ToString();
		}

		public User(string firstName, string lastName, string email, Gender gender) : this()
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			UserName = email;
			Gender = gender;
			SecurityStamp = Guid.NewGuid().ToString();
		}

		public User(string firstName, string lastName, string email, string mobileNumber, Gender gender)
			: this(firstName, lastName, email, gender)
		{
			PhoneNumber = mobileNumber;
		}


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager)
		{
			return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
		}
	}
}