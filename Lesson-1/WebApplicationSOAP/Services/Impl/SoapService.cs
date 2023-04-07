using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services.Impl;

public class SoapService : ISoapService
{
    private readonly IRepositoryBook _repositoryBook;
    public SoapService(IRepositoryBook repositoryBook)
    {
        _repositoryBook = repositoryBook;
    }

    public Book[] GetBooksByAuthor(string author)
    {
        return _repositoryBook.GetByAuthor(author).ToArray();
    }

    public Book[] GetBooksByCategory(string category)
    {
        return _repositoryBook.GetByAuthor(category).ToArray();
    }

    public Book[] GetBooksByTitle(string title)
    {
        return _repositoryBook.GetByTitle(title).ToArray();
    }
}
