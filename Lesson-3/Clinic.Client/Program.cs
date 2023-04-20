using ClinicServiceNamespace;
using Grpc.Net.Client;
using static ClinicServiceNamespace.ClinicService;

namespace Clinic.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");

            ClinicServiceClient clinicServiceClient = new ClinicServiceClient(channel);

            var createClientResponse = clinicServiceClient.CreateClient(new CreateClientRequest
            {
                Document = "DOC34 445",
                Firstname = "Yurii",
                Patronymic = "Victorovich",
                Surname = "Mitrohin"
            });

            if (createClientResponse.ErrorCode == 0)
            {
                Console.WriteLine($"Client #{createClientResponse.ClientId} created successfully.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Create client error.\n" +
                    $"ErrorCode: {createClientResponse.ErrorCode}\n" +
                    $"ErrorMessage: {createClientResponse.ErrorMessage}");
            }


            var getClientResponse = clinicServiceClient.GetClients(new GetClientsRequest());

            if (createClientResponse.ErrorCode == 0)
            {
                Console.WriteLine("Clients");
                Console.WriteLine(new String('=', 10) + '\n');

                foreach (var item in getClientResponse.Clients)
                {
                    Console.WriteLine($"#{item.ClientId} " +
                        $"{item.Document} " +
                        $"{item.Surname} " +
                        $"{item.Firstname} " +
                        $"{item.Patronymic}");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Create client error.\n" +
                    $"ErrorCode: {getClientResponse.ErrorCode}\n" +
                    $"ErrorMessage: {getClientResponse.ErrorMessage}");
            }


            Console.ReadKey(true);
        }
    }
}