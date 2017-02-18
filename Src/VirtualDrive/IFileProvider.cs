namespace VirtualDrive
{
    public interface IFileProvider
    {
        bool Exists(string filePath);
    }
}