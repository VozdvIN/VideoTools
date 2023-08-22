using VideoTools.Cli.Commands;
using System.Text;

namespace VideoTools.Cli {
    abstract public class Command {
        public string Help { get => Name + " " + ArgumentsTemplateInfo + "\n" + Description + "\n" + ArgumentsRoleInfo + "\n"; }

        public abstract string Name { get; }

        protected abstract string Description { get; }

        protected abstract string ArgumentsTemplateInfo { get; }

        protected abstract string ArgumentsRoleInfo { get; }

        protected StringBuilder _log = new();

        public static Dictionary<string, Func<Command>> Commands => new() {
                { "scripting:batch:dir", () => new ScriptingBatchDir() }
        };

        public abstract void Execute(CommandArguments args, Action<string> onLog);

        public static Command Factory(string commandName) {
            if (!Commands.ContainsKey(commandName)) {
                throw new ArgumentException(string.Format("AnnotationTools.Cli.Services.Commands.Command.Factory: Command `{0}` is unknown.", commandName));
            }

            return Commands[commandName]();
        }

        public static string GetCommandsHelp() {
            var buffer = new StringBuilder();
            buffer.AppendLine("Available commands:");
            foreach (var cmdNameAndCmd in Commands) {
                buffer.AppendLine("");
                buffer.AppendLine(cmdNameAndCmd.Value().Help);
            }
            return buffer.ToString();
        }
    }
}
