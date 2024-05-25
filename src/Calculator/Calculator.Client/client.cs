using System;
using System.Threading.Tasks;
using Calculator.SharedLib.Generated;
using Grpc.Net.Client;

// error message:  AuthenticationException: The remote certificate is invalid because of errors in the certificate chain: UntrustedRoot
// use command: `dotnet dev-certs https --trust` 

namespace CalculatorClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
#pragma warning disable S6966
            // Using Microsoft's Grpc
            using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
            CalculatorService.CalculatorServiceClient client = new CalculatorService.CalculatorServiceClient(channel);

            const int n1 = 34;
            const int n2 = 76;
            CalculatorReply sum = client.Add(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} + {n2} = {sum.Result}");

            CalculatorReply difference = client.Subtract(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} - {n2} = {difference.Result}");

            CalculatorReply product = client.Multiply(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} * {n2} = {product.Result}");

            CalculatorReply division = client.Divide(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} / {n2} = {division.Result}");

            CalculatorReply divisionAsync = await client.DivideAsync(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service async: {n1} / {n2} = {divisionAsync.Result}");

            CalculatorReply divisionError = client.Divide(new CalculatorRequest { N1 = n1, N2 = 0 });
            Console.WriteLine($"Called service with error: {n1} / {0} = {divisionError.Result}");
#pragma warning restore S6966
        }
    }
}

