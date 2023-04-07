using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services.Impl
{
    public class BooksContext : IRepositoryContext
    {
        private IList<Book> _books;

        public IList<Book> Books { get => _books; }

        public BooksContext()
        {
            Initialize();
        }

        private void Initialize()
        {
            var stringBooks = Encoding.UTF8.GetString(Resources.Resource.books);
            _books = (List<Book>)JsonConvert
                .DeserializeObject(
                stringBooks,
                typeof(List<Book>));
        }
    }
}
