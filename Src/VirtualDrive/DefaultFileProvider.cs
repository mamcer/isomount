using System.IO;

namespace VirtualDrive
{
    public class DefaultFileProvider : IFileProvider
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}