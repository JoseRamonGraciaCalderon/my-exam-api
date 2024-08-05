using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class TreeNodeDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(30, ErrorMessage = "El nombre debe tener menos de 30 caracteres")]
        public string Name { get; set; }

        public List<TreeNodeDto>? Children { get; set; }
    }
}
