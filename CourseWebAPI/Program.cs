using CourseCore.Interface;
using CourseCore.Service;
using CourseDataAccess.Models;
using CourseDataAccess.Repository;
using CourseWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace CourseWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<KhNetCourseContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("KhNetCourseDB"))
                );



            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {

                opts.RequireHttpsMetadata = false; // 設定控制是否強制要求使用 HTTPS 協議，生產環境必須設為 true，以確保安全性
                opts.Events = new JwtBearerEvents();

                //設定要檢驗的JWT內容
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, //啟用發行者(Issuer)驗證，確保 Token 是由可信任的發行者產生
                    ValidateAudience = true,　//啟用接收者(Audience)驗證，確保 Token 是發給正確的接收對象
                    ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
                    ValidateLifetime = true, //啟用 Token 有效期驗證，確保 Token 沒有過期
                    ValidateIssuerSigningKey = true, //啟用簽名金鑰驗證，確保 Token 使用正確的金鑰簽名
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:IssuerSigningKey"])),
                    RequireExpirationTime = true　//要求 Token 必須包含過期時間
                };

                //JWT檢驗失敗的回應處理
                opts.Events.OnChallenge = context =>
                {
                    // 當驗證失敗時，回應標頭包含 WWW-Authenticate 標頭，顯示失敗的詳細原因
                    // 生產環境中通常應該設為 false，因為這涉及安全性考量
                    opts.IncludeErrorDetails = false;

                    context.HandleResponse();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 401;
                    var responseContent = JsonConvert.SerializeObject(new { errorMessage = "invalid token" });

                    return context.Response.WriteAsync(responseContent);
                };
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder
                    .WithOrigins("https://localhost:7034")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
                options.AddPolicy("AllowSpecificOrigin2", builder => {
                    builder
                        .WithOrigins("https://localhost:9999")
                        .WithMethods("GET","POST")
                        .AllowAnyHeader();
                });
            });


            builder.Services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            builder.Services.AddScoped<ICourseScheduleRepository, CourseScheduleRepository>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<IStuCourseScheduleRepository, StuCourseScheduleRepository>();
            builder.Services.AddScoped<JwtHelper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigin");
            app.UseCors("AllowSpecificOrigin2");

            ////全站統一Rule
            //app.UseCors(builder => builder
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    );


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
