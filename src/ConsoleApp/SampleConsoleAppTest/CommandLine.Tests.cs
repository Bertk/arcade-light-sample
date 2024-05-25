using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SampleConsoleAppTest.CommandLine.Tests
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
            ProcessStartInfo process = new ProcessStartInfo(command, arguments);
            process.WorkingDirectory = workingDirectory;
            process.RedirectStandardError = true;
            process.RedirectStandardOutput = true;
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
            CommandLineTest.RunCommand(ToolCommandPath, $"-h", out string standardOutput, out string standardError);
            if (!string.IsNullOrEmpty(standardError))
            {
                _output.WriteLine(standardError);
            }
            Assert.Contains("CommandLine example", standardOutput, StringComparison.CurrentCulture);
        }

        [Fact]
        public void commandMessageRequiredTest()
        {
            string ToolCommandPath = GetCommandPath();
            Assert.True(File.Exists(ToolCommandPath), $"File '{ToolCommandPath}' does not exist .");
            CommandLineTest.RunCommand(ToolCommandPath, $"", out string standardOutput, out string standardError);
            _output.WriteLine(standardOutput);
            _output.WriteLine("******************************************************************************************");
            _output.WriteLine(standardError);
            _output.WriteLine("******************************************************************************************");
            Assert.Contains("Option '--message' is required.", standardError, StringComparison.CurrentCulture);
        }
        [Fact]
        public void commandWrongOptionTest()
        {
            string ToolCommandPath = GetCommandPath();
            Assert.True(File.Exists(ToolCommandPath), $"File '{ToolCommandPath}' does not exist .");
            RunCommand(ToolCommandPath, $"--missing", out string standardOutput, out string standardError);
            _output.WriteLine(standardOutput);
            _output.WriteLine("******************************************************************************************");
            _output.WriteLine(standardError);
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
            string rootDirectory = currentPath.Substring(0, currentPath.IndexOf("\\bin\\", StringComparison.CurrentCulture));
#if NET8_0
            string targetFramework = "net8.0";
#endif
#if NET7_0
            string targetFramework = "net7.0";
#endif
#if DEBUG
            string buildConfiguration = "Debug";
#endif
#if RELEASE
            string buildConfiguration = "Release";
#endif
            string ToolCommandPath = Path.Combine(rootDirectory, "bin", "SampleConsoleApp", buildConfiguration, targetFramework, "SampleConsoleApp.exe");
            return ToolCommandPath;
        }
    }
}




