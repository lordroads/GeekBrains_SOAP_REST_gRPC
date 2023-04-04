using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services
{
    public interface IRepositoryBook
    {
        IList<Book> GetByTitle(string title);
        IList<Book> GetByAuthor(string author);
        IList<Book> GetByCategory(string category);
    }
}
