using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellaSanta.Core
{
    public class CoursePath
    {

        public CoursePath()
        {
            //this.Courses = new HashSet<Course>();
        }
        public int CoursePathId { get; set; }
        public string CoursePathName { get; set; }
     
        //public DateTime? DateOfBirth { get; set; }
        //public byte[] Photo { get; set; }
        //public decimal Height { get; set; }
        //public float Weight { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

    
    }
}
