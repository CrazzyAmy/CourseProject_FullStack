using CourseCore.Interface;
using CourseCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseCore.Service
{
    public class MemberService : IMemberService
    {
        private readonly IStudentRepository _studentRepository;

        public MemberService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> MemberRegisterServiceAsync(MemberRegisterModel memberRegisterModel)
        {
            //檢查帳號重覆
            var user = await _studentRepository.QueryAsync(memberRegisterModel.Email);
            if (user != null) return false;


            //密碼hash
            memberRegisterModel.Id = Guid.NewGuid();
            memberRegisterModel.PassWord = PwdHelper.PwdSHA256Hash(memberRegisterModel.PassWord, memberRegisterModel.Id.ToString());

            //insert to db
            await _studentRepository.CreateAsync(memberRegisterModel);

            return true;
        }

        public async Task<MemberModel> MemberSignAsync(string email, string pwd)
        {
            //驗證帳號是否存在
            var user = await _studentRepository.QueryAsync(email);
            if (user is null) return null;

            //驗證密碼
            var hashPwd = PwdHelper.PwdSHA256Hash(pwd, user.Id.ToString());
            if (hashPwd != user.PassWord) return null;

            return user;
        }

        public async Task<bool> MemberPwdUpdateAsync(UserPwdReqModel userPwdReqModel)
        {
            //query user by id
            var user = await _studentRepository.QueryAsync(userPwdReqModel.UserId);
            if (user is null) return false;

            //檢查舊密碼是否正確
            var hashPwd = PwdHelper.PwdSHA256Hash(userPwdReqModel.OldPwd, user.Id.ToString());
            if (hashPwd != user.PassWord) return false;

            //update pwd
            var hashNewPwd = PwdHelper.PwdSHA256Hash(userPwdReqModel.NewPwd, user.Id.ToString());
            return await _studentRepository.UpdatePwdAsync(userPwdReqModel.UserId, hashNewPwd);
        }

        public async Task<bool> MemberInfoUpdateAsync(UserInfoReqModel userInfoReqModel)
        {
            return await _studentRepository.UpdateInfoAsync(userInfoReqModel);
        }

        public async Task<UserInfoReqModel> GetMemberInfoAsync(Guid userId)
        {
            var user = await _studentRepository.QueryAsync(userId);
            if (user is null) return null;

            return new UserInfoReqModel { UserId = user.Id, Name = user.UserName, Mobile = user.Mobile };
        }
    }
}
