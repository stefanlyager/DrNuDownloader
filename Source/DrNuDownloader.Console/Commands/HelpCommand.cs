using System;
using Args;
using Args.Help;

namespace DrNuDownloader.Console.Commands
{
    public interface IHelpCommand : ICommand { }

    public class HelpCommand : BaseCommand, IHelpCommand
    {
        private readonly IModelBindingDefinition<Arguments> _modelBindingDefinition;

        public HelpCommand(IModelBindingDefinition<Arguments> modelBindingDefinition)
        {
            if (modelBindingDefinition == null) throw new ArgumentNullException("modelBindingDefinition");

            _modelBindingDefinition = modelBindingDefinition;
        }

        public override void Execute()
        {
            base.Execute();

            var helpProvider = new HelpProvider();
            var modelHelp = helpProvider.GenerateModelHelp(_modelBindingDefinition);

            var helpFormatter = new HelpFormatter();
            helpFormatter.WriteHelp(modelHelp, System.Console.Out);
        }
    }
}