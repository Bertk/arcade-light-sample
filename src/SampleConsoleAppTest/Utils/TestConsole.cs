using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Xunit.Abstractions;

namespace SampleConsoleAppTest.CommandLine.Tests.Utils
{
    public class TestConsole : IConsole
    {
        public TestConsole(ITestOutputHelper output)
        {
            Out = new XunitTextWriter(output);
            Error = new XunitTextWriter(output);
        }

        public TextWriter Out { get; set; }

        public TextWriter Error { get; set; }

        public TextReader In => throw new NotSupportedException();

        public bool IsInputRedirected => throw new NotSupportedException();

        public bool IsOutputRedirected => true;

        public bool IsErrorRedirected => true;

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public event ConsoleCancelEventHandler? CancelKeyPress
        {
            add { 
              // empty body
            }
            remove {
              // empty body
            }
        }

        public void ResetColor()
        {
            // empty body
        }
    }
}
