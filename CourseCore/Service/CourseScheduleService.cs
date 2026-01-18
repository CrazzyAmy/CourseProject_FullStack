using CourseCore.Interface;
using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Service
{
    public class CourseScheduleService : ICourseScheduleService
    {
        private readonly ICourseScheduleRepository _courseScheduleRepository;

        public CourseScheduleService(ICourseScheduleRepository courseScheduleRepository)
        {
            _courseScheduleRepository = courseScheduleRepository;
        }

        //public async Task<List<CourseScheduleViewModel>> QueryAsync()
        //{
        //    var result = await _courseScheduleRepository.QueryAsync();

        //    return result.ToList();
        //}

        public async Task<List<CourseScheduleViewModel>> QueryAsync(CourseScheduleQueryCondition? courseScheduleQueryCondition)
        {
            var result = await _courseScheduleRepository.QueryAsync(courseScheduleQueryCondition);

            return result.ToList();
        }
    }
}
