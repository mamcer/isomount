using System.Diagnostics;

namespace VirtualDrive
{
    public interface IProcessProvider
    {
        Process Start(string fileName, string arguments);
    }
}
