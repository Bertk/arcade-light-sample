using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleConsoleAppTest.CommandLine.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace SampleConsoleAppTest.CommandLine.Tests
{
    public class CommandLineTest
    {
        private readonly ITestOutputHelper _output;
        [Fact]
        public async Task TestReturnCode()
        {
            var exitCode = await new HostBuilder()
                    .ConfigureServices(collection => collection.AddSingleton<IConsole>(new TestConsole(_output)))
                    .RunCommandLineApplicationAsync<Return42Command>(Array.Empty<string>());
            Assert.Equal(42, exitCode);
        }

        [Command]
        public class Return42Command
        {
            private int OnExecute()
            {
                return 42;
            }
        }
    }
}




