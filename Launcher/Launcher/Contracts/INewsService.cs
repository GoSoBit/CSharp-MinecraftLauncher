using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetNews();
    }
}
