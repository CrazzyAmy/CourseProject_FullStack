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
    public class StuCourseScheduleRepository : IStuCourseScheduleRepository
    {
        private readonly KhNetCourseContext _dbContext;
        public StuCourseScheduleRepository(KhNetCourseContext khNetCourseContext)
        {
            _dbContext = khNetCourseContext;
        }

        public async Task<bool> AddScheduleAsync(Guid userId, Guid courseScheduleId)
        {
            var entity = new Stucourseschedule()
            {
                Id = Guid.NewGuid(),
                Studentid = userId,
                Cscheduleid = courseScheduleId
            };

            await _dbContext.Stucourseschedules.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsExistScheduleAsync(Guid userId, Guid courseScheduleId)
        {
            var entity = await _dbContext.Stucourseschedules.FirstOrDefaultAsync(
                  d => d.Studentid == userId
                  && d.Cscheduleid == courseScheduleId);

            return (entity is null ? false : true);
        }

        public async Task<List<StuCourseScheduleModel>> GetScheduleAsync(Guid userId)
        {
            var query = from stucs in _dbContext.Stucourseschedules
                        join cs in _dbContext.Courseschedules on stucs.Cscheduleid equals cs.Id
                        join c in _dbContext.Courses on cs.Courseid equals c.Id
                        join t in _dbContext.Teachers on cs.Teacherid equals t.Id
                        where stucs.Studentid == userId
                        select new StuCourseScheduleModel
                        {
                            Id = stucs.Id,
                            CourseName = c.Name,
                            TeacherName = t.Name,
                            Sdate = cs.Sdate,
                            Edate = cs.Edate
                        };
            return await query.ToListAsync();
        }

        public async Task<bool> DeleteScheduleAsync(Guid stuScheduleId)
        {
            var entity = await _dbContext.Stucourseschedules.FirstOrDefaultAsync(s => s.Id == stuScheduleId);

            if (entity != null)
            {
                _dbContext.Stucourseschedules.Remove(entity);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
