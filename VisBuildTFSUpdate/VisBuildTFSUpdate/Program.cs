using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
// ReSharper disable PossibleNullReferenceException

namespace VisBuildTFSUpdate
{
    internal class Program
    {
        private static void Main()
        {
            string buildFiles;
            buildFiles = @"C:\tfssrc\ibs_main\VisualBuildProjects\Integration_Area";
            //BuildFiles = @"C:\tfssrc\Daily_Builds\Build_70181120\VisualBuildProjects\Integration_Area";
            //BuildFiles = @"C:\tfssrc\Daily_Builds\Build_70181120\VisualBuildProjects\Integration_Area\Impalla";

            //Remove the readonly flag from all 
            foreach (var file in Directory.GetFiles(buildFiles, "*.bld", SearchOption.AllDirectories))
                RemoveReadOnlyFlag(file);


            foreach (var file in Directory.GetFiles(buildFiles, "*.bld", SearchOption.AllDirectories))
            {
                UpdateRetrieveTfsExeVbScript(file);
                UpdateTeamFoundationVisualBuildStepWithPathToTfs(file);
            }
        }

        private static void UpdateTeamFoundationVisualBuildStepWithPathToTfs(string scriptFile)
        {
            var xRoot = XDocument.Load(scriptFile);
            var teamFoundationSteps = xRoot.XPathSelectElements("//step[@action='Team Foundation']");

            foreach (var teamFoundationStep in teamFoundationSteps)
                if (teamFoundationStep.Elements().Where(x => x.Name.LocalName == "Exe").FirstOrDefault() == null)
                {
                    teamFoundationStep.Add(new XElement("Exe", "%TFS_EXE%"));
                }
                else
                {
                    var xelmentExe = teamFoundationStep.Elements().Where(x => x.Name.LocalName == "Exe")
                        .FirstOrDefault();
                    xelmentExe.Value = "%TFS_EXE%";
                }

            xRoot.Save(scriptFile);
        }


        private static void UpdateRetrieveTfsExeVbScript(string scriptFile)
        {
            var xRoot = XDocument.Load(scriptFile);
            var teamFoundationSteps =
                xRoot.XPathSelectElements("//step[@action='Run Script' and name='Retrieve TFS Path']");

            if (teamFoundationSteps.Any())
            {
                var retrievedTFSPath = teamFoundationSteps.First();
                retrievedTFSPath.Element("Script").ReplaceNodes(new XCData(Create_Retrieve_TFS_Base_path_Script()));
            }
            else
            {
                teamFoundationSteps =
                    xRoot.XPathSelectElements("//step[@action='Run Script' and name='Compute TFS BASE Macro']");
                if (teamFoundationSteps.Any())
                {
                    var xElement_Retrieve_TFS_Path = new XElement("step",
                        new XElement("name", "Retrieve TFS Path"),
                        new XElement("Language", "VBScript"),
                        new XElement("Script", new XCData(Create_Retrieve_TFS_Base_path_Script())),
                        new XElement("description", new XCData("")),
                        new XAttribute("action", "Run Script"),
                        teamFoundationSteps.First().Elements().Where(x => x.Name == "indent").First()
                    );

                    //teamFoundationSteps.First().Elements().Where(x => x.Name == "indent").First();

                    teamFoundationSteps.First().AddBeforeSelf(xElement_Retrieve_TFS_Path);
                }
            }


            xRoot.Save(scriptFile);
        }


        private static void RemoveReadOnlyFlag(string scriptFile)
        {
            var attributes = File.GetAttributes(scriptFile);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                // Make the file RW
                attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                File.SetAttributes(scriptFile, attributes);
                Console.WriteLine("The {0} file is no longer RO.", scriptFile);
            }

            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes = RemoveAttribute(attributes, FileAttributes.Hidden);
                File.SetAttributes(scriptFile, attributes);
            }
        }


        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        private static string Create_Retrieve_TFS_Base_path_Script()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("dim fso, f, TFS_EXE \n");
            stringBuilder.Append("\n\n");

            stringBuilder.Append("set fso = createobject(\"Scripting.FileSystemObject\")\n\n");

            stringBuilder.Append(
                "if fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\tf.exe\") then \n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\tf.exe\"\n\n");

            stringBuilder.Append(
                "elseif fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\tf.exe\") then \n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\tf.exe\" \n");

            stringBuilder.Append(
                "elseif fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio 14.0\\Common7\\IDE\\tf.exe\") then\n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%\\Microsoft Visual Studio 14.0\\Common7\\IDE\\tf.exe\"\n\n");

            stringBuilder.Append(
                "elseif fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio 12.0\\Common7\\IDE\\tf.exe\") then\n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%\\Microsoft Visual Studio 12.0\\Common7\\IDE\\tf.exe\"\n\n");

            stringBuilder.Append(
                "elseif fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio 11.0\\Common7\\IDE\\tf.exe\") then\n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%\\Microsoft Visual Studio 11.0\\Common7\\IDE\\tf.exe\"\n\n");

            stringBuilder.Append(
                "elseif fso.FileExists(\"%ProgramFiles%\\Microsoft Visual Studio 10.0\\Common7\\IDE\\tf.exe\") then\n");
            stringBuilder.Append(
                "  \tTFS_EXE = \"%ProgramFiles%\\Microsoft Visual Studio 10.0\\Common7\\IDE\\tf.exe\"\n");

            stringBuilder.Append("end if");

            stringBuilder.Append("\n");
            stringBuilder.Append("builder.LogMessage \"The path to TFS is \" + TFS_EXE \n");
            stringBuilder.Append("Application.Macros(vbldMacroTemporary).Add \"tfs_exe\", TFS_EXE");


            return stringBuilder.ToString();
        }
    }
}