using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Models
{
    public class Profile
    {
        public Profile()
        {
        }

        public Profile(string id, string name, bool legacy)
        {
            Id = id;
            Name = name;
            Legacy = legacy;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Legacy { get; set; }
    }
}
