using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInOne
{
    class SampleAdapter : MarshalByRefObject
    {
        internal void Execute(string content, Proxy p)
        {
            Console.SetOut(new HtmlConsoleWriter(p));

            var result = Compiler.CompileAndRun(content);

            foreach (CompilerError error in result.Errors)
            {
                p.ConsoleWrite(error.ErrorText, true);
            }   
        }
    }
}
