using System.Diagnostics.CodeAnalysis;

using MySql.Data.MySqlClient;

namespace PriceCheck.DB.Controllers
{
    public class ManyMouthsDb : IDisposable
    {
        public ManyMouthsDb(SecretsFile secretsFile)
        {
            SecretsFile = secretsFile;
        }

        public SecretsFile SecretsFile { get; }

        [AllowNull]
        private MySqlConnection Connection { get; set; }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }

        public MySqlConnection OpenConnection()
        {
            if (Connection is null)
            {
                var sb = new MySqlConnectionStringBuilder
                {
                    Database = "many_mouths",
                    Server = "localhost",
                    Port = 3306,
                    UserID = "root",
                    Password = SecretsFile.DatabasePassword()
                };
                Connection = new MySqlConnection(sb.ToString());
                Connection.Open();
            }

            return Connection;
        }
    }
}