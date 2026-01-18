using CourseCore.Interface;
using CourseCore.Models;
using CourseDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataAccess.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly KhNetCourseContext _dbContext;

        public StudentRepository(KhNetCourseContext khNetCourseContext)
        {
            _dbContext = khNetCourseContext;
        }


        public async Task<bool> CreateAsync(MemberRegisterModel user)
        {
            await _dbContext.Students.AddAsync(new Student()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.UserName,
                Password = user.PassWord
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<MemberModel> QueryAsync(string email)
        {
            var entity = await _dbContext.Students.FirstOrDefaultAsync(s => s.Email.ToUpper() == email.ToUpper());

            if (entity == null) return null;

            return new MemberModel()
            {
                Id = entity.Id,
                Email = entity.Email,
                Mobile = entity.Mobile,
                PassWord = entity.Password,
                UserName = entity.Name
            };
        }


        public async Task<MemberModel> QueryAsync(Guid userId)
        {
            var entity = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == userId);

            if (entity == null) return null;

            return new MemberModel()
            {
                Id = entity.Id,
                Email = entity.Email,
                Mobile = entity.Mobile,
                PassWord = entity.Password,
                UserName = entity.Name
            };
        }

        public async Task<bool> UpdatePwdAsync(Guid userId, string newPwd)
        {
            var entity = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == userId);

            if (entity == null) return false;

            entity.Password = newPwd;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateInfoAsync(UserInfoReqModel userInfoReqModel)
        {
            var entity = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == userInfoReqModel.UserId);

            if (entity == null) return false;

            entity.Name = userInfoReqModel.Name;
            entity.Mobile = userInfoReqModel.Mobile;

            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
