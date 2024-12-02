using Gerenciamento.Models;

namespace Gerenciamento.Repositories.Address
{
    public interface IAddressRepository
    {
        Task<string> AddAddress(AddressViewModel address);
        Task<DataAddressViewModel?> GetAddressByPersonId(int? id);
        Task<DataOneAddressViewModel?> GetAddressById(int? id, int? uid);
        Task<string> UpdateAddress(AddressViewModel address);
        Task<string> DeleteAddress(int? id, int? personId);
    }
}
