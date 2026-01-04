using System;

namespace ToDo_with_CSHARP_v2.EntityCore.Models
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Level { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDefault { get; set; }
    }
}
