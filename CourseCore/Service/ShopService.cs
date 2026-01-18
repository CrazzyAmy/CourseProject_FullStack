using CourseCore.Interface;
using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Service
{
    public class ShopService : IShopService
    {
        private readonly IStuCourseScheduleRepository _stuCourseScheduleRepository;

        public ShopService(IStuCourseScheduleRepository stuCourseScheduleRepository)
        {
            _stuCourseScheduleRepository = stuCourseScheduleRepository;
        }
        public async Task<bool> BookingCourseAsync(Guid userId, Guid courseScheduleId)
        {
            //驗證重覆購課
            if (await _stuCourseScheduleRepository.IsExistScheduleAsync(userId, courseScheduleId))
            {
                return false;
            }

            //寫入DB
            return await _stuCourseScheduleRepository.AddScheduleAsync(userId, courseScheduleId);
        }

        public async Task<bool> BookingDeleteAsync(Guid stuScheduleId)
        {
            return await _stuCourseScheduleRepository.DeleteScheduleAsync(stuScheduleId);
        }

        public async Task<List<StuCourseScheduleModel>> BookingListAsync(Guid userId)
        {
            return await _stuCourseScheduleRepository.GetScheduleAsync(userId);
        }
    }
}
