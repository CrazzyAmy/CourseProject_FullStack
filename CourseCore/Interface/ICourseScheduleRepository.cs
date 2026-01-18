using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Interface
{
    public interface ICourseScheduleRepository
    {
        //Task<IEnumerable<CourseScheduleViewModel>> QueryAsync();
        Task<IEnumerable<CourseScheduleViewModel>> QueryAsync(CourseScheduleQueryCondition? courseScheduleQueryCondition);
    }
}
