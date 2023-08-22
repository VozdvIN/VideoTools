using VideoTools.Cli;
using VideoTools.Cli.Commands;
using VideoTools.Extensions;

namespace VideoTools {
    internal class VideoTools {
        const string VERSION = "0.0.1b";

        static void Main(string[] args) {
            Console.WriteLine($"Video Tools CLI, v{VERSION}, (C) Ilya Vozdvijensky, 2023\n");

            var argumentGroups = CommandArguments.ParseGroups(args.ToList());

            if (argumentGroups.Count == 0) {
                Console.Write(Command.GetCommandsHelp());
                return;
            }

            var commandPlans = CreateCommandPlans(argumentGroups);

            if (commandPlans.Count == 0) {
                Console.Write(Command.GetCommandsHelp());
                return;
            }

            try {
                if (commandPlans[0].Command.Name == "scripting:batch:dir") {
                    var batchCommandPlan = commandPlans[0];
                    commandPlans.RemoveAt(0);

                    var commandFullName = "VideoTools.Cli.Commands.ScriptingBatchDir";

                    if (batchCommandPlan.Arguments.PositionalArgumentCount < 2) {
                        throw new Exception($"{commandFullName}: Source directory not specified.");
                    }

                    var sourceDirArg = batchCommandPlan.Arguments[1];

                    if (!Directory.Exists(sourceDirArg)) {
                        throw new Exception($"{commandFullName}: Source directory not found.");
                    }

                    if (batchCommandPlan.Arguments.PositionalArgumentCount < 3) {
                        throw new Exception($"{commandFullName}: Target directory not specified.");
                    }

                    var targetDirArg = batchCommandPlan.Arguments[2];

                    if (batchCommandPlan.Arguments.HasFlag("f") && Directory.Exists(targetDirArg)) {
                        Directory.Delete(targetDirArg, true);
                    }

                    if (!Directory.Exists(targetDirArg)) {
                        Directory.CreateDirectory(targetDirArg);
                    }

                    var sourceFiles = Directory.GetFiles(sourceDirArg);

                    foreach (var sourceFile in sourceFiles) {
                        var sourceFileInfo = sourceFile.AsPathAndFilename();

                        var sourceDir = sourceDirArg + "\\";
                        var sourceFullFileName = sourceFileInfo.FullFilename;
                        var sourceFileName = sourceFileInfo.Filename;
                        var sourceBaseName = sourceFileInfo.BaseName;
                        var targetDir = targetDirArg + "\\";

                        string argumentConverter(string arg) =>
                            arg
                            .Replace(ScriptingBatchDir.SOURCE_DIR_ALIAS, sourceDir)
                            .Replace(ScriptingBatchDir.SOURCE_FILE_ALIAS, sourceFullFileName)
                            .Replace(ScriptingBatchDir.SOURCE_FILENAME_ALIAS, sourceFileName)
                            .Replace(ScriptingBatchDir.SOURCE_BASENAME_ALIAS, sourceBaseName)
                            .Replace(ScriptingBatchDir.TARGET_DIR_ALIAS, targetDir);

                        foreach (var commandPlan in commandPlans) {
                            var localCommandPlan = new CommandPlan(new CommandArguments(commandPlan.Arguments.RawArguments.Select(argumentConverter).ToList()));
                            Console.WriteLine("Executing `{0}`:", localCommandPlan.Arguments.ToString());
                            localCommandPlan.Execute(Console.WriteLine);
                        }
                    }
                }
                else {
                    foreach (var commandPlan in commandPlans) {
                        Console.WriteLine("Executing `{0}`:", commandPlan.Arguments.ToString());
                        commandPlan.Execute(Console.WriteLine);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine($"ERROR: {e.Message}");
                Console.WriteLine("WARNING: Subsequent commands execution is cancelled.");
            }
        }

        protected static List<CommandPlan> CreateCommandPlans(List<CommandArguments> argumentGroups) {
            var result = new List<CommandPlan>();
            foreach (var argumentGroup in argumentGroups) {
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
