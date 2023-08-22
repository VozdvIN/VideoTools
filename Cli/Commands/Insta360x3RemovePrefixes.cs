using VideoTools.Extensions;

namespace VideoTools.Cli.Commands {
    public class Insta360x3RemovePrefixes : Command {
        public override string Name => "insta360:x3:remove-prefixes";

        protected override string Description => "Remove prefixes `IMG_`, `LRV_` & `VID_` from filenames of camera files. WARNING: Command affects source files.";

        protected override string ArgumentsTemplateInfo => "[<work-dir>]";

        protected override string ArgumentsRoleInfo => "<work-dir> - Dir with files to process. Default is program dir.";

        public override void Execute(CommandArguments args, Action<string> onLog)
        {
            var workDirFullPath = args.GetPositionalArgumentOrDefault(1, Environment.CurrentDirectory);

            if (!Directory.Exists(workDirFullPath)) {
                throw new Exception($"Dir `{workDirFullPath}` does not exists.");
            }

            var fileList = Directory.GetFiles(workDirFullPath)
                .Where(filename => {
                    var shortName = filename.AsPathAndFilename().Filename.ToString();
                    return shortName.StartsWith("IMG_") || shortName.StartsWith("LRV_") || shortName.StartsWith("VID_");
                })
                .ToList();

            foreach (var fullFileName in fileList) {
                File.Move(fullFileName, fullFileName.AsPathAndFilename().SetBasename(fullFileName.AsPathAndFilename().BaseName[4..]).ToString());
            }

            onLog($"{fileList.Count} files renamed.");
            onLog("Done.");
        }
    }
}
