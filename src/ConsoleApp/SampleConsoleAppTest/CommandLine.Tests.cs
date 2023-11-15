using System;
using System.Diagnostics;
using System.Linq;
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
            using Process commandProcess = new();
            commandProcess.StartInfo.FileName = command;
            commandProcess.StartInfo.Arguments = arguments;
            commandProcess.StartInfo.WorkingDirectory = workingDirectory;
            commandProcess.StartInfo.RedirectStandardError = true;
            commandProcess.StartInfo.RedirectStandardOutput = true;
            string eOut = "";
            commandProcess.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { eOut += e.Data; });
            commandProcess.Start();
            // To avoid deadlocks, use an asynchronous read operation on at least one of the streams.
            commandProcess.BeginErrorReadLine();
            if (!commandProcess.WaitForExit((int)TimeSpan.FromMinutes(5).TotalMilliseconds))
            {
                throw new XunitException($"Command '{command} {arguments}' didn't end after 5 minute");
            }
            standardOutput = commandProcess.StandardOutput.ReadToEnd();
            standardError = eOut;
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
            string ToolCommandPath = string.Concat(rootDirectory, $"\\bin\\SampleConsoleApp\\Debug\\{targetFramework}\\SampleConsoleApp.exe");
            return ToolCommandPath;
        }
    }
}




