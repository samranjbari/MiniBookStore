using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace Books.Data.Models
{
    public enum UserRoles
    {
        Admin,
        Member,
        Author
    }

    [Route("/userSession")]
    public class UserModels
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<UserRoles> Role { get; set; }

        public bool IsAdmin
        {
            get
            {
                return Role.Contains(UserRoles.Admin);
            }
        }
    }
}
