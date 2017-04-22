using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Desktop.Models;

namespace Launcher.Desktop.Contracts
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetNewsAsync();
    }
}