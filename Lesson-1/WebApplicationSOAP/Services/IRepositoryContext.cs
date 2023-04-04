using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services
{
    public interface IRepositoryContext
    {
        public IList<Book> Books { get; }
    }
}
