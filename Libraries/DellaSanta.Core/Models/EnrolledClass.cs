using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellaSanta.Core
{
    public class EnrolledClass
    {
        //public int EnrolledClassId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }

        public DateTime? ExamDate { get; set; }
        public int? ExamGrade { get; set; }

        public int StudentId { get; set; }
        public virtual User Student { get; set; }
      
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
