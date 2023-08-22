using System.Text;

namespace VideoTools.Utils {
    public class PathAndFilename {
        public string Path { get; protected set; } = "";

        /// <summary>
        /// File name without extension and path
        /// </summary>
        public string BaseName { get; protected set; } = "";

        public string Extension { get; protected set; } = "";

        /// <summary>
        /// File name with extension without path ( BASENAME.EXT )
        /// </summary>
        public string Filename { get => BaseName + (Extension.IsNotEmpty() ? ('.' + Extension) : ""); }

        /// <summary>
        /// File name with extension and path ( PATH \ BASENAME.EXT )
        /// </summary>
        public string FullFilename {
            get {
                var result = new StringBuilder();
                if (Path.IsNotEmpty()) {
                    result.Append(Path);
                    result.Append('\\');
                }
                result.Append(Filename);
                return result.ToString();
            }
        }

        public PathAndFilename(string fullFileName) {
            Parse(fullFileName);
        }

        public PathAndFilename(string path, string basename, string extension = "") {
            Path = path;
            BaseName = basename;
            Extension = extension;
            Parse(FullFilename);
        }

        /// <summary>
        /// Assign path. Base name and extension does not changed.
        /// </summary>
        public PathAndFilename SetPath(string value) {
            Path = value.Replace('/', '\\');
            Parse(FullFilename);
            return this;
        }

        /// <summary>
        /// Assign base name and extension. Path does not changed.
        /// </summary>
        public PathAndFilename SetFilename(string value) {
            value = value.Replace('/', '\\');
            BaseName = ExtractBaseName(value);
            Extension = ExtractExtension(value);
            Parse(FullFilename);
            return this;
        }


        /// <summary>
        /// Assign base name. Path and extension does not changed.
        /// </summary>
        public PathAndFilename SetBasename(string value)
        {
            value = value.Replace('/', '\\');
            BaseName = ExtractBaseName(value);
            Parse(FullFilename);
            return this;
        }

        public PathAndFilename SetExtension(string value) {
            Extension = value.Replace('/', '\\');
            Parse(FullFilename);
            return this;
        }

        /// <summary>
        /// Append existing extension to the end of BaseName, and assign a new extension.
        /// </summary>
        public PathAndFilename AddExtension(string value) {
            if (Extension.IsNotEmpty()) {
                BaseName = BaseName + '.' + Extension;
            }
            Extension = value;
            Parse(FullFilename);
            return this;
        }

        /// <summary>
        /// Change part of the path from it's start
        /// </summary>
        /// <param name="oldPathBase">Begnning part of the path to replace; must correspond to existing path value</param>
        /// <param name="newPathBase"></param>
        public PathAndFilename Rebase(string oldPathBase, string newPathBase) {
            oldPathBase = oldPathBase.Replace('/', '\\');
            if (!Path.StartsWith(oldPathBase)) {
                throw new Exception($"VideoTools.Utils.PathAndFilename.Rebase: Path `{oldPathBase}` is not a base of `{Path}`");
            }
            Path = Path.Replace(oldPathBase, newPathBase.Replace('/', '\\'));
            Parse(FullFilename);
            return this;
        }

        public PathAndFilename Parse(string fullFileName) {
            Path = ExtractPath(fullFileName);
            BaseName = ExtractBaseName(fullFileName);
            Extension = ExtractExtension(fullFileName);
            return this;
        }

        public override string ToString() => FullFilename;

        protected static string ExtractPath(string fullFilename) {
            fullFilename = fullFilename.Replace('/', '\\');
            return fullFilename.Contains('\\') ? fullFilename[..fullFilename.LastIndexOf('\\')] : "";
        }

        protected static string ExtractFilename(string fullFilename) {
            fullFilename = fullFilename.Replace('/', '\\');
            return fullFilename.Contains('\\') ? fullFilename[(fullFilename.LastIndexOf('\\') + 1)..] : fullFilename;
        }

        protected static string ExtractBaseName(string filename) {
            filename = ExtractFilename(filename);
            return filename.Contains('.') ? filename[..filename.LastIndexOf('.')] : filename;
        }

        protected static string ExtractExtension(string filename) {
            filename = ExtractFilename(filename);
            return filename.Contains('.') ? filename[(filename.LastIndexOf('.') + 1)..] : "";
        }
    }
}
