using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RestaurantCuisine
{
    public class DB
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }

        public static void CloseSqlConnection(SqlDataReader reader, SqlConnection connection)
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }

        public static void TableDeleteAll(string tableName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM " + tableName + ";", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Delete(string passedTable, string idType, int targetId)
        {
            SqlConnection conn = Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM @TableParameter WHERE @IdType = @TargetId;", conn);

            cmd.Parameters.Add(new SqlParameter("@TableParameter", passedTable));
            cmd.Parameters.Add(new SqlParameter("@IdType", idType));
            cmd.Parameters.Add(new SqlParameter("@TargetId", targetId));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}
