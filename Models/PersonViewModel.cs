using System.ComponentModel.DataAnnotations;

namespace Gerenciamento.Models
{
    public class PersonViewModel : BaseModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? CPF { get; set; }
    }
}
