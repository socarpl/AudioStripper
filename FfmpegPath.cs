namespace AudioStripper
{
    internal static class FfmpegPath
    {
        internal static string ForConfig(string selectedPath, string? applicationDirectory = null)
        {
            var baseDirectory = Path.GetFullPath(applicationDirectory ?? AppContext.BaseDirectory);
            var fullPath = Path.GetFullPath(selectedPath, baseDirectory);
            var relativePath = Path.GetRelativePath(baseDirectory, fullPath);

            // Keep files outside the application directory as absolute paths.
            if (Path.IsPathRooted(relativePath) || relativePath == ".."
                || relativePath.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                return fullPath;

            return "." + Path.DirectorySeparatorChar + relativePath;
        }

        internal static string Resolve(string location, string? applicationDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(location)) return string.Empty;
            try
            {
                return Path.GetFullPath(location, applicationDirectory ?? AppContext.BaseDirectory);
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return string.Empty;
            }
        }
    }
}
