using CourseCore.Interface;
using CourseCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CourseWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public StudentController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // URL:api/student/12345678
        [HttpPut("{stuId}")]
        public async Task<ActionResult<UserReguestModel>> ChangeStudentInfo(Guid stuId, [FromBody] UserReguestModel user)
        {
            if (stuId != user.Id)
            {
                return BadRequest(new { message = "invalid request model" });
            }

            await _memberService.MemberInfoUpdateAsync(
                new UserInfoReqModel()
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Mobile = user.Phone
                });

            return Ok(user);
        }
    }
}
