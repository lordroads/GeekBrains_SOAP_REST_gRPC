using Microsoft.AspNetCore.Mvc;
using RepositoryBooksServiceReference;
using WebApplicationSOAP.WebClient.Models;

namespace WebApplicationSOAP.WebClient.Controllers
{
    public class BookController : Controller
    {
        private readonly ISoapService _client;
        private readonly ILogger<BookController> _logger;

        public BookController(ISoapService soapServiceClient,
            ILogger<BookController> logger)
        {
            _client = soapServiceClient;
            _logger = logger;
        }

        public IActionResult Index(SearchType searchType, string searchString)
        {
            try
            {
                var modelResult = new BooksViewModel();

                if (!string.IsNullOrEmpty(searchString) && searchString.Length >= 3)
                {


                    switch (searchType)
                    {
                        case SearchType.Title:
                            modelResult.Books = 
                                _client
                                .GetBooksByTitleAsync(
                                    new GetBooksByTitleRequest(
                                        new GetBooksByTitleRequestBody(searchString)))
                                .Result
                                .Body
                                .GetBooksByTitleResult;
                            break;
                        case SearchType.Author:
                            modelResult.Books =
                                _client
                                .GetBooksByAuthorAsync(
                                    new GetBooksByAuthorRequest(
                                        new GetBooksByAuthorRequestBody(searchString)))
                                .Result
                                .Body
                                .GetBooksByAuthorResult;
                            break;
                        case SearchType.Category:
                            modelResult.Books =
                                _client
                                .GetBooksByCategoryAsync(
                                    new GetBooksByCategoryRequest(
                                        new GetBooksByCategoryRequestBody(searchString)))
                                .Result
                                .Body
                                .GetBooksByCategoryResult;
                            break;
                        default:
                            break;
                    }
                }


                return View(modelResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return View(new BooksViewModel());
            }
        }
    }
}
