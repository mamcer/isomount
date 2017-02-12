using System;

namespace VirtualDrive
{
    public class DeviceEventArgs : EventArgs
    {
        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }
    }
}