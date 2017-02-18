using System.Diagnostics;

namespace VirtualDrive
{
    public class DefaultProcessProvider : IProcessProvider
    {
        public Process Start(string fileName, string arguments)
        {
            return Process.Start(fileName, arguments);
        }
    }
}