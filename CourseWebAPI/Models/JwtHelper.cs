using CourseCore.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace CourseWebAPI.Models
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GetToken(MemberModel memberModel)
        {
            //建立簽名金鑰
            //從配置檔讀取密鑰字串
            //建立對稱式加密金鑰
            var securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_config["JwtTokenSettings:IssuerSigningKey"]));

            //建立簽名憑證
            // HmacSha256 SecurityAlgorithms
            // - 使用 HMAC SHA256 演算法
            // - 用於對 Token 進行簽名，確保 Token 的完整性
            var signingCredentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256Signature);


            //建立 Claims (存放使用者資訊)
            //Claims 是 JWT 的 Payload 部分，用於存放一些聲明性資訊
            //例如：使用者 ID、使用者名稱、角色、權限等
            //可以依需求添加更多 Claims
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid, memberModel.Id.ToString()),
                new Claim(ClaimTypes.Name, memberModel.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // 建立 Token 描述
            // 定義 Token 的所有屬性
            // 包含發行者、接收者、有效期等資訊
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config["JwtTokenSettings:Issuer"],
                Audience = _config["JwtTokenSettings:Audience"],
                NotBefore = DateTime.Now,
                IssuedAt = DateTime.Now, //核發時間
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(int.Parse(_config["JwtTokenSettings:Expire"])),
                SigningCredentials = signingCredentials
            };

            // 產生 JWT Token
            // 使用 JwtSecurityTokenHandler 類別的 CreateToken 方法
            // 產生 JWT Token 字串
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
    }
}
