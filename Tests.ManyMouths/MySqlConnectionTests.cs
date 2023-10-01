using PriceCheck.DB.ORM;

namespace Tests.ManyMouths
{
    public class MySqlConnectionTests
    {
        [Test]
        public void CreateConnection()
        {
            var db = new ManyMouthsDb(new PriceCheck.DB.SecretsFile());
            using var connection = db.OpenConnection();
            Assert.IsNotNull(connection);
            connection.Open();
            connection.Close();
        }
    }
}