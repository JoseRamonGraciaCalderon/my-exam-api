using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var sql = "SELECT * FROM GetUsers()";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var sql = "SELECT * FROM GetUser(@Id)";
            return await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task AddUserAsync(User user)
        {
            var sql = "SELECT AddUser(@Id, @Name, @Email, @PdfFilePath)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var sql = "SELECT UpdateUser(@Id, @Name, @Email, @PdfFilePath)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var sql = "SELECT DeleteUser(@Id)";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
