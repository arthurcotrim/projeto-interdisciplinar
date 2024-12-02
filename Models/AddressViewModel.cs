namespace Gerenciamento.Models
{
    public class AddressViewModel : BaseModel
    {
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? PersonId { get; set; }  
        public string? Name { get; set; }
    }
}
