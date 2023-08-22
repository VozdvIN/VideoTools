namespace VideoTools.Cli.Commands {
    public class ScriptingBatchDir: Command {
        public const string SOURCE_DIR_ALIAS = "$$SOURCE_DIR$$";
        public const string SOURCE_FILE_ALIAS = "$$SOURCE_FILE$$";
        public const string SOURCE_FILENAME_ALIAS = "$$SOURCE_FILENAME$$";
        public const string SOURCE_BASENAME_ALIAS = "$$SOURCE_BASENAME$$";
        public const string TARGET_DIR_ALIAS = "$$TARGET_DIR$$";

        public override string Name => "scripting:batch:dir";

        protected override string Description => $"If stands in the first place of the query performs execution of the other commands in the query for each file from specified dir, otherwise do nothing." +
            $"Provide macro environment variables to use in query command arguments: `{SOURCE_DIR_ALIAS}`, `{SOURCE_FILE_ALIAS}`, `{SOURCE_FILENAME_ALIAS}`, `{SOURCE_BASENAME_ALIAS}`, `{TARGET_DIR_ALIAS}`";

        protected override string ArgumentsTemplateInfo => "<source_dir> <target_dir> [-f]";

        protected override string ArgumentsRoleInfo =>
            "<source_dir> - directory with source files;\n" +
            "<target_sir> - directory for output files;\n" +
            "-f - force cleanup target directory.";

        public override void Execute(CommandArguments args, Action<string> onLog) {
            /* Not intended to be executed directly */
        }
    }
}
