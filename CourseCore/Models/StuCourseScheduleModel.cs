using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Models
{
    public class StuCourseScheduleModel
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateOnly Sdate { get; set; }
        public DateOnly Edate { get; set; }
    }
}
