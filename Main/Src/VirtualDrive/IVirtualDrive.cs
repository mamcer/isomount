using System.Threading.Tasks;

namespace VirtualDrive
{
    public interface IVirtualDrive
    {
        string UnitLetter { get; set; }

        string VolumeLabel { get; }

        long TotalSize { get; }

        Task<DeviceEventArgs> MountAsync(string isoFilePath);

        Task<DeviceEventArgs> UnMountAsync();
    }
}