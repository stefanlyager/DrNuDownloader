using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Args.Help;
using Args.Help.Formatters;

namespace DrNuDownloader.Console
{
    public class HelpFormatter : IHelpFormatter
    {
        public void WriteHelp(ModelHelp modelHelp, TextWriter textWriter)
        {
            if (!string.IsNullOrWhiteSpace(modelHelp.HelpText))
            {
                WriteDescription(modelHelp.HelpText, textWriter);
            }

            WriteUsage(modelHelp, textWriter);
            WriteSwitches(modelHelp, textWriter);
        }

        private void WriteSwitches(ModelHelp modelHelp, TextWriter textWriter)
        {
            if (modelHelp == null) throw new ArgumentNullException("modelHelp");
            if (textWriter == null) throw new ArgumentNullException("textWriter");

            foreach (var member in modelHelp.Members)
            {
                var memberSwitch = member.Switches.Single();
                textWriter.Write("  {0}{1}", modelHelp.SwitchDelimiter, memberSwitch);

                var spaces = new string(' ', 3 - memberSwitch.Length);
                textWriter.Write(spaces);

                textWriter.WriteLine("{0}", member.HelpText);
            }
        }

        private void WriteDescription(string helpText, TextWriter textWriter)
        {
            if (helpText == null) throw new ArgumentNullException("helpText");
            if (textWriter == null) throw new ArgumentNullException("textWriter");
            
            textWriter.WriteLine(helpText);
            textWriter.WriteLine();
        }

        private void WriteUsage(ModelHelp modelHelp, TextWriter textWriter)
        {
            if (modelHelp == null) throw new ArgumentNullException("modelHelp");
            if (textWriter == null) throw new ArgumentNullException("textWriter");

            var location = Assembly.GetEntryAssembly().Location;
            textWriter.Write(Path.GetFileNameWithoutExtension(location));

            foreach (var member in modelHelp.Members)
            {
                textWriter.Write(" [{0}]", modelHelp.SwitchDelimiter + member.Switches.Single());
            }

            textWriter.WriteLine(" URL");
            textWriter.WriteLine();
        }
    }
}