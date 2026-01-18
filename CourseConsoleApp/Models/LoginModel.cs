using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseConsoleApp.Models
{
    // 登入請求的模型類別
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // 登入回應的模型類別
    public class LoginResponseModel
    {
        public string Token { get; set; }
    }
}
