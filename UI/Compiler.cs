using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AllInOne
{
    public enum CodeLanguage { CSharp, VisualBasic}

    class Compiler
    {
        static string[] refAssemblies;

        static Compiler()
        {
            var clrPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            refAssemblies = new string[]
            {
                clrPath + "System.dll",
                clrPath + "System.Core.dll",
                Path.GetDirectoryName(Environment.CurrentDirectory) + @"\Resources\Library2010.dll"
            };
        }

        public static CompilerResults CompileAndRun(String source, CodeLanguage language = CodeLanguage.CSharp)
        {
            // Select the code provider based on the input file extension.
            var provider = language == CodeLanguage.CSharp ?
                CodeDomProvider.CreateProvider("CSharp") : CodeDomProvider.CreateProvider("VisualBasic");


            var cp = new CompilerParameters
            {
                GenerateExecutable = true,
                GenerateInMemory = true,
                TreatWarningsAsErrors = false
            };

            cp.ReferencedAssemblies.AddRange(refAssemblies);

            // Invoke compilation of the source code.
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
            CopyAssembly();

            if (cr.Errors.Count < 1)
            {
                var entry = cr.CompiledAssembly.GetTypes().Select(t => t.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic))
                    .Single(m => m != null);

                entry.Invoke(null, new object[]{ null });
            }

            return cr;
        }

        static void CopyAssembly()
        {
            foreach (var item in refAssemblies.Where(a=>!a.Contains("System")))
            {
                var filename = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(item));

                if (File.Exists(filename))
                {
                    if(File.GetLastWriteTime(filename) != File.GetLastWriteTime(filename))
                        File.Copy(item, filename);
                }
                else File.Copy(item, filename);
            }
        }

        public static bool CompileExecutable(String sourceName)
        {
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOk = false;

            // Select the code provider based on the input file extension.
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".CS")
            {
                provider = CodeDomProvider.CreateProvider("CSharp");
            }
            else if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".VB")
            {
                provider = CodeDomProvider.CreateProvider("VisualBasic");
            }
            else
            {
                Console.WriteLine("Source file must have a .cs or .vb extension");
            }

            if (provider != null)
            {

                // Format the executable file name.
                // Build the output assembly path using the current directory
                // and <source>_cs.exe or <source>_vb.exe.

                String exeName = String.Format(@"{0}\{1}.exe",
                    System.Environment.CurrentDirectory,
                    sourceFile.Name.Replace(".", "_"));

                CompilerParameters cp = new CompilerParameters();

                // Generate an executable instead of 
                // a class library.
                cp.GenerateExecutable = true;

                // Specify the assembly file name to generate.
                //cp.OutputAssembly = exeName;

                // Generate the assembly in memory.
                cp.GenerateInMemory = true;

                // Set whether to treat all warnings as errors.
                cp.TreatWarningsAsErrors = false;

                // Invoke compilation of the source file.
                CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceName);


                if (cr.Errors.Count > 0)
                {
                    // Display compilation errors.
                    Console.WriteLine("Errors building {0} into {1}",
                        sourceName, cr.PathToAssembly);
                    foreach (CompilerError ce in cr.Errors)
                    {
                        Console.WriteLine("  {0}", ce.ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    // Display a successful compilation message.
                    Console.WriteLine("Source {0} built into {1} successfully.",
                        sourceName, cr.PathToAssembly);
                }

                // Return the results of the compilation.
                if (cr.Errors.Count > 0)
                {
                    compileOk = false;
                }
                else
                {
                    compileOk = true;
                }
            }
            return compileOk;
        }
    }
}
