using System.Text;
using System.Text.RegularExpressions;

namespace AudioStripper
{
    internal static class FfmpegCommand
    {
        internal static string FormatCommand(string executable, IEnumerable<string> arguments)
        {
            return string.Join(" ", new[] { QuoteArgument(executable, true) }
                .Concat(arguments.Select(argument => QuoteArgument(argument, Path.IsPathFullyQualified(argument)))));
        }

        private static string QuoteArgument(string argument, bool forceQuotes = false)
        {
            if (!forceQuotes && argument.Length > 0 && !argument.Any(c => char.IsWhiteSpace(c) || c == '"'))
                return argument;

            var quoted = new StringBuilder("\"");
            int slashes = 0;
            foreach (char character in argument)
            {
                if (character == '\\')
                {
                    slashes++;
                    continue;
                }
                quoted.Append('\\', character == '"' ? slashes * 2 + 1 : slashes);
                quoted.Append(character);
                slashes = 0;
            }
            quoted.Append('\\', slashes * 2);
            return quoted.Append('"').ToString();
        }

        internal static List<string> BuildArguments(string template, string input, string output)
        {
            var arguments = new List<string> { "-nostdin" };
            foreach (var argument in SplitArguments(template))
            {
                // Substitute once so braces in filenames are never treated as macros.
                arguments.Add(Regex.Replace(argument, @"\{i\}|\{o(?::a)?\}",
                    match => match.Value == "{i}" ? input : output));
            }
            return arguments;
        }

        private static List<string> SplitArguments(string command)
        {
            var result = new List<string>();
            int index = 0;
            while (index < command.Length)
            {
                while (index < command.Length && char.IsWhiteSpace(command[index])) index++;
                if (index == command.Length) break;

                var argument = new StringBuilder();
                bool quoted = false;
                while (index < command.Length && (quoted || !char.IsWhiteSpace(command[index])))
                {
                    int slashes = 0;
                    while (index < command.Length && command[index] == '\\')
                    {
                        slashes++;
                        index++;
                    }

                    if (index < command.Length && command[index] == '"')
                    {
                        argument.Append('\\', slashes / 2);
                        if (slashes % 2 == 1)
                            argument.Append('"');
                        else if (quoted && index + 1 < command.Length && command[index + 1] == '"')
                        {
                            argument.Append('"');
                            index++;
                        }
                        else
                            quoted = !quoted;
                        index++;
                    }
                    else
                    {
                        argument.Append('\\', slashes);
                        if (index < command.Length && (quoted || !char.IsWhiteSpace(command[index])))
                            argument.Append(command[index++]);
                    }
                }

                if (quoted)
                    throw new ArgumentException("FFmpeg parameters contain an unclosed double quote.");
                result.Add(argument.ToString());
            }
            return result;
        }
    }
}
