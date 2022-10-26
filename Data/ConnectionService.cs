using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BlogProject.Data
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            var databaseurl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseurl) ? connectionString : BuildConnectionString(databaseurl);
        }

        private static string BuildConnectionString(string databaseurl)
        {
            var databaseuri = new Uri(databaseurl);
            var userInfo = databaseuri.UserInfo.Split(':');

            return new NpgsqlConnectionStringBuilder()
            {
                Host = databaseuri.Host,
                Port = databaseuri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseuri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            }.ToString();
        }
    }
}