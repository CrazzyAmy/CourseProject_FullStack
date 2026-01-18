using CourseCore.Interface;
using CourseCore.Service;
using CourseWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IShopService _shopService;
        public BookingController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BookingCourseRequestModel>> BookingCourse(BookingCourseRequestModel requestModel)
        {
            var result = await _shopService.BookingCourseAsync(requestModel.StuId, requestModel.ScheduleId);
            if (!result)
            {
                return BadRequest(new { errorCode = "E01", message = "登記課程失敗" });
            }
            return Ok(requestModel);
        }

        //path : api/booking/123456
        [Authorize]
        [HttpDelete("{studentScheduleId}")]
        public async Task<IActionResult> DeleteCourse(Guid studentScheduleId)
        {
            var result = await _shopService.BookingDeleteAsync(studentScheduleId);

            if (!result)
            {
                return BadRequest(new { errorCode = "E02", message = "取消課程失敗" });
            }
            return Ok();
        }
    }

}

