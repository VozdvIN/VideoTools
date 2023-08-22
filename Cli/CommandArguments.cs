using System.Text;

namespace VideoTools.Cli
{
    public class CommandArguments {

        const string COMMANDS_SPLITTER = "~~";

        protected List<string> _rawArguments = new();

        protected List<string> _positionalArguments = new();

        protected Dictionary<string, string> _namedArguments = new();

        protected List<string> _flags = new();

        public IEnumerable<string> RawArguments { get => _rawArguments.AsEnumerable(); }

        public string Command { get => _positionalArguments.Any() ? _positionalArguments[0] : ""; }

        public int PositionalArgumentCount { get { return _positionalArguments.Count(); } }

        public string this[int index] {
            get {
                if (index < 0 || index >= PositionalArgumentCount) {
                    throw new Exception(string.Format("VideoTools.Cli.CommandArguments[int]: Positional argument index #{0} is out of range [0;{1}].", index, PositionalArgumentCount));
                }

                return _positionalArguments[index];
            }
        }

        public string this[string name] {
            get {
                if (!HasNamedArgument(name)) {
                    throw new Exception(string.Format("VideoTools.Cli.CommandArguments[string]: Named argument `{0}` not defined", name));
                }

                return _namedArguments[name.ToUpperInvariant()];
            }
        }

        public CommandArguments(List<string> rawArguments) {
            if (!rawArguments.Any()) {
                throw new Exception("VideoTools.Cli.CommandArguments: Empty source array");
            }

            foreach (var rawArg in rawArguments) {
                _rawArguments.Add(rawArg);
                if (LooksLikeNameArg(rawArg)) {
                    var parts = rawArg.Split('=').ToList();
                    var namedArgName = parts[0][2..].ToUpperInvariant();
                    var namedArgValue = parts[1];
                    if (HasNamedArgument(namedArgName)) {
                        throw new Exception(string.Format("VideoTools.Cli.CommandArguments: Redefinition of named argument `{0}`", namedArgName));
                    }
                    _namedArguments.Add(namedArgName.ToUpperInvariant(), namedArgValue);
                }
                else if (LooksLikeFlag(rawArg)) {
                    var flagName = rawArg.Substring(1);
                    if (HasFlag(flagName)) {
                        throw new Exception(string.Format("VideoTools.Cli.CommandArguments: Redefinition of flag `{0}`", flagName));
                    }
                    _flags.Add(flagName.ToUpperInvariant());
                }
                else {
                    _positionalArguments.Add(rawArg);
                }
            }
        }

        public bool HasNamedArgument(string name) {
            return _namedArguments.ContainsKey(name.ToUpperInvariant());
        }

        public bool HasFlag(string name) {
            return _flags.Any(a => a.ToUpperInvariant() == name.ToUpperInvariant());
        }

        public string GetPositionalArgumentOrDefault(int argIndex, string defaultValue = "") {
            return (argIndex < PositionalArgumentCount && argIndex >= 0) ? this[argIndex] : defaultValue;
        }

        public string GetNamedArgumentOrDefault(string argName, string defaultValue = "") {
            return HasNamedArgument(argName) ? this[argName] : defaultValue;
        }

        public override string ToString() {
            var result = new StringBuilder();
            var hasData = false;

            foreach (var posArg in _positionalArguments) {
                if (hasData) {
                    result.Append(' ');
                }
                result.Append('"');
                result.Append(posArg);
                result.Append('"');
                hasData = true;
            }

            foreach (var namedArg in _namedArguments) {
                if (hasData) {
                    result.Append(' ');
                }
                result.Append("--");
                result.Append(namedArg.Key);
                result.Append("=\"");
                result.Append(namedArg.Value);
                result.Append('"');
                hasData = true;
            }

            foreach (var flag in _flags) {
                if (hasData) {
                    result.Append(' ');
                }
                result.Append('-');
                result.Append(flag);
            }

            return result.ToString();
        }

        public static List<CommandArguments> ParseGroups(List<string> rawArgs) {
            var result = new List<CommandArguments>();
            var argumentsBuffer = new List<string>();
            foreach (var rawArg in rawArgs) {
                if (rawArg == COMMANDS_SPLITTER) {
                    if (argumentsBuffer.Any()) {
                        result.Add(new CommandArguments(argumentsBuffer));
                        argumentsBuffer = new List<string>();
                    }
                }
                else {
                    argumentsBuffer.Add(rawArg);
                }
            }
            if (argumentsBuffer.Count > 0) {
                result.Add(new CommandArguments(argumentsBuffer));
            }
            return result;
        }

        protected static bool LooksLikeNameArg(string cmdLineArg) {
            return (cmdLineArg.Length > 3)
                && (cmdLineArg[0] == '-')
                && (cmdLineArg.Split('=').Length == 2);
        }

        protected static bool LooksLikeFlag(string cmdLineArg) {
            return (cmdLineArg.Length > 1)
                && (cmdLineArg[0] == '-')
                && (cmdLineArg.Split('=').Length == 1);
        }
    }
}
