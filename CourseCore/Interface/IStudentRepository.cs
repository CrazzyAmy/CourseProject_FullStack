using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Interface
{
    public interface IStudentRepository
    {
        Task<bool> CreateAsync(MemberRegisterModel memberRegisterModel);

        Task<MemberModel> QueryAsync(string email);
        Task<MemberModel> QueryAsync(Guid userId);

        Task<bool> UpdatePwdAsync(Guid userId, string newPwd);
        Task<bool> UpdateInfoAsync(UserInfoReqModel userInfoReqModel);
    }
}
