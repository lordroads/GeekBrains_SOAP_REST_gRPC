using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services.Impl
{
    public class BooksRepository : IRepositoryBook
    {
        private readonly IRepositoryContext _context;
        private readonly ILogger _logger;

        public BooksRepository(
            IRepositoryContext context, 
            ILogger<BooksRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Book> GetByAuthor(string author)
        {
            try
            {
                return _context.Books
                .Where(book =>
                    book.Authors
                    .Where(a =>
                        a.Name
                        .ToLower()
                        .Contains(author.ToLower()))
                    .Count() > 0)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Book>();
            }
            
        }

        public IList<Book> GetByCategory(string category)
        {
            try
            {
                return _context.Books
                    .Where(book =>
                    book.Category
                    .ToLower()
                    .Contains(category.ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Book>();
            }
                
        }

        public IList<Book> GetByTitle(string title)
        {
            try
            {
                return _context.Books
                    .Where(book => 
                    book.Title
                    .ToLower()
                    .Contains(title.ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Book>();
            }
                    
        }
    }
}
