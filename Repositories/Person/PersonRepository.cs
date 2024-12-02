using Gerenciamento.Models;
using Gerenciamento.Context;
using Dapper;

namespace Gerenciamento.Repositories.Person
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DataContext _context;

        public PersonRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddPerson(PersonViewModel person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            string phone = await RemoveMaskPhone(person.Phone);
            string cpf = await RemoveMaskCPF(person.CPF);

            using (var connection = _context.CreateConnection())
            {
                var checkQuery = "SELECT COUNT(*) FROM Person WHERE Phone = @Phone OR CPF = @Cpf";
                var existingCount = await connection.ExecuteScalarAsync<int>(checkQuery, new { Phone = phone, Cpf = cpf });

                if (existingCount > 0)
                {
                    return -1;
                }

                var query = $"INSERT INTO Person (Name, Phone, CPF, DtCreation, DtLastModified, Status)" +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Name, @Phone, @CPF, GETDATE(), GETDATE(), 0)";

                var parameters = new { person.Name, Phone = phone, CPF = cpf };
                int newPersonId = await connection.ExecuteScalarAsync<int>(query, parameters);

                return newPersonId;
            }
        }

        public async Task<string> DeletePerson(int id)
        {
            if (id == null || id < 0)
            {
                return "Dados incorretos. Verifique os dados enviados";
            }

            using (var connection = _context.CreateConnection())
            {
                var deleteAddressesQuery = "DELETE FROM Address WHERE PersonId = @PersonId";
                await connection.ExecuteAsync(deleteAddressesQuery, new { PersonId = id });

                var query = "DELETE FROM Person WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return "";
            }
        }

        public async Task<List<PersonViewModel>> GetAllPersons()
        {
            using (var connection = _context.CreateConnection())
            {
                var query = "SELECT * FROM Person";
                var result = await connection.QueryAsync<PersonViewModel>(query);

                return result.ToList();
            }
        }

        public async Task<PersonViewModel?> GetPersonById(int? id)
        {
            if (id <= 0 || !id.HasValue)
            {
                return null;
            }

            using (var connection = _context.CreateConnection())
            {
                var query = "SELECT * FROM Person WHERE Id = @Id";
                var result = await connection.QueryAsync<PersonViewModel>(query, new { Id = id });

                return result.FirstOrDefault();
            }
        }

        public async Task<string> UpdatePerson(PersonViewModel? person)
        {
            if (person == null)
            {
                return "Dados Inválidos";
            }

            person.Phone = await RemoveMaskPhone(person.Phone);
            person.CPF = await RemoveMaskCPF(person.CPF);

            using (var connection = _context.CreateConnection())
            {
                var verifyQuery = "SELECT COUNT(*) FROM Person WHERE Id = @Id AND CPF = @CPF AND Phone = @Phone";
                var result = await connection.ExecuteScalarAsync<int>(verifyQuery, new { person.Id, person.CPF, person.Phone });

                if (result <= 0)
                {
                    return "Nenhum usuário encontrado no banco de dados";
                }

                var updateQuery = @"
                    UPDATE Person
                    SET Name = @Name,
                        Phone = @Phone,
                        CPF = @CPF,
                        DtLastModified = GETDATE()
                    WHERE Id = @Id";

                await connection.ExecuteAsync(updateQuery, new
                {
                    person.Name,
                    person.Phone,
                    person.CPF,
                    person.Id
                });

                return "";
            }
        }

        private async Task<string> RemoveMaskCPF(string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }

        private async Task<string> RemoveMaskPhone(string phone)
        {
            return phone.Replace("(", "").Replace("-", "").Replace(")", "").Replace(" ", "");
        }
    }
}
