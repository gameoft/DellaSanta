using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellaSanta.Core
{
    public class User
    {
        public User()
        {
            Claims = new List<UserClaims>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        
        public virtual ICollection<UserClaims> Claims { get; set; }
        
        public ICollection<EnrolledClass> StudentEnrollments { get; set; }
        public ICollection<Course> CoursesTaught { get; set; }


    }
}
