using System;

namespace ToDo_with_CSHARP_v2.EntityCore.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
