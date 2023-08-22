using VideoTools.Utils;

namespace VideoTools.Extensions {
    public static class PathAndFilenameExtensions {
        public static PathAndFilename AsPathAndFilename(this string str) => new(str);
    }
}
