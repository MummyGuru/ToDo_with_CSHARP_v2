using System;

namespace EntityCore.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDefault { get; set; }
    }
}