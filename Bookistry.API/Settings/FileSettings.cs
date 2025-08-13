namespace Bookistry.API.Settings;

public static class FileSettings
{
    public partial class ImageSettings
    {
        public const int MaxSizeInMB = 15;
        public const int MaxSizeInBytes = MaxSizeInMB * 1024 * 1024;
        public static readonly string[] AllowedSignuture = ["89-50","FF-D8","47-49","42-4D"];
    }
    public partial class PdfSettings
    {
        public const int MaxSizeInMB = 100;
        public const int MaxSizeInBytes = MaxSizeInMB * 1024 * 1024;
        public static readonly string AllowedSignuture = "25-50-44-46"; // pdf
    }
}
