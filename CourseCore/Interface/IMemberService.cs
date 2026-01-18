using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Interface
{
    public interface IMemberService
    {
        Task<bool> MemberRegisterServiceAsync(MemberRegisterModel memberRegisterModel);

        Task<MemberModel> MemberSignAsync(string email, string pwd);
        Task<bool> MemberPwdUpdateAsync(UserPwdReqModel userPwdReqModel);
        Task<bool> MemberInfoUpdateAsync(UserInfoReqModel userInfoReqModel);

        Task<UserInfoReqModel> GetMemberInfoAsync(Guid userId);

    }
}
