using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetNewsAsync();
    }
}