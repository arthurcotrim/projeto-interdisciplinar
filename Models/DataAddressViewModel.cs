namespace Gerenciamento.Models
{
    public class DataAddressViewModel
    {
        public string? Name { get; set; }
        public int? PersonId { get; set; }
        public List<AddressViewModel?>? AddressViewModel { get; set; }
    }

    public class DataOneAddressViewModel
    {
        public string? Name { get; set; }
        public int? PersonId { get; set; }
        public AddressViewModel? AddressViewModel { get; set; }
    }
}
