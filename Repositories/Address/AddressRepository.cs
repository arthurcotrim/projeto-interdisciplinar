using Dapper;
using Gerenciamento.Context;
using Gerenciamento.Models;
using System;
using System.Net;

namespace Gerenciamento.Repositories.Address
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _context;

        public AddressRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<string> AddAddress(AddressViewModel address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            string? postalCode = address.PostalCode.Replace("-", "");

            using (var connection = _context.CreateConnection())
            {
                var checkQuery = "SELECT COUNT(*) FROM Address WHERE PostalCode = @PostalCode AND PersonId = @PersonId";
                var existingCount = await connection.ExecuteScalarAsync<int>(checkQuery, new { PostalCode = postalCode, address.PersonId });

                if (existingCount > 0)
                {
                    return "Cadastro já possuí endereço cadastrado";
                }

                var query = $"INSERT INTO Address (PostalCode, Address, City, State, PersonId, DtCreation, DtLastModified, Status)" +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@PostalCode, @Address, @City, @State, @PersonId, GETDATE(), GETDATE(), 0)";

                var parameters = new { postalCode, address.Address, address.City, address.State, address.PersonId };
                int newAddressId = await connection.ExecuteScalarAsync<int>(query, parameters);

                return "";
            }
        }

        public async Task<string?> DeleteAddress(int? id, int? personId)
        {
            if (id == null || id < 0 || personId == null || personId < 0)
            {
                return "Dados incorretos. Verifique os dados enviados";
            }

            using (var connection = _context.CreateConnection())
            {
                var query = "DELETE FROM Address WHERE Id = @Id AND PersonId = @PersonId";
                await connection.ExecuteAsync(query, new { Id = id, PersonId = personId });

                return "";
            }
        }

        public async Task<DataOneAddressViewModel?> GetAddressById(int? id, int? uid)
        {
            if (id <= 0 || !id.HasValue || uid <= 0 || !uid.HasValue)
            {
                return null;
            }

            using (var connection = _context.CreateConnection())
            {
                var verifyQuery = "SELECT COUNT(*) FROM Person WHERE Id = @Uid";
                var resultPerson = await connection.ExecuteScalarAsync<int>(verifyQuery, new { Uid = uid });

                if (resultPerson <= 0)
                {
                    return null;
                }

                var query = "SELECT p.Id AS PersonId, p.Name, a.Id, a.PostalCode, a.Address, a.City, a.State " +
                    "FROM Person AS p " +
                    "LEFT JOIN Address AS a ON p.id = a.PersonId " +
                    "WHERE p.Id = @Uid AND a.Id = @Id";

                var personDictionary = new Dictionary<int, DataOneAddressViewModel>();

                var result = await connection.QueryAsync<DataOneAddressViewModel, AddressViewModel, DataOneAddressViewModel>(
                              query,
                              (person, address) =>
                              {
                                  if (!personDictionary.TryGetValue(person.PersonId.Value, out var dataAddress))
                                  {
                                      dataAddress = person;
                                      dataAddress.AddressViewModel = address; // Adiciona o endereço específico
                                      personDictionary.Add(person.PersonId.Value, dataAddress);
                                  }
                                  return dataAddress;
                              },
                              new { Uid = uid, Id = id },
                              splitOn: "Id"
                              );

                return personDictionary.Values.FirstOrDefault();
            }
        }

        public async Task<DataAddressViewModel?> GetAddressByPersonId(int? id)
        {
            if (id <= 0 || id == null)
            {
                return null;
            }

            using (var connection = _context.CreateConnection())
            {
                var verifyQuery = "SELECT COUNT(*) FROM Person WHERE Id = @Id";
                var resultPerson = await connection.ExecuteScalarAsync<int>(verifyQuery, new { Id = id });

                if (resultPerson <= 0)
                {
                    return null;
                }

                try
                {
                    var query = "SELECT p.Id AS PersonId, p.Name, a.Id, a.PostalCode, a.Address, a.City, a.State " +
                        "FROM Person AS p " +
                        "LEFT JOIN Address AS a ON p.id = a.PersonId " +
                        "WHERE p.Id = @Id";

                    var personDictionary = new Dictionary<int, DataAddressViewModel>();

                    var result = await connection.QueryAsync<DataAddressViewModel, AddressViewModel, DataAddressViewModel>(
                            query,
                            (person, address) =>
                            {
                                if (!personDictionary.TryGetValue(person.PersonId.Value, out var dataAddress))
                                {
                                    dataAddress = person;
                                    dataAddress.AddressViewModel = new List<AddressViewModel>();
                                    personDictionary.Add(person.PersonId.Value, dataAddress);
                                }

                                if (address != null)
                                {
                                    dataAddress.AddressViewModel.Add(address);
                                }

                                return dataAddress;
                            },
                            new { Id = id },
                            splitOn: "Id"
                        );

                    return personDictionary.Values.FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<string> UpdateAddress(AddressViewModel address)
        {
            if (address == null)
            {
                return "Dados Inválidos";
            }

            address.PostalCode = address.PostalCode.Replace("-", "");

            using (var connection = _context.CreateConnection())
            {
                var verifyQuery = "SELECT COUNT(*) FROM Person WHERE Id = @PersonId";
                var result = await connection.ExecuteScalarAsync<int>(verifyQuery, new { PersonId = address.PersonId });

                if (result <= 0)
                {
                    return "Nenhum usuário encontrado no banco de dados";
                }

                var updateQuery = @"
                    UPDATE Address
                    SET PostalCode = @PostalCode,
                        Address = @Address,
                        City = @City,
                        State = @State,
                        DtLastModified = GETDATE()
                    WHERE Id = @Id AND PersonId = @PersonId";

                await connection.ExecuteAsync(updateQuery, new
                {
                    address.PostalCode,
                    address.Address,
                    address.City,
                    address.State,
                    address.Id,
                    address.PersonId,
                });

                return "";
            }
        }
    }
}
