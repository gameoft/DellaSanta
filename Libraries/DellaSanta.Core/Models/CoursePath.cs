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

        //One-to-Zero-or-One
        //public virtual Course Address { get; set; }



        ////Foreign key for Standard
        //public int StandardId { get; set; }
        //public Standard Standard { get; set; }

        //public int CurrentGradeId { get; set; }
        //public Grade CurrentGrade { get; set; }

        ////many-to-many
        //public virtual ICollection<Course> Courses { get; set; }
    }
}
