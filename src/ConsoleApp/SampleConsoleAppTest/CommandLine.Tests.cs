using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace SampleConsoleAppTest.CommandLine.Tests
{
    public class CommandLineTest
    {

        private protected static bool RunCommand(string command, string arguments, out string standardOutput, out string standardError, string workingDirectory = "")
        {
            Debug.WriteLine($"BaseTest.RunCommand: {command} {arguments}\nWorkingDirectory: {workingDirectory}");
            var psi = new ProcessStartInfo(command, arguments);
            psi.WorkingDirectory = workingDirectory;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            Process commandProcess = Process.Start(psi)!;
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
            CommandLineTest.RunCommand(ToolCommandPath, $"-h", out string standardOutput, out string standardError);
            Assert.Contains("CommandLine example", standardOutput, StringComparison.CurrentCulture);
        }

        [Fact]
        public void commandMessageRequiredTest()
        {
            string ToolCommandPath = GetCommandPath();
            CommandLineTest.RunCommand(ToolCommandPath, $"", out string standardOutput, out string standardError);
            Assert.Contains("Option '--message' is required.", standardError, StringComparison.CurrentCulture);
        }
        [Fact]
        public void commandWrongOptionTest()
        {
#pragma warning disable S125 // Sections of code should not be commented out
            /* Unmerged change from project 'SampleConsoleApp.Tests (net6.0)'
            Before:
                       string ToolCommandPath = GetCommandPath();
                       RunCommand(ToolCommandPath, $"--missing", out string standardOutput, out string standardError);
            After:
                       string ToolCommandPath = GetCommandPath();
                        CommandLineTest.RunCommand(ToolCommandPath, $"--missing", out string standardOutput, out string standardError);
            */
#pragma warning restore S125 // Sections of code should not be commented out
            string ToolCommandPath = GetCommandPath();
            RunCommand(ToolCommandPath, $"--missing", out string standardOutput, out string standardError);
            Assert.Contains("Unrecognized command or argument '--missing'.", standardError, StringComparison.CurrentCulture);
        }

        static internal string GetCommandPath()
        {
            Assembly TestAssembly = typeof(CommandLineTest).Assembly;
            string currentPath = Assembly.GetExecutingAssembly().Location;
            string rootDirectory = currentPath.Substring(0, currentPath.IndexOf("\\bin\\", StringComparison.CurrentCulture));
            string ToolCommandPath = string.Concat(rootDirectory, "\\bin\\SampleConsoleApp\\Debug\\net6.0\\SampleConsoleApp.exe");
            return ToolCommandPath;
        }
    }
}




