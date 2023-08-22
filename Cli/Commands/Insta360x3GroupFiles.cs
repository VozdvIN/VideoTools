using VideoTools.Extensions;
using VideoTools.Utils;

namespace VideoTools.Cli.Commands {
    class Insta360x3GroupFiles: Command {
        public override string Name => "insta360:x3:group-files";

        protected override string Description => "Group files related to one shoot. WARNING: Affects source files and dir.";

        protected override string ArgumentsTemplateInfo => "[<work-dir>]";

        protected override string ArgumentsRoleInfo => "<work-dir> - Dir with files to process. Default is program dir.";

        protected class FileMovePlan {
            public required string sourceFullFileName;
            public required string targetFullFileName;
        }

        public override void Execute(CommandArguments args, Action<string> onLog)
        {
            var workDirFullPath = args.GetPositionalArgumentOrDefault(1, Environment.CurrentDirectory);

            if (!Directory.Exists(workDirFullPath)) {
                throw new Exception($"Dir `{workDirFullPath}` does not exists.");
            }

            var fileList = Directory.GetFiles(workDirFullPath)
                .Where(fullFilename => {
                    var extension = fullFilename.AsPathAndFilename().Extension.ToString();
                    return (extension == "insv") || (extension == "lrv") || (extension == "gyro") || (extension == "mp4")
                        || (extension == "dng") || (extension == "insp");
                })
                .ToList();

            var fileMovePlans = new List<FileMovePlan>();

            foreach (var fullFilename in fileList) {
                var sourceFullFilenameWrapper = fullFilename.AsPathAndFilename();
                var targetFullFilenameWrapper = fullFilename.AsPathAndFilename();
                targetFullFilenameWrapper.SetPath($"{sourceFullFilenameWrapper.Path}\\{sourceFullFilenameWrapper.BaseName.CutEnd(7)}");
                fileMovePlans.Add(new FileMovePlan() { sourceFullFileName = sourceFullFilenameWrapper.ToString(), targetFullFileName = targetFullFilenameWrapper.ToString() });
            }

            foreach (var fileMovePlan in fileMovePlans) {
                var targetDir = fileMovePlan.targetFullFileName.AsPathAndFilename().Path;
                
                if (!Directory.Exists(targetDir)) {
                    Directory.CreateDirectory(targetDir);
                }

                File.Move(fileMovePlan.sourceFullFileName, fileMovePlan.targetFullFileName);
            }
        }
    }
}
