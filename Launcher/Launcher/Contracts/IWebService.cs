using System;

namespace Launcher.Contracts
{
    public interface IWebService : IDisposable
    {
        string DownloadString(string address);
    }
}