using WebMiniAPI.Interface;
using WebMiniAPI.Service;
using Swashbuckle.AspNetCore.Swagger;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 IBookService 和其實作
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

/*
// 模擬資料庫
var books = new List<Book>();
books.Add(new Book() { Id = 1, Title = "C#", Price = 500, Year = 2023 });
books.Add(new Book() { Id = 2, Title = "ASP.NET", Price = 300, Year = 2024 });
books.Add(new Book() { Id = 3, Title = "Azure", Price = 600, Year = 2024 });

// GET /api/books - 取得所有書籍
app.MapGet("/api/books", () => books);

// GET /api/books/{id} - 取得特定書籍
app.MapGet("/api/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

// POST /api/books - 新增書籍
app.MapPost("/api/books", (Book book) =>
{
    book.Id = books.Count + 1;
    books.Add(book);
    
    return Results.Created($"/books/{book.Id}", book);
});

// PUT /api/books/{id} - 更新書籍
app.MapPut("/api/books/{id}", (int id, Book book) =>
{
    if (id != book.Id) return Results.BadRequest();

    var orgBook = books.FirstOrDefault(b => b.Id == id);
    if (orgBook is null) return Results.NotFound();
    orgBook.Title = book.Title;
    orgBook.Price = book.Price;
    orgBook.Year = book.Year;
    return Results.Ok(book);
});

// DELETE /api/books/{id} - 刪除書籍
app.MapDelete("/api/books/{id}", (int id) =>
{
    var orgBook = books.FirstOrDefault(b => b.Id == id);
    if (orgBook is null) return Results.NotFound();
    books.Remove(orgBook);
    return Results.Ok();
});

*/


#region 抽離API實作邏輯

// 使用非同步方法處理請求
app.MapGet("/api/books", async (IBookService service) => await service.GetAllBooksAsync())
    .WithName("GetBooks") //為端點指定一個唯一的名稱，主要用途在生成 OpenAPI/Swagger 文件時作為 operationId
    .WithOpenApi()　//將端點加入到 OpenAPI/Swagger 文檔中
    .AllowAnonymous(); //允許匿名存取

app.MapGet("/api/books/{id}", async (int id, IBookService service) => await service.GetBookAsync(id))
    .WithName("GetBook")
    .WithOpenApi();

app.MapPost("/api/books", async (Book book, IBookService service) =>
    await service.CreateBookAsync(book))
    .WithName("CreateBook")
    .WithOpenApi();

app.MapPut("/api/books/{id}", async (int id, Book book, IBookService service) =>
    await service.UpdateBookAsync(id, book))
    .WithName("UpdateBook")
    .WithOpenApi();

app.MapDelete("/api/books/{id}", async (int id, IBookService service) =>
    await service.DeleteBookAsync(id))
    .WithName("DeleteBook")
    .WithOpenApi();


#endregion


#region  加入驗證需求
/*
// 使用非同步方法處理請求
app.MapGet("/api/books", async (IBookService service) =>
    await service.GetAllBooksAsync())
    .WithName("GetBooks")
    .WithOpenApi()
    .RequireAuthorization(); // 加入驗證需求
*/

#endregion

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
