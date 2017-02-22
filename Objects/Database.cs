using System.Data;
using System.Data.SqlClient;

namespace RestaurantCuisine
{
    public class DB
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
