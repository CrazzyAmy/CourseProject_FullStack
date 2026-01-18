namespace WebMiniAPI.Interface
{
    public interface IBookService
    {
        Task<IResult> GetAllBooksAsync();
        Task<IResult> GetBookAsync(int id);
        Task<IResult> CreateBookAsync(Book book);
        Task<IResult> UpdateBookAsync(int id, Book book);
        Task<IResult> DeleteBookAsync(int id);
    }
}
