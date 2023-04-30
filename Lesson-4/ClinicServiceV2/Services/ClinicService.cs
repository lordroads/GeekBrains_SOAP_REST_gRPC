using Clinic.Service.Data;
using ClinicServiceNamespace;
using Grpc.Core;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicServiceV2.Services;

public class ClinicService : ClinicServiceBase
{
    private readonly ClinicServiceDBContext _dbContext;

    public ClinicService(ClinicServiceDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        try
        {
            var client = new Client
            {
                Document = request.Document,
                Surname = request.Surname,
                FirstName = request.Firstname,
                Patronymic = request.Patronymic
            };

            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            var response = new CreateClientResponse
            {
                ClientId = client.Id,
                ErrorCode = 0,
                ErrorMessage = ""
            };

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CreateClientResponse
            {
                ErrorCode = 1001,
                ErrorMessage = "Internal server error. \n[Owner error]:\n" + ex.Message
            });
        }
    }

    public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
    {
        try
        {
            var clients = _dbContext.Clients
                .Select(client => new ClientResponse
                {
                    ClientId = client.Id,
                    Document = client.Document,
                    Firstname = client.FirstName,
                    Patronymic = client.Patronymic,
                    Surname = client.Surname
                })
                .ToList();

            var response = new GetClientsResponse();

            response.Clients.AddRange(clients);
            response.ErrorCode = 0;
            response.ErrorMessage = "";

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            return Task.FromResult(new GetClientsResponse
            {
                ErrorCode = 1002,
                ErrorMessage = "Internal server error. \n[Owner error]:\n" + ex.Message
            });
        }
    }
}
