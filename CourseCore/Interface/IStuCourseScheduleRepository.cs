using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseCore.Models;

namespace CourseCore.Interface
{
    public interface IStuCourseScheduleRepository
    {
        Task<bool> IsExistScheduleAsync(Guid userId, Guid courseScheduleId);
        Task<bool> AddScheduleAsync(Guid userId, Guid courseScheduleId);
        Task<List<StuCourseScheduleModel>> GetScheduleAsync(Guid userId);
        Task<bool> DeleteScheduleAsync(Guid stuScheduleId);
    }
}
