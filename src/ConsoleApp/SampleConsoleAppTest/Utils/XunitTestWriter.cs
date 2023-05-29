using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace SampleConsoleAppTest.CommandLine.Tests
{
    public class XunitTextWriter : TextWriter
    {
        private readonly ITestOutputHelper _output;
        private readonly StringBuilder _sb = new();

        public XunitTextWriter(ITestOutputHelper output)
        {
            _output = output;
        }

        public override Encoding Encoding => Encoding.Unicode;

        public override void Write(char value)
        {
            if (value == '\n')
            {
                _output.WriteLine(_sb.ToString());
                _sb.Clear();
            }
            else
            {
                _sb.Append(value);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_sb.Length > 0))
                {
                    _output.WriteLine(_sb.ToString());
                    _sb.Clear();
                }

            base.Dispose(disposing);
        }
    }
}
