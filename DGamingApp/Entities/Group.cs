using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DGamingApp.Entities
{
    public class Group
    {
        public Group()
        {
            
        }
        public Group(string name)
        {   
            Name = name;
        }

        [Key]
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>(); // This is a list of connections that are in the group
    }
}