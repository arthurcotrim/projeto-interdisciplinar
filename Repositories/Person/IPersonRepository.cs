using Gerenciamento.Models;

namespace Gerenciamento.Repositories.Person
{
    public interface IPersonRepository
    {
        Task<int> AddPerson(PersonViewModel person);
        Task<List<PersonViewModel>> GetAllPersons();
        Task<PersonViewModel?> GetPersonById(int? id);
        Task<string> UpdatePerson(PersonViewModel? person);
        Task<string> DeletePerson(int id);
    }
}
