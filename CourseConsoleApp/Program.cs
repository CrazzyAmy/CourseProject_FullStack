using CourseConsoleApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CourseConsoleApp
{
    internal class Program
    {
        static HttpClient client = new HttpClient();
        const string apiHost = "https://localhost:7096";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //var courseScheduleData = await GetCourseScheduleAsync();
            //foreach (var courseSchedule in courseScheduleData)
            //{
            //    Console.WriteLine($"Course:{courseSchedule.CourseCode},Name:{courseSchedule.CourseName},times:{courseSchedule.CourseTimes}");
            //}

            //var courseScheduleData2 = await GetCourseScheduleAsync(Guid.Parse("385C9EC6-4FF5-4473-AF9C-39175A5F1DBB"));
            //if (courseScheduleData2!=null)
            //{
            //    Console.WriteLine($"Course:{courseScheduleData2.CourseCode},Name:{courseScheduleData2.CourseName},times:{courseScheduleData2.CourseTimes}");
            //}
            //else
            //{
            //    Console.WriteLine("=== Not Found =====");
            //}

            //var jwtToken = await LoginAsync("user202409@mail.com", "123456");
            //Console.WriteLine(jwtToken);

            //var booking = await StuBookingAsync(jwtToken, Guid.Parse("F1A058E6-3706-43AA-8907-3A97007FF174"), Guid.Parse("C1ECD4B3-EF73-4589-B601-59368B835F94"));
            //Console.WriteLine(booking == true ? "預約成功！" : "預約失敗！");


            await AzureOpenAIAsync("寫一個生成式AI的影響力臉書貼文");

        }

        /// <summary>
        /// 取得開課記錄公開資料
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static async Task<List<CourseScheduleResponseModel>> GetCourseScheduleAsync()
        {
            var cs = new List<CourseScheduleResponseModel>();

            HttpResponseMessage response = await client.GetAsync($"{apiHost}/api/CourseSchedule");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                cs = JsonConvert.DeserializeObject<List<CourseScheduleResponseModel>>(jsonString);
            }
            return cs;
        }

        /// <summary>
        /// 取得單一筆開課記錄公開資料
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static async Task<CourseScheduleResponseModel> GetCourseScheduleAsync(Guid scheduleId)
        {
            CourseScheduleResponseModel cs = null;
            HttpResponseMessage response = await client.GetAsync($"{apiHost}/api/CourseSchedule/{scheduleId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                cs = JsonConvert.DeserializeObject<CourseScheduleResponseModel>(jsonString);
            }
            return cs;
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static async Task<string> LoginAsync(string email, string password)
        {
            //var dataAsString = JsonConvert.SerializeObject(new LoginRequestModel()
            //{
            //    Email = email,
            //    Password = password
            //});

            var dataAsString = JsonConvert.SerializeObject(new
            {
                Email = email,
                Password = password
            });

            var requestContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{apiHost}/api/login", requestContent);

            if (response.IsSuccessStatusCode)
            {
                // 讀取回應內容
                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析成功回應
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(responseContent);
                return loginResponse.Token;
            }
            return string.Empty;
        }


        static async Task<bool> StuBookingAsync(string token, Guid stuId, Guid scheduleId)
        {
            var booking = new BookingCourseRequestModel
            {
                StuId = stuId,
                ScheduleId = scheduleId
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");

            // 設定 JWT Bearer Token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //固定式Token，且自訂義的header
            //client.DefaultRequestHeaders.Add("Your-Custom-Header", "YourFixedTokenValue");

            // 發送 POST 請求到 booking endpoint
            var response = await client.PostAsync($"{apiHost}/api/Booking", requestContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            string rsp = await response.Content.ReadAsStringAsync();
            Console.WriteLine(rsp);
            return false;

            //if (!response.IsSuccessStatusCode)
            //{
            //    string rsp = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(rsp);
            //    return false;
            //}
            //return true;
        }


        static async Task AzureOpenAIAsync(string userPrompt)
        {
            string apiKey = "YOUR_OPENAI_API_KEY"; // Set your key here
            string apiUrl = "https://openaijpeast07.openai.azure.com/openai/deployments/gpt4o/chat/completions?api-version=2024-02-15-preview";

            client.DefaultRequestHeaders.Add("api-key", apiKey);

            var payload = new
            {
                messages = new object[]
                {
                  new {
                      role = "system",
                      content = new object[] {
                          new {
                              type = "text",
                              text = "You are an AI assistant that helps people find information."
                          }
                      }
                  },
                  new {
                      role = "user",
                      content = new object[] {
                          new {
                              type = "text",
                              text = userPrompt
                          }
                      }
                  }
                },
                temperature = 0.7,
                top_p = 0.95,
                max_tokens = 800,
                stream = false
            };
           
            var response = await client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                Console.WriteLine(responseData);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            }
        }


    }
}
