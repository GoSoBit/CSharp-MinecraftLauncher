using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Launcher.Contracts;

namespace Launcher.ViewModels
{
    public sealed class ShellViewModel : Conductor<ITab>, IShell
    {
        public ShellViewModel()
        {
            DisplayName = "Minecraft Launcher";
        }
    }
}
