using CourseCore.Interface;
using CourseCore.Models;
using CourseDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataAccess.Repository
{
    public class CourseScheduleRepository : ICourseScheduleRepository
    {
        private readonly KhNetCourseContext _dbContext;
        public CourseScheduleRepository(KhNetCourseContext khNetCourseContext)
        {
            _dbContext = khNetCourseContext;
        }

        //public async Task<IEnumerable<CourseScheduleViewModel>> QueryAsync()
        //{
        //    var query = from cs in _dbContext.Courseschedules
        //                join c in _dbContext.Courses on cs.Courseid equals c.Id
        //                join t in _dbContext.Teachers on cs.Teacherid equals t.Id
        //                select new CourseScheduleViewModel()
        //                {
        //                    Id = cs.Id,
        //                    CourseCode = c.Code,
        //                    CourseName = c.Name,
        //                    CourseDesc = c.Description,
        //                    CourseTimes = c.Times,
        //                    TeacherName = t.Name,
        //                    Sdate = cs.Sdate,
        //                    Edate = cs.Edate,
        //                    Location = cs.Location
        //                };

        //    return await query.ToListAsync();
        //}


        public async Task<IEnumerable<CourseScheduleViewModel>> QueryAsync(CourseScheduleQueryCondition? courseScheduleQueryCondition)
        {
            var query = from cs in _dbContext.Courseschedules
                        join c in _dbContext.Courses on cs.Courseid equals c.Id
                        join t in _dbContext.Teachers on cs.Teacherid equals t.Id
                        select new CourseScheduleViewModel()
                        {
                            Id = cs.Id,
                            CourseCode = c.Code,
                            CourseName = c.Name,
                            CourseDesc = c.Description,
                            CourseTimes = c.Times,
                            TeacherName = t.Name,
                            Sdate = cs.Sdate,
                            Edate = cs.Edate,
                            Location = cs.Location
                        };
            if (courseScheduleQueryCondition != null)
            {
                if (courseScheduleQueryCondition.Id != Guid.Empty)
                {
                    query = query.Where(c => c.Id== courseScheduleQueryCondition.Id);
                }
                if (!string.IsNullOrEmpty(courseScheduleQueryCondition.CourseName))
                {
                    query = query.Where(c => c.CourseName.Contains(courseScheduleQueryCondition.CourseName));
                }
                if (!string.IsNullOrEmpty(courseScheduleQueryCondition.TeacherName))
                {
                    query = query.Where(t => t.TeacherName.Contains(courseScheduleQueryCondition.TeacherName));
                }
            }
            return await query.ToListAsync();
        }

    }
}
