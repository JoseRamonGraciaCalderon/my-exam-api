using System.Collections.Generic;

namespace Application.DTOs
{
    public class TreeNodeDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public List<TreeNodeDto>? Children { get; set; }
    }
}
