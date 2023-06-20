using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Read.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Read.Data.DAL
{
    public class DocumentsDAL : IDocumentsDAL
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DocumentsDAL> _logger;
        private readonly string connString;
        public DocumentsDAL(IConfiguration configuration, ILogger<DocumentsDAL> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var host = _configuration.GetConnectionString("DBHOST") ?? _configuration["DBHOST"];
            var port = _configuration.GetConnectionString("DBPORT") ?? _configuration["DBPORT"];
            var password = _configuration.GetConnectionString("MYSQL_PASSWORD") ?? _configuration["MYSQL_PASSWORD"];
            var userid = _configuration.GetConnectionString("MYSQL_USER") ?? _configuration["MYSQL_USER"];
            var usersDataBase = _configuration["MYSQL_DATABASE"] ?? _configuration.GetConnectionString("MYSQL_DATABASE");
            connString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
        }

        #region Public Methods
        public async Task<List<UsersDto>> GetDocuments()
        {
            _logger.LogInformation("Started GetDocuments");
            var users = new List<UsersDto>();
            try
            {
                string query = @"SELECT * FROM Users";
                using (var connection = new MySqlConnection(connString))
                {
                    var result = await connection.QueryAsync<UsersDto>(query, CommandType.Text);
                    users = result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to fetch data of Users : {connString} ; Exception : {ex.Message}");
            }
            _logger.LogInformation("Finished GetDocuments");
            return users;
        }


        #endregion

    }
}
