using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseConsoleApp.Models
{
    public class CourseScheduleResponseModel
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public int CourseTimes { get; set; }
        public DateTime Sdate { get; set; }
        public DateTime Edate { get; set; }
    }
}
