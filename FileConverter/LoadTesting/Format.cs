namespace FileConverter.LoadTesting
{
    public static class Format
    {
        public static string ElapsedTime(double totalSeconds)
        {
            if (totalSeconds < 60)
                return $"{totalSeconds:0.##} s";
            if (totalSeconds < 3600)
                return $"{totalSeconds / 60:0.##} min";

            return $"{totalSeconds / 3600:0.##} h";
        }


        public static string Bytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double length = bytes;
            int order = 0;

            while (length >= 1024 && order < sizes.Length - 1)
            {
                order++;
                length /= 1024;
            }

            return $"{length:0.##} {sizes[order]}";
        }
    }
}
