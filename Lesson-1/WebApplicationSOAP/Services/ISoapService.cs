using System.ServiceModel;
using WebApplicationSOAP.Models;

namespace WebApplicationSOAP.Services;

[ServiceContract]
public interface ISoapService
{
    [OperationContract]
    public Book[] GetBooksByTitle(string title);

    [OperationContract]
    public Book[] GetBooksByAuthor(string author);

    [OperationContract]
    public Book[] GetBooksByCategory(string category);
}
