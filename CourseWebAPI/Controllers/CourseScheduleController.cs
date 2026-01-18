using CourseCore.Interface;
using CourseCore.Models;
using CourseCore.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseScheduleController : ControllerBase
    {
        private readonly ICourseScheduleService _scheduleService;
        public CourseScheduleController(ICourseScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        //url: api/CourseSchedule
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseScheduleViewModel>>> GetCourseSchedule()
        {
            var result = await _scheduleService.QueryAsync(null);
            return Ok(result);
        }

        // url : api/CourseSchedule/f4f2846a-39e5-4d50-a148-064c9f44c26e
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseScheduleViewModel>> GetCourseById(Guid id)
        {
            var result = await _scheduleService.QueryAsync(new CourseScheduleQueryCondition() { Id = id });

            if (result.Count() > 0)
            {
                return Ok(result.First());
            }

            return NotFound();
        }

    }
}
