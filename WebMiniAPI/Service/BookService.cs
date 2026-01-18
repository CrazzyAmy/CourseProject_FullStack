using WebMiniAPI.Interface;

namespace WebMiniAPI.Service
{
    public class BookService : IBookService
    {
        //模擬資料庫
        private static List<Book> _books = new List<Book>()
        {
            new Book() { Id = 1, Title = "C#", Price = 500, Year = 2023 },
            new Book() { Id = 2, Title = "ASP.NET", Price = 300, Year = 2024 },
            new Book() { Id = 3, Title = "Azure", Price = 600, Year = 2024 }
        };

        public async Task<IResult> GetAllBooksAsync()
        {
            // 模擬非同步操作
            await Task.Delay(10);
            return Results.Ok(_books);
        }

        public async Task<IResult> GetBookAsync(int id)
        {
            // 模擬非同步操作
            await Task.Delay(10);
            var book = _books.FirstOrDefault(b => b.Id == id);
            return book is null ? Results.NotFound() : Results.Ok(book);
        }

        public async Task<IResult> CreateBookAsync(Book book)
        {
            await Task.Delay(10);
            book.Id = _books.Count + 1;
            _books.Add(book);
            return Results.Created($"/books/{book.Id}", book);
        }

        public async Task<IResult> UpdateBookAsync(int id, Book book)
        {
            await Task.Delay(10);
            if (id != book.Id) return Results.BadRequest();

            var orgBook = _books.FirstOrDefault(b => b.Id == id);
            if (orgBook is null) return Results.NotFound();
            orgBook.Title = book.Title;
            orgBook.Price = book.Price;
            orgBook.Year = book.Year;
            return Results.Ok(book);
        }

        public async Task<IResult> DeleteBookAsync(int id)
        {
            await Task.Delay(10);
            var orgBook = _books.FirstOrDefault(b => b.Id == id);
            if (orgBook is null) return Results.NotFound();
            _books.Remove(orgBook);
            return Results.Ok();
        }
    }
}
