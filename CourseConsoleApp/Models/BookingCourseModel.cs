using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseConsoleApp.Models
{
    public class BookingCourseRequestModel
    {
        public Guid StuId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
