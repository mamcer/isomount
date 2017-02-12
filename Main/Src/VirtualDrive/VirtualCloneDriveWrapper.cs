using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualDrive
{
    public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);

    public class VirtualCloneDriveWrapper : IVirtualDrive
    {
        private string _unitLetter;

        private IDriveInfo _driveInfo;

        public VirtualCloneDriveWrapper(string unitLetter, string vcdMountPath) : this(unitLetter, vcdMountPath, 5, 1000, new DefaultDriveInfo())
        {
        }

        public VirtualCloneDriveWrapper(string unitLetter, string vcdMountPath, int triesBeforeError, int waitTime, IDriveInfo driveInfo)
        {
            _driveInfo = driveInfo;

            UnitLetter = unitLetter;

            TriesBeforeError = triesBeforeError;

            WaitTime = waitTime;

            VcdMountPath = vcdMountPath;
        }

        public string UnitLetter
        {
            get 
            {
                return _unitLetter; 
            }

            set 
            { 
                _unitLetter = value;
                _driveInfo.SetDriveLetter(_unitLetter);
            }
        }

        public int TriesBeforeError { get; set; }

        public string VcdMountPath { get; set; }

        public string IsoFilePath { get; set; }

        /// <summary>
        /// Wait time in milliseconds to device status change.
        /// </summary>
        public int WaitTime { get; set; }

        public string VolumeLabel
        {
            get
            {
                return _driveInfo != null ? _driveInfo.VolumeLabel : string.Empty;
            }
        }

        public long TotalSize
        {
            get
            {
                return _driveInfo != null ? _driveInfo.TotalSize : 0;
            }
        }

        public async Task<DeviceEventArgs> MountAsync(string isoFilePath)
        {
            IsoFilePath = isoFilePath;
            try
            {
                await Task.Run(new Action(Mount));

                return new DeviceEventArgs { HasError = false, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new DeviceEventArgs { HasError = true, ErrorMessage = ex.Message };
            }
        }

        public async Task<DeviceEventArgs> UnMountAsync()
        {
            try
            {
                await Task.Run(new Action(UnMount));

                return new DeviceEventArgs { HasError = false, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new DeviceEventArgs { HasError = true, ErrorMessage = string.Format("{0} {1}", ex.Message, ex.StackTrace) };
            }
        }

        private void UnMount()
        {
            Process.Start(VcdMountPath, string.Format("/l={0} /u", UnitLetter));

            var i = 0;
            while (_driveInfo.IsReady && i < TriesBeforeError)
            {
                Thread.Sleep(WaitTime);
                i++;
            }

            if (i >= TriesBeforeError)
            {
                throw new Exception(string.Format(Resources.Messages.ErrorUnmountingFile, UnitLetter));
            }
        }

        private void Mount()
        {
            if (!File.Exists(IsoFilePath))
            {
                throw new Exception(string.Format("File '{0}' doesn't exists or don't have access.", IsoFilePath));
            }

            Process.Start(VcdMountPath, string.Format("/l={0} \"{1}\"", UnitLetter, IsoFilePath));

            var i = 0;
            while (!_driveInfo.IsReady && i < TriesBeforeError)
            {
                Thread.Sleep(WaitTime);
                i++;
            }

            if (i >= TriesBeforeError)
            {
                throw new Exception(string.Format(Resources.Messages.ErrorMountingFile, UnitLetter));
            }
        }
    }
}