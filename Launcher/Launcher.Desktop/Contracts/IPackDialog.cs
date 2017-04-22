using System;
using Launcher.Desktop.Models;

namespace Launcher.Desktop.Contracts
{
    public interface IPackDialog
    {
        event EventHandler<Pack> ClosedDialog;
    }
}