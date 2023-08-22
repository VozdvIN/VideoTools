using VideoTools.Cli.Commands;
using System.Text;

namespace VideoTools.Cli {
    abstract public class Command {
        public string Help { get => $"{Name} {ArgumentsTemplateInfo}\n{Description}\n{ArgumentsRoleInfo}\n"; }

        public abstract string Name { get; }

        protected abstract string Description { get; }

        protected abstract string ArgumentsTemplateInfo { get; }

        protected abstract string ArgumentsRoleInfo { get; }

        protected StringBuilder _log = new();

        public static Dictionary<string, Command> Commands => new() {
            { new Insta360x3GroupFiles().Name, new Insta360x3GroupFiles() },
            { new Insta360x3RemovePrefixes().Name, new Insta360x3RemovePrefixes() },
            { new ScriptingBatchDir().Name, new ScriptingBatchDir() }
        };

        public abstract void Execute(CommandArguments args, Action<string> onLog);

        public static Command Factory(string commandName) {
            if (!Commands.ContainsKey(commandName)) {
                throw new ArgumentException(string.Format("AnnotationTools.Cli.Services.Commands.Command.Factory: Command `{0}` is unknown.", commandName));
            }

            return Commands[commandName];
        }

        public static string GetCommandsHelp() {
            var buffer = new StringBuilder();
            buffer.AppendLine("\nAvailable commands:\n");
            foreach (var cmdNameAndCmd in Commands) {
                buffer.AppendLine(cmdNameAndCmd.Value.Help);
            }
            return buffer.ToString();
        }
    }
}
