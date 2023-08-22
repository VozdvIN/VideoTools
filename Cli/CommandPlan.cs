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
    }
}
