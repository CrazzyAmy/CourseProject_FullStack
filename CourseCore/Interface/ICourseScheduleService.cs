using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseCore.Models;

namespace CourseCore.Interface
{
    public interface ICourseScheduleService
    {
        //Task<List<CourseScheduleViewModel>> QueryAsync();
        Task<List<CourseScheduleViewModel>> QueryAsync(CourseScheduleQueryCondition? courseScheduleQueryCondition);
    }
}
