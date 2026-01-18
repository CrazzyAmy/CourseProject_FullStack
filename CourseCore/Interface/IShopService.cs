using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Interface
{
    public interface IShopService
    {
        Task<bool> BookingCourseAsync(Guid userId, Guid courseScheduleId);
        Task<List<StuCourseScheduleModel>> BookingListAsync(Guid userId);
        Task<bool> BookingDeleteAsync(Guid stuScheduleId);
    }
}
