using System.Text;

namespace VideoTools.Utils {
    public static class StringExtensions {
        public const int DEFAULT_STRING_PREVIEW_LENGTH = 32;

        public static bool IsNullOrEmpty(this string str) { return string.IsNullOrEmpty(str); }

        public static bool IsNotEmpty(this string str) { return ! string.IsNullOrEmpty(str); }

        public static bool HasNot(this string str, string subStr) { return !str.Contains(subStr, StringComparison.CurrentCulture); }

        public static IEnumerable<string> TrimAll(this IEnumerable<string> strs) { return strs.Select(s => s.Trim()); }

        public static IEnumerable<string> DropEmpty(this IEnumerable<string> strs) { return strs.Where(s => !s.IsNullOrEmpty()); }

        public static string Slice(this string str, int from, int to) {
            if (from <= 0) {
                from = 0;
            }

            if (to >= str.Length) {
                to = str.Length - 1;
            }

            if (from > to) {
                return "";
            }

            var length = to - from + 1;
            return str.Substring(from, length);
        }

        public static string Replace(this string str, int from, int to, string newFragment) {
            var buf = new StringBuilder();
            buf.Append(str.Slice(0, from - 1));
            buf.Append(newFragment);
            buf.Append(str.Slice(to + 1, str.Length));
            return buf.ToString();
        }

        public static string PreviewStart(this string str, int count = DEFAULT_STRING_PREVIEW_LENGTH) => (count >= str.Length) ? str : str[..(count - 1)];

        public static string PreviewEnd(this string str, int count = DEFAULT_STRING_PREVIEW_LENGTH) => (count >= str.Length) ? str : str.Substring(str.Length - count, count);
    }
}
