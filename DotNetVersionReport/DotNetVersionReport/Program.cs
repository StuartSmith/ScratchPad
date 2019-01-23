using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DotNetVersionReport
{
    class Program
    {
        static int Main(string[] args)
        {
            string directoryOfAsemblies;
            Dictionary<string, int> dicOfFiles = new Dictionary<string, int>();

            var errorMsg =
                "A directory to run the Dot Net Version report must be included plus at least one wild card of the files to update.\n  " +
                "An example command line would be DoNetVersionReport \n " +
                "\"C:\\Temp\\\" Assemblies*.dll  or DifferentAssmeblyNames*.dll \n " +
                "\"C:\\Temp\\\" Assemblies*.dll   ";

            if (args.Length <= 1)
            {
                throw new Exception(errorMsg);
            }

            directoryOfAsemblies = args[0];

            for (int i=1;i < args.Length; i++)
            {
                var fileNames = Directory.GetFiles(directoryOfAsemblies, args[i]);
                foreach (var fileName in fileNames)
                {
                    dicOfFiles[fileName] = 0;
                }
            }

            
            var assemblyRuntimeValues = dicOfFiles.Select(x=>
                {
                    return new
                    {
                        fileName = Path.GetFileName(x.Key),
                        frameWorkVersion = System.Reflection.Assembly.LoadFrom(x.Key).GetCustomAttributesData().Where(attrib => attrib.AttributeType.Name== "TargetFrameworkAttribute")
                    };
                }
         );

            foreach (var assemblyRuntimeValue in assemblyRuntimeValues.OrderBy(x => x.fileName))
            {
               Console.WriteLine($"fileName={assemblyRuntimeValue.fileName} FrameworkVersion={ assemblyRuntimeValue.frameWorkVersion.FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue}");
            }


            return 0;
        }
    }
}
