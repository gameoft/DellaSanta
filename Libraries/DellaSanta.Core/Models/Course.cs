using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellaSanta.Core
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public virtual User Teacher { get; set; }
        public int TeacherId { get; set; }

        public int CoursePathId { get; set; }
        public virtual CoursePath CoursePath { get; set; }

        public ICollection<EnrolledClass> EnrolledClasses { get; set; }

    }
}
