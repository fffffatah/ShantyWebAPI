using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ShantyWebAPI.Providers
{
    public class MysqlConnectionProvider:IDisposable
    {
        MySqlConnection con;
        MySqlCommand command;

        public MysqlConnectionProvider()
        {
            con = new MySqlConnection(Environment.GetEnvironmentVariable("CUSTOMMYSQL_CONN_STRING"));
            con.Open();
            command = new MySqlCommand();
            command.Connection = con;
        }

        public void CreateQuery(string query)
        {
            command.CommandText = query;
            command.CommandTimeout = 15;
            command.CommandType = CommandType.Text;
        }

        //Call this after performing queries to close the established connection
        public void Dispose()
        {
            con.Close();
        }

        //Return query data to calling method that will return query data as JSON and update the DOM
        public MySqlDataReader DoQuery()
        {
            return command.ExecuteReader();
        }

        //Call this CREATE, UPDATE or DELETE
        public int DoNoQuery()
        {
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return -1;
        }
    }
}
