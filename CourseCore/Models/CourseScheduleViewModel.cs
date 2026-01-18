using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Models
{
    public class CourseScheduleViewModel
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string CourseDesc { get; set; }
        public int CourseTimes { get; set; }
        public DateOnly Sdate { get; set; }
        public DateOnly Edate { get; set; }
        public string Location { get; set; }
    }
}
