using System.IO;

namespace VirtualDrive
{
    public class DefaultDriveInfo : IDriveInfo
    {
        private DriveInfo _driveInfo;

        public bool IsReady
        {
            get
            {
                if (_driveInfo != null)
                {
                    return _driveInfo.IsReady;
                }

                return false;
            }
        }

        public long TotalSize
        {
            get
            {
                if (_driveInfo != null)
                {
                    return _driveInfo.TotalSize;
                }

                return 0;
            }
        }

        public string VolumeLabel
        {
            get
            {
                if (_driveInfo != null)
                {
                    return _driveInfo.VolumeLabel;
                }

                return string.Empty;
            }
        }

        public void SetDriveLetter(string letter)
        {
            _driveInfo = new DriveInfo(letter);
        }
    }
}