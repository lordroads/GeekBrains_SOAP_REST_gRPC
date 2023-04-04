using RepositoryBooksServiceReference;

namespace WebApplicationSOAP.WebClient.Models;

public class BooksViewModel
{
    public Book[] Books { get; set; }
    public SearchType SearchType { get; set; }
    public string SearchString { get; set; }
}
