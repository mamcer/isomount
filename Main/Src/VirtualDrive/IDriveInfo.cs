namespace VirtualDrive
{
    public interface IDriveInfo
    {
        string VolumeLabel { get; }

        long TotalSize { get; }

        bool IsReady { get; }

        void SetDriveLetter(string letter);
    }
}