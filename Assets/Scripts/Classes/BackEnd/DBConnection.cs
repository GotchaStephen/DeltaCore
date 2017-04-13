using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace DeltaCoreBE
{

    public class DBConnection
    {
        private string host;
        private string dbName;
        private string user;
        private string pass;

        private MySqlConnection connection;

        public DBConnection(string h, string db, string u, string p)
        {
            host = h;
            dbName = db;
            user = u;
            pass = p;
            Initialize();
        }

        private void Initialize()
        {
            string connectionString;
            connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", host, dbName, user, pass);

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                string debugMessage = String.Empty;
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        debugMessage = "Cannot connect to server.  Contact administrator\n";
                        debugMessage += String.Format("SERVER:{0},DATABASE:{1},UID:{2}", host, dbName, user);
                        System.Console.WriteLine(debugMessage);
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        debugMessage = "Invalid username/password, please try again";
                        debugMessage += String.Format("USER:{0},PASS:{1}", user, pass);
                        System.Console.WriteLine(debugMessage);
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                string debugMessage = ex.Message;
                System.Console.WriteLine(debugMessage);
                return false;
            }
        }


        public DataTable Select(string query)
        {
            bool dataTableIsEmpty = false;
            DataTable dataSQL = new DataTable();

            if (this.OpenConnection() == true)
            {
                using (MySqlDataAdapter a = new MySqlDataAdapter(query, connection))
                {
                    a.Fill(dataSQL);
                }

                // If the data set has data 
                if (!(dataSQL.Rows.Count > 0))
                {
                    dataTableIsEmpty = true;
                }
                this.CloseConnection();
            }
            else
            {
                string debugMessage = "Failed to open conncetion";
                System.Console.WriteLine(debugMessage);
                return null;
            }
            if (!dataTableIsEmpty)
            {
                return dataSQL;
            }
            return null;
        }

        public bool Modify(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    string debugMessage = String.Empty;
                    debugMessage = ex.Message;
                    System.Console.WriteLine(debugMessage);
                    return false;
                }
                this.CloseConnection();
            }
            else
            {
                string debugMessage = "Failed to open conncetion";
                System.Console.WriteLine(debugMessage);
                return false;
            }
            return true;
        }

        public int InsertReturnID(string query)
        {
            int tempID = 0;
            if (this.OpenConnection() == true)
            {

                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    tempID = (int)cmd.LastInsertedId;
                }
                catch (MySqlException ex)
                {
                    string debugMessage = String.Empty;
                    debugMessage = ex.Message;
                    System.Console.WriteLine(debugMessage);
                    return tempID;
                }
                this.CloseConnection();
            }
            else
            {
                string debugMessage = "Failed to open conncetion";
                System.Console.WriteLine(debugMessage);
                return -1;
            }
            return tempID;
        }

        public int Count(string query)
        {
            int count = -1;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                count = int.Parse(cmd.ExecuteScalar() + "");
                this.CloseConnection();
                return count;
            }
            else
            {
                string debugMessage = "Failed to open conncetion";
                System.Console.WriteLine(debugMessage);
            }
            return count;
        }

    }
}