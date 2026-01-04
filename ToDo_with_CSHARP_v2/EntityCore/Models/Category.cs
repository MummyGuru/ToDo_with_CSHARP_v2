using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_with_CSHARP_v2.EntityCore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDefault { get; set; }
    }
}
