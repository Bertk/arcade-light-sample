using System.Threading.Tasks;
using Calculator.SharedLib.Generated;
using Grpc.Core;
using static Calculator.SharedLib.Generated.CalculatorService;

namespace Calculator.ServiceLib
{
    public class CalculatorService : CalculatorServiceBase
    {
        public override Task<CalculatorReply> Add(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 + request.N2 });

        public override Task<CalculatorReply> Divide(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 / request.N2 });

        public override Task<CalculatorReply> Multiply(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 * request.N2 });

        public override Task<CalculatorReply> Subtract(CalculatorRequest request, ServerCallContext context)
        {
#pragma warning disable S1244 // Floating point numbers should not be tested for equality
            if (request.N2 == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Division by zero."));
            }
#pragma warning restore S1244 // Floating point numbers should not be tested for equality

            return Task.FromResult(new CalculatorReply { Result = request.N1 - request.N2 });
        }
    }
}
