namespace AudioStripper
{
    internal static class AppIcon
    {
        internal static Icon Value { get; } = Load();

        private static Icon Load()
        {
            using var stream = typeof(AppIcon).Assembly.GetManifestResourceStream("AudioStripper.audiostripper.ico")
                ?? throw new InvalidOperationException("The application icon resource is missing.");
            using var icon = new Icon(stream);
            return (Icon)icon.Clone();
        }
    }
}
