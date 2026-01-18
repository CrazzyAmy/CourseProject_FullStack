using CourseCore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CourseWebAPI.Models;

namespace CourseWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private IMemberService _memberService;
        private JwtHelper _jwtHelper;
        public LoginController(IMemberService memberService, JwtHelper jwtHelper)
        {
            _memberService = memberService;
            _jwtHelper = jwtHelper;
        }


        //nuget : Microsoft.AspNetCore.Authentication.JwtBearer
        // url : api/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> SignIn(LoginRequestModel loginRequestModel)
        {
            var user = await _memberService.MemberSignAsync(loginRequestModel.Email, loginRequestModel.Password);

            //驗證成功回傳USER物件
            if (user != null)
            {
                //Gen jwt
                var tokenStr = _jwtHelper.GetToken(user);
                //回傳 JWT Token
                return Ok(new { token = tokenStr });
            }
            else
            {
                return Unauthorized(new { message = "Login fail" });
            }
        }
    }
}
