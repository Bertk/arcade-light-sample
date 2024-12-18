using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace SampleConsoleApp.Tests
{
    public class CommandLineTest
    {
        private readonly ITestOutputHelper _output;
        private readonly string _language;
        public CommandLineTest(ITestOutputHelper output)
        {
            _output = output;
            _language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
        private protected static bool RunCommand(string command, string arguments, out string standardOutput, out string standardError, string workingDirectory = "")
        {
            Debug.WriteLine($"BaseTest.RunCommand: {command} {arguments}\nWorkingDirectory: {workingDirectory}");
            ProcessStartInfo process = new(command, arguments)
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            Process commandProcess = Process.Start(process)!;
            if (!commandProcess.WaitForExit((int)TimeSpan.FromMinutes(5).TotalMilliseconds))
            {
                throw new XunitException($"Command '{command} {arguments}' didn't end after 5 minute");
            }
            standardOutput = commandProcess.StandardOutput.ReadToEnd();
            standardError = commandProcess.StandardError.ReadToEnd();
            return commandProcess.ExitCode == 0;
        }

        [Fact]
        public void commandHelpTest()
        {
            string ToolCommandPath = GetCommandPath();
            Assert.True(File.Exists(ToolCommandPath), $"File '{ToolCommandPath}' does not exist .");
            _ = RunCommand(ToolCommandPath, $"-h", out string standardOutput, out string standardError);
            if (!string.IsNullOrEmpty(standardError))
            {
                _output.WriteLine(standardError);
                TestContext.Current.AddAttachment("error", standardError);
            }
            else
            {
                _output.WriteLine(standardOutput);
                TestContext.Current.AddAttachment("output", standardOutput);
            }
            Assert.Contains("CommandLine example", standardOutput, StringComparison.CurrentCulture);
        }

        [Fact]
        public void commandMessageRequiredTest()
        {
            string ToolCommandPath = GetCommandPath();
            Assert.True(File.Exists(ToolCommandPath), $"File '{ToolCommandPath}' does not exist .");
            _ = RunCommand(ToolCommandPath, $"", out string standardOutput, out string standardError);
            _output.WriteLine(standardOutput);
            TestContext.Current.AddAttachment("output", standardOutput);
            _output.WriteLine("******************************************************************************************");
            _output.WriteLine(standardError);
            TestContext.Current.AddAttachment("error", standardError);
            _output.WriteLine("******************************************************************************************");
            Assert.Contains("Option '--message' is required.", standardError, StringComparison.CurrentCulture);
        }
        [Fact]
        public void commandWrongOptionTest()
        {
            string ToolCommandPath = GetCommandPath();
            Assert.True(File.Exists(ToolCommandPath), $"File '{ToolCommandPath}' does not exist .");
            _ = RunCommand(ToolCommandPath, $"--missing", out string standardOutput, out string standardError);
            _output.WriteLine(standardOutput);
            TestContext.Current.AddAttachment("output", standardOutput);
            _output.WriteLine("******************************************************************************************");
            _output.WriteLine(standardError);
            TestContext.Current.AddAttachment("error", standardError);
            _output.WriteLine("******************************************************************************************");
            switch (_language)
            {
                case "de": Assert.Contains("Befehl oder Argument '--missing' nicht erkannt", standardError, StringComparison.InvariantCulture); break;
                default: Assert.Contains("Unrecognized command or argument '--missing'", standardError, StringComparison.InvariantCulture); break;
            }

        }

        internal static string GetCommandPath()
        {
            string currentPath = Assembly.GetExecutingAssembly().Location;
            string rootDirectory = currentPath.Substring(0, currentPath.IndexOf($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.CurrentCulture));
#if NET6_0
            const string targetFramework = "net6.0";
#endif
#if NET8_0
            const string targetFramework = "net8.0";
#endif
#if NET9_0
            const string targetFramework = "net9.0";
#endif
#if DEBUG
            const string buildConfiguration = "Debug";
#endif
#if RELEASE
            const string buildConfiguration = "Release";
#endif

            string extension = "";
            if (OperatingSystem.IsWindows())
            {
                extension = ".exe";
            }

            string ToolCommandPath = Path.Combine(rootDirectory, "bin", "SampleConsoleApp", buildConfiguration, targetFramework, $"SampleConsoleApp{extension}");

            return ToolCommandPath;
        }
    }
}
