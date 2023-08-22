namespace VideoTools.Cli {
    public class CommandPlan {
        public Command Command { get; protected set; }

        public CommandArguments Arguments { get; protected set; }

        public CommandPlan(CommandArguments args) {
            if (args.PositionalArgumentCount == 0) {
                throw new ArgumentException("VideoTools.Cli.CommandPlan: Arguments has not command name.");
            }

            Arguments = args;
            Command = Command.Factory(args[0]);
        }

        public void Execute(Action<string> onLog) {
            Command.Execute(Arguments, onLog);
        }

        public static List<CommandPlan> CreateAll(List<CommandArguments> commandArgumentsGroups) {
            var result = new List<CommandPlan>();
            foreach (var argumentGroup in commandArgumentsGroups) {
                try {
                    result.Add(new CommandPlan(argumentGroup));
                }
                catch (Exception e) {
                    Console.WriteLine($"ERROR: Argument group `{argumentGroup}` can not be parsed as command and its arguments: {e.Message}");
                    result.Clear();
                }
            }
            return result;
        }
    }
}
