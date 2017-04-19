using System;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface IPackDialog
    {
        event EventHandler<Pack> ClosedDialog;
    }
}