using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Desktop.Models;

namespace Launcher.Desktop.Contracts
{
    public interface IPacksService
    {
        Task<IEnumerable<Pack>> GetAvailablePacksAsync();
        IEnumerable<Pack> GetSavedPacks();
    }
}