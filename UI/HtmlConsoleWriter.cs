using System;
using System.Text;

namespace AllInOne
{
    class HtmlConsoleWriter:System.IO.TextWriter 
    {
        Proxy proxy;
        bool newLine;

        public HtmlConsoleWriter(Proxy proxy)
        {
            this.proxy = proxy;
        }

        public override void WriteLine(string value)
        {
            proxy.ConsoleWrite(value, true);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            proxy.ConsoleWrite(String.Format(format, arg), true);
        }

        public override void Write(string value)
        {
            proxy.ConsoleWrite(value, false);
        }

        public override void Write(object value)
        {
            proxy.ConsoleWrite(value.ToString(), false);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
