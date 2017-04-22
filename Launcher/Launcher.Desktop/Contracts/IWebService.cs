using System;

namespace Launcher.Desktop.Contracts
{
    public interface IWebService : IDisposable
    {
        string DownloadString(string address);
    }
}